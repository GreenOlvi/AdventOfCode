open System.IO
open System

let solve1Row row = List.max row - List.min row

let rec checkList ns =
    match ns with
        | a::t ->
            let r = List.tryFind (fun b -> (a % b = 0) || (b % a = 0)) t
            match r with
                | Some b -> Some (a, b)
                | None -> checkList t
        | _ -> None

let solve2Row row =
    let pair = checkList row
    match pair with
        | Some (a, b) -> if a > b then a / b else b / a
        | None -> invalidArg "row" "Row should have divisable items"

let solve rowSolver input = input |> List.map rowSolver |> List.sum
let solve1 input = solve solve1Row input
let solve2 input = solve solve2Row input

let test1 f expected =
    let result = f()
    printfn "result: %A, should be: %A - %A" result expected (result = expected)

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

let splitLines (input:string) =
    input.Split([|'\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

let splitRow (line:string) =
    line.Split([|'\t'; ' '|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList
        |> List.map int

let splitTable = splitLines >> List.map splitRow
let readInput file = File.ReadAllText(file) |> splitTable

[<EntryPoint>]
let main argv = 
    let input = readInput "input.txt"

    printfn "--- Tests ---"
    test solve1Row [(splitRow "5 1 9 5", 8); (splitRow "7 5 3", 4); (splitRow "2 4 6 8", 6)]
    test solve1 [(splitTable "5 1 9 5\n7 5 3\n2 4 6 8", 18)]

    let result1 = solve1 input
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---"
    test1 (fun () -> checkList [9; 4; 7; 3]) (Some (9, 3))
    test1 (fun () -> checkList [3; 4; 7; 9]) (Some (3, 9))
    test1 (fun () -> checkList [9; 5; 2; 4]) (None)

    test solve2Row [(splitRow "5 9 2 8", 4); (splitRow "9 4 7 3", 3); (splitRow "3 8 6 5", 2)]
    test solve2 [(splitTable "5 9 2 8\n9 4 7 3\n3 8 6 5", 9)]

    let result2 = solve2 input
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
