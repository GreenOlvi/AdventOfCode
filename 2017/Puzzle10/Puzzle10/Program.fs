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

let round initialState input = input |> List.fold step initialState

let rounds count initialState input =
    let rec r state i =
        match i with
        | 0 -> state
        | _ ->
            let s = round state input
            r s (i - 1)
    r initialState count

let parseInput =
    splitString [| '\n'; '\r'; ' '; '\t'; ',' |]
    >> Array.map Int32.Parse
    >> List.ofArray

let toAscii (string:String) =
    System.Text.ASCIIEncoding.ASCII.GetBytes(string)
    |> List.ofArray
    |> List.map Convert.ToInt32

let xor values = List.fold (^^^) 0 values

let bytesToHex (bytes:int list) =
    bytes
    |> List.map (fun (b:int) -> System.String.Format("{0:x2}", b))
    |> String.concat String.Empty

let readInput file = File.ReadAllText(file).Trim()

let initialState = { position = 0; skipSize = 0; list = List.ofSeq {0..255}}

let solve1 input =
    let parsed = input |> parseInput
    let state = round initialState parsed
    List.item 0 state.list * List.item 1 state.list

let solve2 input =
    let newInput = List.concat [input |> toAscii; [17; 31; 73; 47; 23]]
    let state = rounds 64 initialState newInput
    let chunks = state.list |> List.chunkBySize 16
    chunks |> List.map xor |> bytesToHex

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

    test (round {position = 0; skipSize = 0; list = List.ofSeq {0..4}}) [
        ([3; 4; 1; 5], {position = 4; skipSize = 4; list = [3; 4; 2; 1; 0]});
    ]

    test toAscii [
        ("0", [48]);
        ("1,2,3", [49; 44; 50; 44; 51])
    ]

    test xor [
        ([65; 27; 9; 1; 4; 3; 40; 50; 91; 7; 6; 0; 2; 5; 68; 22], 64);
    ]

    test solve2 [
        ("", "a2582a3a0e66e6e86e3812dcb672a272");
        ("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd");
        ("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d");
        ("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e");
    ]

    let input = readInput "input.txt"

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    let result2 = solve2 input
    printfn "Result 2: %s" result2


    Console.ReadLine() |> ignore
    0 // return an integer exit code
