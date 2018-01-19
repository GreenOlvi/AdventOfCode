open System
open System.IO
open Utils.Common

type Direction = North | South | East | West
type NodeState = Clean | Weakened | Infected | Flagged

type MapType = Map<int*int, NodeState>

type State = {
    Position : int*int;
    Direction : Direction
    Map : MapType;
    Infected : int;
}

let rotateLeft dir =
    match dir with
    | North -> West
    | South -> East
    | East -> North
    | West -> South

let rotateRight dir =
    match dir with
    | North -> East
    | South -> West
    | East -> South
    | West -> North

let rotateBack dir =
    match dir with
    | North -> South
    | South -> North
    | East -> West
    | West -> East

let rotateOnState nodeState dir =
    match nodeState with
    | Clean -> rotateLeft dir
    | Weakened -> dir
    | Infected -> rotateRight dir
    | Flagged -> rotateBack dir

let moveForward direction (x, y) =
    match direction with
    | North -> (x, y + 1)
    | South -> (x, y - 1)
    | East -> (x + 1, y)
    | West -> (x - 1, y)

let nextState2 state =
    match state with
    | Clean -> Weakened
    | Weakened -> Infected
    | Infected -> Flagged
    | Flagged -> Clean

let getState (map:MapType) position =
    match map.ContainsKey position with
    | true -> map.[position]
    | false -> Clean

let solve action input (count:int64) =
    let rec loop state i =
        if (i % 100000L = 0L) then
            printfn "%A%%" (i / 100000L)

        match i >= count with
        | true -> state
        | false ->
            let newState = action state
            loop newState (i + 1L)
    loop { Position = (0, 0); Direction = North; Map = input; Infected = 0 } 0L

let solve1 (input, count) =
    let action state =
        let nodeState = getState state.Map state.Position
        let newDir = rotateOnState nodeState state.Direction
        match nodeState with
        | Infected ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Remove state.Position;
                Infected = state.Infected;
            }
        | Clean ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Add (state.Position, Infected);
                Infected = state.Infected + 1;
            }
        | _ -> invalidOp "Should not be in this state"

    let s = solve action input count
    s.Infected

let solve2 (input, count:int64) =
    let action state =
        let nodeState = getState state.Map state.Position
        let newDir = rotateOnState nodeState state.Direction
        let newState = nextState2 nodeState
        match nodeState with
        | Clean ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Add (state.Position, newState);
                Infected = state.Infected;
            }
        | Weakened ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Add (state.Position, newState);
                Infected = state.Infected + 1;
            }
        | Infected ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Add (state.Position, newState);
                Infected = state.Infected;
            }
        | Flagged ->
            {
                Position = moveForward newDir state.Position;
                Direction = newDir;
                Map = state.Map.Remove state.Position;
                Infected = state.Infected;
            }

    let s = solve action input count
    s.Infected

let parseMap (lines:string[]):MapType =
    let width = String.length lines.[0]
    let height = lines.Length
    let ox = (width / 2)
    let oy = (height / 2)

    let parseLine y =
        [0..width - 1]
            |> Seq.filter (fun x -> lines.[y].[x] = '#')
            |> Seq.map (fun x -> ((x - ox, oy - y), Infected))

    [0..height - 1]
        |> Seq.collect (fun y -> parseLine y)
        |> Map.ofSeq

let readInput file =
    File.ReadAllLines file |> parseMap

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    let testInput =
        [|
            "..#";
            "#..";
            "...";
        |]

    printfn "--- Tests ---\n"
    test parseMap [(testInput, Map.ofList [((1, 1), Infected); ((-1, 0), Infected)])]

    test solve1 [
        (((parseMap testInput), 7L), 5);
        (((parseMap testInput), 70L), 41);
        (((parseMap testInput), 10000L), 5587);
    ]

    let result1 = timeIt (fun () -> solve1 (input, 10000L))
    printfn "Result 1: %A" result1

    test solve2 [
        (((parseMap testInput), 100L), 26);
        (((parseMap testInput), 10000000L), 2511944);
    ]

    let result2 = timeIt (fun () -> solve2 (input, 10000000L))
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0
