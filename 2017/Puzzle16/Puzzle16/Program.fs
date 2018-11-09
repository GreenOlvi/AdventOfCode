open System
open System.IO
open Utils

type Program = P of char
type Move =
    | Spin of int
    | Exchange of int * int
    | Partner of Program * Program

let (|Prefix|_|) (p:string) (s:string) =
    if s.StartsWith(p) then
        Some(s.Substring(p.Length))
    else
        None

let parsePositions text : int * int =
    let num = text |> Common.splitString [|'/'|] |> Array.map int
    (num.[0], num.[1])

let parseNames text : Program * Program =
    let str = text |> Common.splitString [|'/'|] |> Array.map char
    (P str.[0], P str.[1])

let parseMove (text:string) =
    match text with
    | Prefix "s" rest -> Spin (int rest)
    | Prefix "x" rest -> Exchange (parsePositions rest)
    | Prefix "p" rest -> Partner (parseNames rest)
    | _ -> invalidArg "text" "Invalid value in text"

let parseInput input : Move list =
    input
    |> Common.splitString [|','|]
    |> Array.map parseMove
    |> List.ofArray

let spin (i:int) (input:Program list) : Program list =
    let l = i % List.length input
    let pre = input |> List.skip (input.Length - l) |> List.take l
    let rest = input |> List.take (input.Length - l)
    List.concat [pre; rest]

let exchange (a:int) (b:int) (input:Program list) : Program list =
    if a = b then
        input
    else
        let left = Math.Min(a, b)
        let right = Math.Max(a, b)
        List.concat [
            input |> List.take left;
            [input.[right]];
            input |> List.skip (left + 1) |> List.take (right - left - 1);
            [input.[left]];
            input |> List.skip (right + 1) |> List.take (List.length input - right - 1)
        ]

let partner (a:Program) (b:Program) (input:Program list) : Program list =
    let iA = List.findIndex (fun e -> e = a) input
    let iB = List.findIndex (fun e -> e = b) input
    exchange iA iB input

let runInstruction (instruction:Move) (input:Program list) : Program list =
    match instruction with
    | Spin i -> spin i input
    | Exchange (a, b) -> exchange a b input
    | Partner (a, b) -> partner a b input

let runInstructions (input:Program list) (instructions:Move list) : Program list =
    List.fold (fun s i -> runInstruction i s) input instructions

let programList count =
    seq {97..count + 97 - 1} |> Seq.map (fun i -> P (char i)) |> List.ofSeq

let readInput file = (File.ReadAllText file).Trim()

let iter2String (input:Program list) : string =
    input |> List.map (fun (P p) -> string p) |> String.concat ""

let string2Iter (input:string) : Program list =
    input |> Seq.map (fun p -> P (char p)) |> List.ofSeq

let solve1 instructions =
    let r = runInstructions (programList 16) instructions
    iter2String r

let solve2 instructions =
    let rec run (input:string) cache (count:int) : string =
        //if (count % 10_000_000 = 0) then printfn "%A%%" ((1000_000_000 - count) / 10_000_000)
        match count with
        | 0 ->
            input
        | _ ->
            let i = input
            match Map.containsKey i cache with
            | true ->
                run cache.[i] cache (count - 1)
            | false -> 
                let iter = runInstructions (string2Iter input) instructions |> iter2String
                run iter (cache.Add(i, iter)) (count - 1)
    run (programList 16 |> iter2String) Map.empty 1000_000_000

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt" |> parseInput

    Common.test parseInput [
        ("s2", [Spin 2]);
        ("x3/4", [Exchange (3, 4)]);
        ("pe/b", [Partner (P 'e', P 'b')]);
        ("s1,x3/4,pe/b", [Spin 1; Exchange (3, 4); Partner (P 'e', P 'b')]);
    ]

    Common.test programList [
        (5, [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']);
    ]

    Common.test (fun i -> spin i [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']) [
        (0, [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']);
        (1, [P 'e'; P 'a'; P 'b'; P 'c'; P 'd']);
        (2, [P 'd'; P 'e'; P 'a'; P 'b'; P 'c']);
        (3, [P 'c'; P 'd'; P 'e'; P 'a'; P 'b']);
        (6, [P 'e'; P 'a'; P 'b'; P 'c'; P 'd']);
    ]

    Common.test (fun (a, b) -> exchange a b [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']) [
        ((0, 0), [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']);
        ((0, 1), [P 'b'; P 'a'; P 'c'; P 'd'; P 'e']);
        ((0, 2), [P 'c'; P 'b'; P 'a'; P 'd'; P 'e']);
        ((0, 4), [P 'e'; P 'b'; P 'c'; P 'd'; P 'a']);
        ((2, 1), [P 'a'; P 'c'; P 'b'; P 'd'; P 'e']);
        ((2, 4), [P 'a'; P 'b'; P 'e'; P 'd'; P 'c']);
    ]

    Common.test (fun (a, b) -> partner a b [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']) [
        ((P 'b', P 'b'), [P 'a'; P 'b'; P 'c'; P 'd'; P 'e']);
        ((P 'a', P 'b'), [P 'b'; P 'a'; P 'c'; P 'd'; P 'e']);
        ((P 'a', P 'e'), [P 'e'; P 'b'; P 'c'; P 'd'; P 'a']);
        ((P 'e', P 'a'), [P 'e'; P 'b'; P 'c'; P 'd'; P 'a']);
        ((P 'c', P 'd'), [P 'a'; P 'b'; P 'd'; P 'c'; P 'e']);
    ]

    Common.test (fun i -> runInstructions (programList 5) i) [
        ([Spin 1; Exchange (3, 4); Partner (P 'e', P 'b')], [P 'b'; P 'a'; P 'e'; P 'd'; P 'c']);
    ]

    let result1 = Common.timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    let result2 = Common.timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
