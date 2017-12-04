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

let splitWords (line:string) =
    line.Split([|'\t'; ' '|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

let splitLines (input:string) =
    input.Split([|'\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

let readInput file =
    File.ReadAllText(file)
        |> splitLines
        |> List.map splitWords

let rec isValidPassphrase cmp words =
    let checkWord word ws =
        let repeats = List.exists (fun w -> cmp word w) ws
        match repeats with
            | true -> false
            | false -> isValidPassphrase cmp ws

    match words with
        | h::t -> checkWord h t
        | _ -> true

let getKey (word:string) =
    word.ToLower().ToCharArray()
        |> Array.sort
        |> String.Concat

let isValidPassphrase1 = isValidPassphrase (fun a b -> a = b)
let isValidPassphrase2 = isValidPassphrase (fun a b -> getKey(a) = getKey(b))

let solve validFun passphrases = 
    passphrases
        |> List.filter (fun p ->  validFun p)
        |> List.length

let solve1 = solve isValidPassphrase1
let solve2 = solve isValidPassphrase2

[<EntryPoint>]
let main argv = 
    let input = readInput "input.txt"

    printfn "--- Tests ---"

    test isValidPassphrase1 [
        (splitWords "aa bb cc dd ee", true);
        (splitWords "aa bb cc dd aa", false);
        (splitWords "aa bb cc dd aaa", true);
    ]

    let result1 = solve1 input
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---"

    test isValidPassphrase2 [
        (splitWords "abcde fghij", true);
        (splitWords "abcde xyz ecdab", false);
        (splitWords "a ab abc abd abf abj", true);
        (splitWords "iiii oiii ooii oooi oooo", true);
        (splitWords "oiii ioii iioi iiio", false);
    ]

    let result2 = solve2 input
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
