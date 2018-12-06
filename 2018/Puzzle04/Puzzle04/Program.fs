open System
open System.IO
open Utils

type Action =
    | Guard of int
    | FallAsleep
    | WakeUp

type Range = int * int

type Sleep = {id : int; sleep : Range }

type GuardState =
    | Invalid
    | Awoke
    | Asleep of int

type State = {
    currentGuard : int option;
    guardState : GuardState;
    sleepRecords : Sleep list;
}

let analyzeRecords list =
    let processRecord (state:State) ((dt:DateTime), action) =
        match action with
        | Guard id -> { state with currentGuard = Some id }
        | FallAsleep -> { state with guardState = Asleep dt.Minute }
        | WakeUp ->
            let gid = match state.currentGuard with Some id -> id | None -> invalidOp "No guard assigned"
            let start = match state.guardState with Asleep t -> t | _ -> invalidOp "Guard was not sleeping"
            let sleeps = List.append state.sleepRecords [{ id = gid; sleep = (start, dt.Minute - 1) }]
            { currentGuard = Some gid; guardState = Awoke; sleepRecords = sleeps }

    let result =
        list
        |> Seq.sortBy fst
        |> Seq.fold processRecord { currentGuard = None; guardState = Invalid; sleepRecords = []}
    result.sleepRecords

let expandRange (r:Range) = seq {fst r..snd r}

let parseInput (line:string) =
    let dt = line.Substring(1, 16) |> DateTime.Parse
    let rest = line.Substring(19)
    let action =
        match rest with
        | Common.FirstRegexGroup "Guard #(\d+) begins shift" id -> Guard (Int32.Parse id)
        | Common.StartsWith "wakes up" -> WakeUp
        | Common.StartsWith "falls asleep" -> FallAsleep
        | _ -> invalidArg "line" line
    (dt, action)

let readInput file =
    File.ReadAllLines file
    |> Seq.map parseInput

let getMinutesByGuard input =
    input
    |> analyzeRecords
    |> Seq.groupBy (fun r -> r.id)
    |> Seq.map (fun (g, s) -> (g, s |> Seq.collect (fun r -> expandRange r.sleep) |> List.ofSeq))

let findMostFrequent input =
    input
        |> List.groupBy id
        |> List.map (fun (m, c) -> (m, c.Length))
        |> List.sortByDescending (fun (_, c) -> c)
        |> List.item 0

let solve1 input =
    let guard =
        getMinutesByGuard input
        |> Seq.sortByDescending (fun (_, s) -> s.Length)
        |> Seq.item 0
    let gid = fst guard
    let minutes = snd guard
    let bestMinute = minutes |> findMostFrequent |> fst
    gid * bestMinute

let solve2 input =
    let best =
        getMinutesByGuard input
        |> Seq.map (fun (g, minutes) -> (g, findMostFrequent minutes) )
        |> Seq.sortByDescending (fun (_, (_, c)) -> c)
        |> Seq.item 0
    let (gid, (m, _)) = best
    gid * m

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    let result2 = solve2 input
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
