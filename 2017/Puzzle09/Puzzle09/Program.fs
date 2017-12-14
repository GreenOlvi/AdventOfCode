open System
open System.IO
open Utils.Common

type Parser<'a> = char list -> ('a * char list) option

type Group = Group of Group list
type Garbage = unit

let map f p = p >> Option.map (fun (x, rest) -> (f x), rest)

let cons a b = a::b

let rec parseGroup:Group Parser =
    let rec parseInner =
        function
        | '}'::rest -> Some ([], rest)
        | ','::text
        | text ->
            match parseGroupOrGarbage text with
            | Some (Choice1Of2 group, rst) -> map (cons group) parseInner rst
            | Some (Choice2Of2 _, rst) -> parseInner rst
            | None -> None

    function
    | '{'::inner -> map Group parseInner inner
    | _ -> None

and parseGarbage:Garbage Parser =
   let rec parseInner chs =
       match chs with
       | '!'::_::rest -> parseInner rest
       | '>'::rest -> Some((), rest)
       | _::rest -> parseInner rest

   function
   | '<'::inner -> parseInner inner
   | _ -> None

and parseGroupOrGarbage: Choice<Group, Garbage> Parser =
    function
    | '{'::inner -> map Choice1Of2 parseGroup ('{'::inner)
    | '<'::inner -> map Choice2Of2 parseGarbage ('<'::inner)
    | _ -> None

let score g:int =
    let rec innerScore n (Group groups) =
        n + Seq.sumBy (innerScore (n+1)) groups
    innerScore 1 g

let readInput file = File.ReadAllText(file).Trim() |> List.ofSeq

let solve1 input =
    let g = input |> parseGroup
    match g with
    | Some (group, _) -> score group
    | None -> 0

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    test parseGroupOrGarbage [
        ("{}" |> List.ofSeq), Some (Choice1Of2 (Group []), []);
        ("{{}}" |> List.ofSeq), Some (Choice1Of2 (Group [Group []]), []);
        ("{{},{}}" |> List.ofSeq), Some (Choice1Of2 (Group [Group []; Group []]), []);
        ("{{{}},{}}" |> List.ofSeq), Some (Choice1Of2 (Group [Group [Group []]; Group []]), []);
    ]

    test score [
        ((Group []), 1);
        ((Group [Group []]), 3);
        ((Group [Group [Group []]]), 6);
        ((Group [Group []; Group []]), 5);
    ]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    Console.ReadLine() |> ignore
    0 // return an integer exit code
