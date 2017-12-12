open System
open System.IO
open Utils.Common
open System.Collections.Generic

let parseLine (line:string) =
    let d = line.Split([| "<->" |], StringSplitOptions.RemoveEmptyEntries)
    let n =
        d.[1]
        |> splitString [| ','; ' ' |]
        |> Array.map int
        |> Set.ofArray
    (int(d.[0]), n)

let readInput file =
    File.ReadAllLines file
    |> Array.map parseLine
    |> dict

let solve1 (input:IDictionary<int, int Set>) =
    let rec bfs (node:int) visited todo =
        let v = Set.add node visited 
        let t' = Set.union todo input.[node]
        match Set.toList t' with
        | [] -> v
        | h::t -> bfs h v t

    let group = bfs 0 (Set.ofList []) (Set.ofList [])
    group.Count

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    let testInput =
        [
            "0 <-> 2";
            "1 <-> 1";
            "2 <-> 0, 3, 4";
            "3 <-> 2, 4";
            "4 <-> 2, 3, 6";
            "5 <-> 6";
            "6 <-> 4, 5";
        ]
        |> List.map parseLine
        |> dict

    printfn "--- Tests ---\n"
    
    test solve1 [(testInput, 6)]

    Console.ReadLine() |> ignore
    0 // return an integer exit code
