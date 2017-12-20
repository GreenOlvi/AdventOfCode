open System
open System.IO
open Utils.Common

type MapPiece =
    | Space
    | Path
    | Turn
    | Letter of char

type Map = MapPiece[][]

type Direction = N | S | E | W

type State = { Position:int * int; Direction:Direction; Letters:char list; Steps:int }

let toMapPiece c =
    match c with
    | ' ' -> Space
    | '|'
    | '-' -> Path
    | '+' -> Turn
    | _ -> Letter c

let parseLine (line:string) = line |> Seq.map toMapPiece |> Array.ofSeq
let parseInput lines:Map = lines |> Array.map parseLine
let readInput file = File.ReadAllLines file |> parseInput

let findStart (input:Map) = input.[0] |> Array.findIndex (fun i -> i = Path) |> fun x -> (x, 0)

let nextPos ((x, y):int * int) (dir:Direction) =
    match dir with
    | N -> (x, y - 1)
    | S -> (x, y + 1)
    | E -> (x + 1, y)
    | W -> (x - 1, y)

let oppositeDir = [(N, S); (S, N); (E, W); (W, E)] |> dict

let move (map:Map) (s:State):State option =
    let (x, y) = s.Position
    let currentPiece = map.[y].[x]
    let turn pos currDir =
        let o = oppositeDir.[currDir]
        [N; S; E; W]
        |> Seq.filter (fun d -> d <> o)
        |> Seq.map (fun d -> (nextPos pos d, d))
        |> Seq.find (fun (p, _) ->
            match p with
            | -1, _ -> false
            | _, -1 -> false
            | _, y when y > map.Length - 1 -> false
            | x, y when x > map.[y].Length - 1 -> false
            | x, y -> map.[y].[x] <> Space)

    match currentPiece with
    | Path -> Some {
            s with
                Position = nextPos s.Position s.Direction;
                Steps = s.Steps + 1;
            }
    | Letter c -> Some {
            s with
                Position = nextPos s.Position s.Direction;
                Letters = c::s.Letters;
                Steps = s.Steps + 1;
            }
    | Turn ->
        let (newPos, newDir) = turn s.Position s.Direction
        Some {
            s with
                Position = newPos;
                Direction = newDir;
                Steps = s.Steps + 1;
            }
    | Space -> None

let solve input =
    let start = findStart input
    let rec loop s =
        let next = move input s
        match next with
        | None ->
            let letters = s.Letters |> List.rev |> String.Concat
            (letters, s.Steps)
        | Some state -> loop state
        
    loop { Position = start; Direction = S; Letters = []; Steps = 0 }

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    let testInput =
        [|
            "     |          ";
            "     |  +--+    ";
            "     A  |  C    ";
            " F---|----E|--+ ";
            "     |  |  |  D ";
            "     +B-+  +--+ ";
        |]
        |> parseInput
    
    printfn "--- Tests ---\n"
    test solve [(testInput, ("ABCDEF", 38))]

    let (result1, result2) = timeIt (fun () -> solve input)
    printfn "Result 1: %A" result1
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
