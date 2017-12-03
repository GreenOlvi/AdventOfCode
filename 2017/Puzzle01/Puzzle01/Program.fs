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

let succ i arr =
    let newIndex = (i + 1) % Array.length arr
    arr.[newIndex]

let solve input =
    let arr = input |> Seq.map charToInt |> Array.ofSeq
    arr
        |> Seq.mapi (fun index item -> (item, succ index arr))
        |> Seq.filter (fun (a, b) -> a = b)
        |> Seq.map (fun (a, b) -> a)
        |> Array.ofSeq
        |> Seq.sum

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
    test solve [("1122", 3); ("1111", 4); ("1234", 0); ("91212129", 9)]

    let input = readInput "input.txt"
    let result = solve input
    printfn "Final result: %A" result

    Console.ReadLine() |> ignore;

    0 // return an integer exit code
