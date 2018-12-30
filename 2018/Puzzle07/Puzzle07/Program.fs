open System
open System.IO
open System.Text.RegularExpressions
open Utils

type Node = string
type Edge = string * string

let parseLine line : Edge =
    let lineRegex = new Regex(@"^Step (?<L>\w) must be finished before step (?<R>\w) can begin.$", RegexOptions.Compiled);
    let m = lineRegex.Match(line)
    match m.Success with
    | false -> invalidArg "line" "Line does not have correct format"
    | true -> (m.Groups.["L"].Value, m.Groups.["R"].Value)

let readInput file =
    File.ReadAllLines file |> Seq.map parseLine |> List.ofSeq

let availableTasks (edges : Edge seq) =
    let left = edges |> Seq.map fst |> set
    let right = edges |> Seq.map snd |> set
    Set.difference left right |> Seq.sort

let solve1 (input : Edge list) =
    let rec loop (result : string) (edges : Edge list) : string =
        let node = availableTasks edges |> Seq.item 0
        let newEdges = edges |> List.filter (fun (l, _) -> l <> node)
        match newEdges with
        | [] -> (result + node + (edges |> Seq.item 0 |> snd))
        | _ -> loop (result + node) newEdges
    loop String.Empty input

type IdleWorker = int
type BusyWorker = { id : int; task : Node; busyFor : int }

let time (name : Node) = (name |> Seq.item 0 |> int) - 0x40
let time60 name = time name + 60

type Task =
    | Free of Node
    | Blocked of (Node * Node list)

let sortTasks (tasks : Task seq) = tasks |> Seq.sortBy (fun t -> match t with Free n -> n | Blocked (n, _) -> n)

let parseTaskList (input : Edge list) =
    let blocked = 
        input
        |> List.groupBy snd
        |> List.map (fun (t, e) ->
            let blockers = e |> List.map fst
            match blockers with
            | [] -> Free t
            | _ -> Blocked (t, blockers))
    let free = availableTasks input |> Seq.map (fun name -> Free name)
    blocked |> Seq.append free |> sortTasks |> List.ofSeq


let freeTasks (nodes : Node list) (tasks : Task list) =
    tasks
    |> List.map (fun t ->
        match t with
        | Free name -> Free name
        | Blocked (name, blockers) ->
            let newBlockers = blockers |> List.except nodes
            match newBlockers with
            | [] -> Free name
            | _ -> Blocked (name, newBlockers))

type WorkState = {
    time : int;
    busyWorkers : BusyWorker list;
    idleWorkers : IdleWorker list;
    tasks : Task list;
}

type ProcessedResult =
    | Busy of BusyWorker
    | Idle of IdleWorker * Node

let workTick timeFun (state : WorkState) =
    let processed =
        state.busyWorkers
        |> List.map (fun w -> 
            let t = w.busyFor - 1
            match t with
            | 0 -> Idle (w.id, w.task)
            | t -> Busy { w with busyFor = t})

    let busy = processed |> List.choose (fun w -> match w with Busy b -> Some b | _ -> None)
    let newIdle = processed |> List.choose (fun w -> match w with Idle (id, _) -> Some id | _ -> None)
    let doneTasks = processed |> List.choose (fun w -> match w with Idle (_, t) -> Some t | _ -> None)

    let idle = state.idleWorkers |> List.append newIdle |> List.sort
    let tasks = state.tasks |> freeTasks doneTasks

    match idle with
    | [] ->
        {
            time = state.time + 1;
            busyWorkers = busy;
            idleWorkers = idle;
            tasks = tasks;
        }
    | _ -> 
        let processingTasks = busy |> Seq.map (fun w -> w.task)

        let freeTasks =
            tasks
            |> Seq.choose (fun t -> match t with Free name -> Some name | _ -> None)
            |> Seq.except processingTasks
            |> Seq.sort
            |> List.ofSeq

        match freeTasks with
        | [] ->
            {
                time = state.time + 1;
                busyWorkers = busy;
                idleWorkers = idle;
                tasks = tasks;
            }
        | _ ->
            let assigned = Seq.zip idle freeTasks
            let newBusy =
                assigned
                |> Seq.map (fun (w, t) -> { id = w; task = t; busyFor = (timeFun t) })
                |> List.ofSeq
            let removedTasks = assigned |> Seq.map (fun (_, t) -> Free t)|> List.ofSeq
            {
                time = state.time + 1;
                busyWorkers = busy |> List.append newBusy;
                idleWorkers = idle |> List.except (newBusy |> List.map (fun w -> w.id));
                tasks = tasks |> List.except removedTasks;
            }

let initialState workers tasks =
    {
        time = 0;
        busyWorkers = [];
        idleWorkers = (seq {1 .. workers} |> List.ofSeq);
        tasks = tasks;
    }

let solve2 timeFunction workerCount (input : Edge list) =
    let tick = workTick timeFunction
    let rec loop state = 
        let r = tick state
        match (r.tasks |> List.isEmpty && r.busyWorkers |> List.isEmpty) with
        | true -> r
        | false -> loop r
    let tasks = parseTaskList input
    let result = loop (initialState workerCount tasks)
    result.time - 1

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

    let testTasks = testInput |> parseTaskList
    
    Common.test (workTick time) [
        ((initialState 2 testTasks),
            { time = 1; busyWorkers = [{ id = 1; task = "C"; busyFor = 3 }]; idleWorkers = [2]; tasks = testTasks |> List.except [Free "C"] });

        ({ time = 1; busyWorkers = [{ id = 1; task = "C"; busyFor = 3 }]; idleWorkers = [2]; tasks = testTasks |> List.except [Free "C"] },
            { time = 2; busyWorkers = [{ id = 1; task = "C"; busyFor = 2}]; idleWorkers = [2]; tasks = testTasks |> List.except [Free "C"] });

        (
            {
                time = 3;
                busyWorkers = [{ id = 1; task = "C"; busyFor = 1 }];
                idleWorkers = [2];
                tasks = testTasks |> List.except [Free "C"];
            },
            {
                time = 4;
                busyWorkers = [{ id = 1; task = "A"; busyFor = 1 }; { id = 2; task = "F"; busyFor = 6 }];
                idleWorkers = [];
                tasks = [Blocked ("B", ["A"]); Blocked ("D", ["A"]); Blocked ("E", ["B"; "D"; "F"])];
            }
        )
    ]

    Common.test solve1 [(testInput, "CABDFE")]
    Common.test (fun i -> solve2 time 2 i) [(testInput, 15)]

    let result1 = Common.timeAction (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    let result2 = Common.timeAction (fun () -> solve2 time60 5 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
