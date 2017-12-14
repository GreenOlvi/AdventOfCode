open System
open System.IO
open Utils.Common

type Parser<'a> = char list -> ('a * char list) option

type Group = Group of Group list
type Garbage = unit

let map f p = p >> Option.map (fun (x, rest) -> (f x), rest)

let cons a b = a::b

// Group parser
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

// Garbager parser
type Garbager = char list

let rec parseGarbagerFromGroup: Garbager Parser =
    let rec parseInner =
        function
            | '}'::rest -> Some ([], rest)
            | ','::text
            | text ->
                match parseGroupOrGarbager text with
                | Some (garbage, rest) -> map (List.append garbage) parseInner rest
                | None -> None
    function
        | '{'::inner -> parseInner inner
        | _ -> None

and parseGarbager: Garbager Parser =
    let rec parseInner =
        function
            | '!'::_::rest -> parseInner rest
            | '>'::rest -> Some ([], rest)
            | c::rest -> map (cons c) parseInner rest
    function
        | '<'::inner -> parseInner inner
        | _ -> None

and parseGroupOrGarbager: Garbager Parser =
    function
        | '{'::inner -> parseGarbagerFromGroup ('{'::inner)
        | '<'::inner -> parseGarbager ('<'::inner)
        | _ -> None

let readInput file = File.ReadAllText(file).Trim() |> List.ofSeq

let solve1 input =
    let g = input |> List.ofSeq |> parseGroup
    match g with
    | Some (group, _) -> score group
    | None -> 0

let solve2 input =
    let g = input |> List.ofSeq |> parseGroupOrGarbager
    match g with
    | Some (garbage, _) -> garbage.Length
    | None -> 0

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    printfn "--- Tests ---\n"
    test parseGroupOrGarbage [
        ("{}" |> List.ofSeq, Some (Choice1Of2 (Group []), []));
        ("{{}}" |> List.ofSeq, Some (Choice1Of2 (Group [Group []]), []));
        ("{{},{}}" |> List.ofSeq, Some (Choice1Of2 (Group [Group []; Group []]), []));
        ("{{{}},{}}" |> List.ofSeq, Some (Choice1Of2 (Group [Group [Group []]; Group []]), []));
    ]

    test score [
        ((Group []), 1);
        ((Group [Group []]), 3);
        ((Group [Group [Group []]]), 6);
        ((Group [Group []; Group []]), 5);
    ]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    printfn "--- Tests ---\n"
    test solve2 [
        ("{}", 0);
        ("<>", 0);
        ("{<random characters>}", 17);
        ("<<<<>", 3);
        ("<!!>", 0);
    ]

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
