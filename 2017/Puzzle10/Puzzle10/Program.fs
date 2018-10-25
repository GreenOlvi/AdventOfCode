open System
open System.IO
open Utils.Common

type State = { position:int; skipSize:int; list:int list }

let reverseFragment start length l =
    let L = Seq.length l
    if length > L then
        invalidArg "length" "Fragment length should be shorter than list"
    let boundStart = start % L
    let double = Seq.concat [|l; l|]
    let preFragment = double |> Seq.take boundStart
    let postFragment = double |> Seq.skip (boundStart + length)
    let fragment = double |> Seq.skip boundStart |> Seq.take length |> Seq.rev
    let newDouble = Seq.concat [|preFragment; fragment; postFragment|]
    let overflow =
        match (start + length > L) with
        | true -> start + length - L
        | false -> 0
    let newStart =
        match overflow with
        | 0 -> Seq.empty
        | o -> newDouble |> Seq.skip L |> Seq.take o
    let newEnd = newDouble |> Seq.skip overflow |> Seq.take (L - overflow)
    Seq.concat [newStart; newEnd] |> List.ofSeq

let step (s:State) l :State =
    let newList = reverseFragment s.position l s.list
    {
        position = (s.position + l + s.skipSize) % (List.length s.list);
        skipSize = s.skipSize + 1;
        list = newList;
    }

let solve size input =
    let lastState = input |> List.fold step { position = 0; skipSize = 0; list = List.ofSeq {0..size}}
    List.item 0 lastState.list * List.item 1 lastState.list

let solve1 = (solve 255)

let parseInput =
    splitString [| '\n'; '\r'; ' '; '\t'; ',' |]
    >> Array.map Int32.Parse
    >> List.ofArray

let readInput file =
    File.ReadAllText file |> parseInput

[<EntryPoint>]
let main _ =

    printfn "--- Tests ---\n"
    test parseInput [
        ("0,1,2,3,4", [0; 1; 2; 3; 4])
    ]

    test3 reverseFragment [
        ((0, 1, [0; 1; 2; 3; 4]), [0; 1; 2; 3; 4]);
        ((0, 2, [0; 1; 2; 3; 4]), [1; 0; 2; 3; 4]);
        ((0, 3, [0; 1; 2; 3; 4]), [2; 1; 0; 3; 4]);
        ((1, 3, [0; 1; 2; 3; 4]), [0; 3; 2; 1; 4]);
        ((3, 3, [0; 1; 2; 3; 4]), [3; 1; 2; 0; 4]);
        ((3, 4, [0; 1; 2; 3; 4]), [4; 3; 2; 1; 0]);
    ]

    test (solve 4) [
        ([3; 4; 1; 5], 12);
    ]

    let input = readInput "input.txt"

    let result = solve1 input
    printfn "%A" result

    Console.ReadLine() |> ignore
    0 // return an integer exit code
