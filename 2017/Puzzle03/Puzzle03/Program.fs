open System

let test f data =
    let testResults =
        data
        |> List.map (fun (input, expected) -> 
            let result = input |> f
            printfn "input: %A, result: %A, should be: %A" input result expected
            result = expected
        )
    let successful = (testResults |> List.filter (fun i -> i) |> List.length)
    printfn "Success %A/%A\n" successful data.Length

let toCartesian (id:int):(int*int) =
    match id with
        | 0 -> invalidArg "id" "Should be greater than 1"
        | 1 -> (0, 0)
        | _ ->
            let layer = int(Math.Floor(Math.Sqrt(double (id - 1)) - 1.0) / 2.0) + 1
            let n = 8 * layer
            let side = 2 * layer
            let start = 4 * layer * layer - 4 * layer + 1
            let dist = (double ((id - 1) - start) / (double n)) * 4.0
            let quarter = int(Math.Floor(dist))
            let offset = int(Math.Ceiling((dist - double(quarter)) * double(side) - double(side / 2 - 1)))
            match quarter with
                | 0 -> (layer, offset)
                | 1 -> (-offset, layer)
                | 2 -> (-layer, -offset)
                | 3 -> (offset, -layer)
                | _ -> invalidArg "quarter" "Expected value in 0..3"

let solve1 (input:int) =
    let (x, y) = toCartesian input
    abs(x) + abs(y)

let solve2 input = 0

[<EntryPoint>]
let main argv = 
    let input = 325489

    printfn "--- Tests ---"
    test toCartesian [(1, (0, 0)); (2, (1, 0)); (3, (1, 1)); (5, (-1, 1)); (10, (2, -1)); (23, (0, -2))]
    test solve1 [(1, 0); (12, 3); (23, 2); (1024, 31)]

    let result1 = solve1 input
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---"
    test solve2 []

    let result2 = solve2 input
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
