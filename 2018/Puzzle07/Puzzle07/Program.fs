open System
open System.IO
open System.Text.RegularExpressions
open Utils

let parseLine line =
    let lineRegex = new Regex(@"^Step (?<L>\w) must be finished before step (?<R>\w) can begin.$", RegexOptions.Compiled);
    let m = lineRegex.Match(line)
    match m.Success with
    | false -> invalidArg "line" "Line does not have correct format"
    | true -> (m.Groups.["L"].Value, m.Groups.["R"].Value)

let readInput file =
    File.ReadAllLines file |> Seq.map parseLine |> List.ofSeq

let solve1 input =
    let rec loop (result:string) (edges:(string * string) list) : string =
        let left = edges |> Seq.map fst |> set
        let right = edges |> Seq.map snd |> set
        let node = Set.difference left right |> Seq.sort |> Seq.item 0
        let newEdges = edges |> List.filter (fun (l, _) -> l <> node)
        match newEdges with
        | [] -> (result + node + (right |> Seq.item 0))
        | _ -> loop (result + node) newEdges

    loop String.Empty input

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    let testInput =
        [
            "Step C must be finished before step A can begin.";
            "Step C must be finished before step F can begin.";
            "Step A must be finished before step B can begin.";
            "Step A must be finished before step D can begin.";
            "Step B must be finished before step E can begin.";
            "Step D must be finished before step E can begin.";
            "Step F must be finished before step E can begin.";
        ]
        |> List.map parseLine
    
    Common.test solve1 [(testInput, "CABDFE")]

    let result1 = Common.timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    Console.ReadLine() |> ignore
    0 // return an integer exit code
