open System
open System.IO

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
    input.Split([|'\n'; '\r'; ' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map int
        |> Array.toList

let readInput file = File.ReadAllText(file) |> splitLines

type State = int * int * int list

let replaceAt index sub =
    List.mapi (fun idx item ->
        match idx with
        | i when i = index -> sub item
        | _ -> item)

let succ jumpSub ((id, index, jumps):State):option<State> =
    let jump = jumps.Item index
    match index + jump with
    | i when i >= 0 && i < jumps.Length ->
        let newJumps = jumps |> replaceAt index jumpSub
        Some (id + 1, index + jump, newJumps)
    | _ -> None

let succ1 = succ (fun j -> j + 1)
let succ2 = succ (fun j ->
    match j with
    | i when i >= 3 -> j - 1
    | _ -> j + 1)

let solve succFun input =
    let rec loop (state:State):int =
        let r = succFun state
        match r with
        | None ->
            let id, _, _ = state
            id + 1
        | Some s -> loop s
    loop (0, 0, input)

let solve1 = solve succ1
let solve2 = solve succ2

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"

    printfn "--- Tests ---"
    test succ1 [
        ((0, 0, splitLines "0 3 0 1 -3"), Some (1, 0, splitLines "1 3 0 1 -3"));
        ((1, 0, splitLines "1 3 0 1 -3"), Some (2, 1, splitLines "2 3 0 1 -3"));
        ((2, 1, splitLines "2 3 0 1 -3"), Some (3, 4, splitLines "2 4 0 1 -3"));
        ((3, 4, splitLines "2 4 0 1 -3"), Some (4, 1, splitLines "2 4 0 1 -2"));
        ((4, 1, splitLines "2 4 0 1 -2"), None);
    ]

    test solve1 [(splitLines "0 3 0 1 -3"), 5]

    let stopwatch1 = System.Diagnostics.Stopwatch.StartNew()
    let result1 = solve1 input
    stopwatch1.Stop()
    printfn "Result 1: %A" result1
    printfn "%f seconds\n" stopwatch1.Elapsed.TotalSeconds

    printfn "--- Tests ---"
    test solve2 [(splitLines "0 3 0 1 -3"), 10]

    let stopwatch2 = System.Diagnostics.Stopwatch.StartNew()
    let result2 = solve2 input
    stopwatch2.Stop()
    printfn "Result 2: %A" result2
    printfn "%f seconds\n" stopwatch2.Elapsed.TotalSeconds

    Console.ReadLine() |> ignore
    0 // return an integer exit code
