open System
open System.IO
open Utils.Common
open System.Collections.Generic

type Node = int

let parseLine (line:string) =
    let d = line.Split([| "<->" |], StringSplitOptions.RemoveEmptyEntries)
    let n =
        d.[1]
        |> splitString [| ','; ' ' |]
        |> Array.map int
        |> Array.toList
    (int(d.[0]), n)

let readInput file =
    File.ReadAllLines file
    |> Array.map parseLine
    |> dict

let walkBfs (neighbours:IDictionary<Node, Node list>) node =
    let rec bfs (visited:Node Set) (todo:Node list):Node Set =
        match todo with
        | [] -> visited
        | n::rest ->
            let visited' = visited.Add n
            let todo' = rest |> List.append (neighbours.[n] |> List.filter (fun i -> visited'.Contains i |> not))
            bfs visited' todo'
    bfs (Set.ofList []) [node]

let solve1 input = walkBfs input 0 |> Set.count

let solve2 (input:IDictionary<Node, Node list>) =
    let rec findGroup (visited:Node Set) (todo:Node list) i =
        match todo with
        | [] -> i
        | n::rest ->
            let visited' = visited + walkBfs input n
            let todo' = rest |> List.filter (fun t -> visited'.Contains t |> not)
            findGroup visited' todo' (i + 1)
    let nodes = input.Keys |> Seq.toList
    findGroup Set.empty nodes 0

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

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    printfn "--- Tests ---\n"
    test solve2 [(testInput, 2)]

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
