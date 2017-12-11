open System
open System.IO
open Utils.Common

type Hex = int * int
type Direction = N | NE | SE | S | SW | NW

let dirMap = dict [(N, (0, -1)); (NE, (1, -1)); (SE, (1, 0)); (S, (0, 1)); (SW, (-1, 1)); (NW, (-1, 0));]
let addHex ((aq, ar):Hex) ((bq, br):Hex) = (aq + bq, ar + br)
let dist ((aq, ar):Hex) ((bq, br):Hex):int = (abs(aq - bq) + abs(aq + ar - bq - br) + abs(ar - br)) / 2

let toDirection (s:string) =
    match s.ToLower() with
    | "n" -> N
    | "ne" -> NE
    | "se" -> SE
    | "s" -> S
    | "sw" -> SW
    | "nw" -> NW
    | _ -> invalidArg "s" "Invalid direction"

let parseInput =
    splitString [| '\n'; '\r'; ' '; '\t'; ',' |]
    >> Array.map toDirection
    >> Array.toList

let readInput file = File.ReadAllText file |> parseInput

let solve1 input =
    let rec loop pos directions =
        match directions with
        | [] -> pos
        | h::t -> loop (addHex pos dirMap.[h]) t
    let endPos = loop (0, 0) input
    dist (0, 0) endPos

let solve2 input =
    input
    |> List.fold (
        fun (p, max) dir ->
            let p' = addHex p dirMap.[dir]
            (p', Math.Max(max, dist (0, 0) p')
        )) ((0, 0), 0)
    |> snd

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    printfn "--- Tests ---\n"
    test solve1 [
        (parseInput "ne,ne,ne", 3);
        (parseInput "ne,ne,sw,sw", 0);
        (parseInput "ne,ne,s,s", 2);
        (parseInput "se,sw,se,sw,sw", 3);
    ]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A\n" result1

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
