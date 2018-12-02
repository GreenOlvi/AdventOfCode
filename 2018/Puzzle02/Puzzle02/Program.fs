open System
open System.IO
open Utils

let hasValue (counts:Map<char, int>) i =
    counts |> Seq.where (fun kv -> kv.Value = i) |> Seq.isEmpty |> not

let bool2int v = match v with true -> 1 | false -> 0

let analyze (s:string) : int * int =
    let nextLetter counts l =
        match (Map.containsKey l counts) with
        | true -> counts |> Map.add l (counts.[l] + 1)
        | false -> counts |> Map.add l 1

    let counts = s |> Seq.fold nextLetter Map.empty
    (hasValue counts 2 |> bool2int, hasValue counts 3 |> bool2int)

let matchEnough (s1:string) (s2:string) =
    let diff = Seq.zip s1 s2 |> Seq.filter (fun (l1, l2) -> l1 <> l2) |> Seq.length
    diff <= 1

let solve1 input =
    let (s2, s3) =
        input
        |> Seq.map analyze
        |> Seq.fold (fun (c2, c3) (h2, h3) -> (c2 + h2, c3 + h3)) (0, 0)
    s2 * s3

let solve2 input : string =
    let rec findMatching (strings:string list) =
        match strings with
        | [] -> invalidOp "Match not found"
        | s::ss ->
            let matching = ss |> Seq.tryFind (matchEnough s)
            match (matching) with
            | None -> findMatching ss
            | Some m -> (s, m)
    let (s1, s2) = findMatching (List.ofSeq input)
    Seq.zip s1 s2
    |> Seq.filter (fun (l1, l2) -> l1 = l2)
    |> Seq.map (fun (l1, _) -> l1)
    |> String.Concat

let readInput file = File.ReadAllLines file

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    Common.test analyze [
        ("abcdef", (0, 0));
        ("bababc", (1, 1));
        ("abbcde", (1, 0));
        ("abcccd", (0, 1));
        ("aabcdd", (1, 0));
        ("abcdee", (1, 0));
        ("ababab", (0, 1));
    ]

    Common.test solve2 [
        (["abcde"; "fghij"; "klmno"; "pqrst"; "fguij"; "axcye"; "wvxyz"], "fgij");
    ]

    Common.test solve1 [
        (["abcdef"; "bababc"; "abbcde"; "abcccd"; "aabcdd"; "abcdee"; "ababab"], 12)
    ]

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    let result2 = solve2 input
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
