open System
open System.IO
open Utils

type Point = string * int * int
type Rect = int * int * int * int

let encapsulate points =
    let minX = points |> Seq.map (fun (_, x, _) -> x) |> Seq.min
    let maxX = points |> Seq.map (fun (_, x, _) -> x) |> Seq.max
    let minY = points |> Seq.map (fun (_, _, y) -> y) |> Seq.min
    let maxY = points |> Seq.map (fun (_, _, y) -> y) |> Seq.max
    (minX, minY, maxX, maxY)

let rectBorder (x0, y0, x1, y1) =
    Seq.concat [
        seq {x0..x1} |> Seq.collect (fun x -> [(x, y0); (x, y1)]);
        seq {y0..y1} |> Seq.collect (fun y -> [(x0, y); (x1, y)]);
    ]

let enumerateRect ((x0, y0, x1, y1) : Rect) =
    Common.cartesian (seq {x0..x1}) (seq {y0..y1})

let dist ((x1, y1):int * int) (x2, y2) =
    Math.Abs(x1 - x2) + Math.Abs(y1 - y2)

let distName ((s, x1, y1):Point) (x2, y2) = (s, dist (x1, y1) (x2, y2))

let findClosest points p =
    let distances =
        points
        |> Seq.map distName
        |> List.ofSeq
    let d =
        distances
        |> Seq.map (fun f -> f p)
        |> Seq.sortBy snd
        |> Seq.take 2
        |> Array.ofSeq
    match (snd (d.[0])) = (snd (d.[1])) with
    | true -> None
    | false -> Some (fst d.[0])

let parseInputLine line =
    let ints = line |> Common.splitString [' '; ','] |> Array.map Int32.Parse
    (ints.[0], ints.[1])

let readInput file : Point seq =
    File.ReadAllLines file
    |> Seq.map parseInputLine
    |> Seq.mapi (fun i (x, y) -> ((i + 1).ToString(), x, y))

let solve1 input =
    let closest = findClosest input
    let rect = encapsulate input

    let infinite =
        rect
        |> rectBorder
        |> Seq.choose closest
        |> Seq.distinct
        |> set

    let countAreas (areas:Map<string, int>) s =
        match areas.ContainsKey s with
        | true -> areas.Add(s, (areas.[s] + 1))
        | false -> areas.Add(s, 1)

    let areas =
        rect
        |> enumerateRect
        |> Seq.choose closest
        |> Seq.filter (fun s -> not (infinite.Contains s))
        |> Seq.fold countAreas Map.empty

    let largest = areas |> Seq.maxBy (fun kv -> kv.Value)
    largest.Value

let solve2 input maxDist =
    let distances =
        input |> Seq.map (fun (_, x, y) -> dist (x, y)) |> List.ofSeq

    input
    |> encapsulate
    |> enumerateRect
    |> Seq.map (fun p -> distances |> Seq.map (fun f -> f p) |> Seq.sum)
    |> Seq.filter (fun d -> d < maxDist)
    |> Seq.length

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"
    let testInput = [("A", 1, 1); ("B", 1, 6); ("C", 8, 3); ("D", 3, 4); ("E", 5, 5); ("F", 8, 9)]

    Common.test solve1 [testInput, 17]
    Common.test (fun (i, d) -> solve2 i d) [(testInput, 32), 16]

    let result1 = Common.timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    let result2 = Common.timeIt (fun () -> solve2 input 10000)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
