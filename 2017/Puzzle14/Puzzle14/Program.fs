open System
open Utils

let hexToBit c =
    match c with
    | '0' -> "0000"
    | '1' -> "0001"
    | '2' -> "0010"
    | '3' -> "0011"
    | '4' -> "0100"
    | '5' -> "0101"
    | '6' -> "0110"
    | '7' -> "0111"
    | '8' -> "1000"
    | '9' -> "1001"
    | 'a' -> "1010"
    | 'b' -> "1011"
    | 'c' -> "1100"
    | 'd' -> "1101"
    | 'e' -> "1110"
    | 'f' -> "1111"
    | _ -> invalidArg "c" "Character should be one of [0-9a-f]"

let hexToBinary input =
    input
    |> Seq.map hexToBit
    |> String.concat String.Empty

let getRowSequence input =
    seq {0..127}
    |> Seq.map (fun i -> input + "-" + string i)

let countOnes (input:string) =
    input
    |> Seq.map (fun c ->
        match c with
        | '0' -> 0
        | '1' -> 1
        | _ -> invalidArg "c" "Should be 0 or 1")
    |> Seq.sum

let binaryToSet (hashList:string seq) =
    hashList
    |> Seq.mapi (fun y s ->
        s
        |> Seq.mapi (fun x b -> (x, b))
        |> Seq.filter (fun (_, b) -> match b with | '1' -> true | _ -> false)
        |> Seq.map (fun (x, _) -> (x, y))
    )
    |> Seq.concat
    |> Set.ofSeq

let findRegions (input:(int*int) Set) : int =
    let rec round (count:int) (toCheck:(int*int) list) (positions:(int*int) Set) =
        match toCheck with
        | h::t ->
            let (x, y) = h
            let neighbours =
                [(x - 1, y); (x + 1, y); (x, y - 1); (x, y + 1)]
                |> List.filter (fun p -> Set.contains p positions)
            round count (List.concat [t; neighbours]) (positions.Remove h)
        | [] ->
            match Set.isEmpty positions with
            | true -> count
            | false ->
                let h = Set.minElement positions
                round (count + 1) [h] (Set.remove h positions)
    round 0 [] input

let solve1 input =
    getRowSequence input
    |> Seq.map (KnotHash.knotHash >> hexToBinary >> countOnes)
    |> Seq.sum

let solve2 input =
    let map =
        getRowSequence input
        |> Seq.map (KnotHash.knotHash >> hexToBinary)
        |> binaryToSet
        |> findRegions
    map

[<EntryPoint>]
let main _ = 
    let input = "oundnydw"

    Common.test hexToBinary [
        ("a0c2017", "1010000011000010000000010111")
    ]

    Common.test solve1 [
        ("flqrgnkx", 8108);
    ]

    let result1 = Common.timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    Common.test binaryToSet [
        (["0110"; "1010"], set [(1, 0); (2, 0); (0, 1); (2, 1)])
    ]

    Common.test findRegions [
        (set [], 0);
        (set [(1,1)], 1);
        (set [(1, 0); (2, 0); (0, 1); (2, 1)], 2);
        (set [(0, 0); (2, 0); (1, 1); (3, 1)], 4);
    ]

    Common.test solve2 [
        ("flqrgnkx", 1242)
    ]

    let result2 = Common.timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
