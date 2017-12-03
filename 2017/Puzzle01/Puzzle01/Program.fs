module Program

open System
open System.IO
open System.Text.RegularExpressions

let zeroChar = int '0'
let charToInt c =
    let i = int c - zeroChar
    match i with
        | d when d >= 0 && d <= 9 -> i
        | _ -> invalidArg "c" "Character should be between 0 to 9"

let shift i n arr =
    let newIndex = (i + n) % Array.length arr
    arr.[newIndex]

let solve step input =
    let arr = input |> Seq.map charToInt |> Array.ofSeq
    arr
        |> Seq.mapi (fun index item -> (item, shift step index arr))
        |> Seq.filter (fun (a, b) -> a = b)
        |> Seq.map (fun (a, b) -> a)
        |> Array.ofSeq
        |> Seq.sum

let solve1 input = input |> solve 1

let solve2 input = input |> solve (String.length input / 2)

let test f data =
    printfn "--- Tests ---"
    let testResults =
        data
        |> List.map (fun (input, expected) -> 
            let result = input |> f
            printfn "input: %A, result: %A, should be: %A" input result expected
            result = expected
        )
    let successful = (testResults |> List.filter (fun i -> i) |> List.length)
    printfn "Success %A/%A\n" successful data.Length

let readInput file =
    let input = File.ReadAllText(file)
    Regex.Replace(input, @"\n|\r", "")

[<EntryPoint>]
let main argv = 
    let input = readInput "input.txt"

    test solve1 [("1122", 3); ("1111", 4); ("1234", 0); ("91212129", 9)]
    let result1 = solve1 input
    printfn "Solve 1 result: %A\n" result1

    test solve2 [("1212", 6); ("1221", 0); ("123425", 4); ("123123", 12); ("12131415", 4)]
    let result2 = solve2 input
    printfn "Solve 2 result: %A\n" result2

    Console.ReadLine() |> ignore;

    0 // return an integer exit code
