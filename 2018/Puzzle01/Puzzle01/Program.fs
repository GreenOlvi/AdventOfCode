open System
open System.IO

let solve1 input = input |> Seq.sum

let solve2 input =
    let arr = Array.ofSeq input
    let rec loop f i visited =
        let e = f + arr.[i]
        match Set.contains e visited with
        | true -> e
        | false ->
            let j = (i + 1) % Array.length arr
            loop e j (visited.Add(e))
    loop 0 0 Set.empty


let readInput file =
    File.ReadAllLines(file)
    |> Seq.map Int32.Parse 

[<EntryPoint>]
let main argv =
    let input = readInput "input.txt"

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    let result2 = solve2 input
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
