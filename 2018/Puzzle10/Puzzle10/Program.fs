open System
open System.IO
open System.Text.RegularExpressions

type Vector = int * int
type Point = { Position : Vector; Velocity : Vector }

let addVector (left : Vector) (right : Vector) =
    ((fst left) + (fst right), (snd left) + (snd right))

let movePoint p = { Position = addVector p.Position p.Velocity; Velocity = p.Velocity }

let area (points : Point list) =
    let xmin = points |> List.map (fun p -> (fst p.Position)) |> List.min
    let xmax = points |> List.map (fun p -> (fst p.Position)) |> List.max
    let ymin = points |> List.map (fun p -> (snd p.Position)) |> List.min
    let ymax = points |> List.map (fun p -> (snd p.Position)) |> List.max
    (xmax - xmin, ymax - ymin)

let parseLine line =
    let r = new Regex(@"position=\<\s*(?<px>\-?\d+),\s*(?<py>\-?\d+)\> velocity=\<\s*(?<vx>\-?\d+),\s*(?<vy>\-?\d+)\>", RegexOptions.Compiled)
    let m = r.Match(line)
    match m.Success with
    | false -> invalidArg "line" "Invalid format"
    | true ->
        let pos = (m.Groups.["px"].Value |> int, m.Groups.["py"].Value |> int)
        let vel = (m.Groups.["vx"].Value |> int, m.Groups.["vy"].Value |> int)
        { Position = pos; Velocity = vel }

let readInput file = File.ReadAllLines file |> Seq.map parseLine |> List.ofSeq

let drawPoints (points : Point list) : unit =
    let xmin = points |> List.map (fun p -> (fst p.Position)) |> List.min
    let ymin = points |> List.map (fun p -> (snd p.Position)) |> List.min

    points |> List.iter (fun p ->
        Console.SetCursorPosition(fst p.Position - xmin, snd p.Position - ymin + 1)
        printf "#")

let solve points =
    let cmpArea curr last =
        (fst curr < fst last) && (snd curr < snd last)

    let rec movePoints lastArea points iter =
        let newPoints = points |> List.map movePoint
        let a = area newPoints
        match (cmpArea a lastArea |> not) with
        | false -> movePoints a newPoints (iter + 1)
        | true -> (iter - 1, points)

    movePoints (area points) points 1

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    let result = solve input
    printfn "Result 1:"
    drawPoints (snd result)

    Console.SetCursorPosition(0, 12)
    printfn "Result 2: %A" (fst result)

    Console.ReadLine() |> ignore
    0 // return an integer exit code
