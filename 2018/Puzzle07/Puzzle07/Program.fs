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

let availableTasks edges =
    let left = edges |> Seq.map fst |> set
    let right = edges |> Seq.map snd |> set
    Set.difference left right |> Seq.sort

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

type BusyWorker = {
    id : int;
    currentTask : string option;
    busyFor : int;
}

type Worker =
    | Idle of int
    | Busy of BusyWorker

let initWorkers count = seq {1..count} |> Seq.map (fun i -> Idle i) |> List.ofSeq

let decreaseTime busy =
    let ws =
        busy
        |> Seq.map (fun w ->
        let newTime = w.busyFor - 1
        match newTime with
        | 0 -> Idle w.id
        | _ -> Busy { w with busyFor = newTime })
        |> List.ofSeq
    (ws |> List.choose (fun w -> match w with Busy _ -> Some w | _ -> None),
        ws |> List.choose (fun w -> match w with Idle _ -> Some w | _ -> None))


let time name = (name |> Seq.item 0 |> int) - 0x40
let time60 name = time name + 60

let solve2 timeFunction workerCount input =
    let rec loop t edges busy idle =
        printfn "Free workers: %A" idle

        let freeTasks = availableTasks input
        printfn "Free tasks: %A" freeTasks

        0

    loop 0 input [] (initWorkers workerCount)

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
    
    //Common.test solve1 [(testInput, "CABDFE")]
    Common.test (fun i -> solve2 time 2 i) [(testInput, 15)]

    let result1 = Common.timeResult 1 { return solve1 input }

    Console.ReadLine() |> ignore
    0 // return an integer exit code
