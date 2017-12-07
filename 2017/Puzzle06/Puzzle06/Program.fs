open System
open System.IO
open System.Collections.Generic
open Utils
open Utils.Common

let splitNumbers (input:string) =
    input
    |> splitString [| ' '; '\t'; '\r'; '\n' |]
    |> Array.map int

let readInput file = File.ReadAllText(file) |> splitNumbers

type State = int[]

let maxIndex items =
    items
    |> Array.mapi (fun i x -> (i, x))
    |> Array.maxBy snd
    |> fst

let succ (banks:State):State =
    let n = banks.Length
    let idx = maxIndex banks
    let maxVal = banks.[idx]
    let j = maxVal / n
    let k = maxVal % n
    let l = (idx + k) % n
    banks |> Array.mapi (fun i item ->
        match i with
        | index when index = idx -> j
        | _ when k = 0 -> item + j
        | index when (l < idx && (index > idx || index <= l))
            || (l > idx && index > idx && index <= l) ->
                item + j + 1
        | _ -> item + j)

let getKey (b:State) = b |> Array.fold (fun s i -> s + (string i) + "|") "|"
    
let solve input =
    let dict = HashSet<string>()
    let rec loop (b:State) =
        let key = getKey b
        //printfn "%A" key
        match dict.Contains(key) with
        | true -> (dict.Count, b)
        | false ->
            dict.Add(key) |> ignore
            loop (succ b)
    loop input

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    printfn "--- Tests ---"

    test maxIndex [
        (splitNumbers "1 2 3 4", 3);
        (splitNumbers "1 1 1 1", 0);
        (splitNumbers "0 2 7 0", 2);
        (splitNumbers "2 4 1 2", 1);
        (splitNumbers "3 1 2 3", 0);
        (splitNumbers "0 2 3 4", 3);
        (splitNumbers "1 3 4 1", 2);
    ]

    test succ [
        (splitNumbers "0 2 7 0", splitNumbers "2 4 1 2");
        (splitNumbers "2 4 1 2", splitNumbers "3 1 2 3");
        (splitNumbers "3 1 2 3", splitNumbers "0 2 3 4");
        (splitNumbers "0 2 3 4", splitNumbers "1 3 4 1");
        (splitNumbers "1 3 4 1", splitNumbers "2 4 1 2");
        (splitNumbers "0 5 0 0", splitNumbers "1 1 2 1");
    ]

    test getKey [
        (splitNumbers "0 2 7 0", "|0|2|7|0|");
    ]

    Common.test solve [(splitNumbers "0 2 7 0"), (5, splitNumbers "2 4 1 2")]

    let result1 = Common.timeIt (fun () -> solve input)
    wrapColor ConsoleColor.Yellow (fun () -> printfn "Result 1: %A" (fst result1))

    printfn "--- Tests ---"
    test solve [(splitNumbers "2 4 1 2"), (4, splitNumbers "2 4 1 2")]

    let result2 = timeIt (fun () -> solve (snd result1))
    printfn "Result 2: %A" (fst result2)

    Console.ReadLine() |> ignore
    0 // return an integer exit code