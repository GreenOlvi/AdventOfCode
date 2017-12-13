open System
open System.IO
open Utils.Common

type Input = (int * int) list

let scanner r t =
    let off = t % (r - 1)
    let dir = (t / (r - 1)) % 2
    match dir = 0 with
    | true -> off
    | false -> r - 1 - off

let readInput file = 
    File.ReadAllLines file
    |> Array.map (fun l ->
        let s = l |> splitString [| ':'; ' ' |]
        ((int s.[0]), (int s.[1])))
    |> Array.sortBy fst
    |> Array.toList

let solve1 (input:Input) =
    input
    |> Seq.map (fun (t, r) ->
        match scanner r t = 0 with
        | true -> t * r
        | false -> 0)
    |> Seq.sum

let isSafe (input:Input) delay =
    input |> Seq.exists (fun (t, r) -> scanner r (t + delay) = 0) |> not

let solve2 (input:Input) =
    let scan = isSafe input
    let rec loop d =
        match scan d with
        | true -> d
        | false -> loop (d + 1)
    loop 0

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"
    let testInput = [(0, 3); (1, 2); (4, 4); (6, 4);]

    printfn "--- Tests ---\n"
    test solve1 [(testInput, 24)]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---\n"
    test solve2 [(testInput, 10)]

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
