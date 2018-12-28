open System
open System.IO
open System.Text.RegularExpressions

type Position = int64 * int64 * int64
type NanoBot = { pos : Position; r : int64 }

let dist ((a1, a2, a3) : Position) ((b1, b2, b3) : Position) =
    Math.Abs(a1 - b1) + Math.Abs(a2 - b2) + Math.Abs(a3 - b3)

let inRange (n1 : NanoBot) (n2 : NanoBot) = (dist n1.pos n2.pos) <= n1.r

let parseLine line =
    let regex = new Regex(@"^pos=\<(?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)\>,\sr=(?<r>\d+)$")
    let m = regex.Match(line);
    match m.Success with
    | false -> invalidArg "line" "Invalid format"
    | true ->
        let pos =
            (
                Int64.Parse(m.Groups.["x"].Value),
                Int64.Parse(m.Groups.["y"].Value),
                Int64.Parse(m.Groups.["z"].Value)
            )
        { pos = pos; r = Int64.Parse(m.Groups.["r"].Value) }

let readInput file = File.ReadAllLines file |> Array.map parseLine

let solve1 (input : NanoBot seq) =
    let max = input |> Seq.maxBy (fun n -> n.r)
    let maxInRange = inRange max
    input |> Seq.filter (fun n -> maxInRange n) |> Seq.length

[<EntryPoint>]
let main argv =
    let input = readInput "input.txt" |> List.ofSeq

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    Console.ReadLine() |> ignore
    0 // return an integer exit code
