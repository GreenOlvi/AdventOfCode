open System
open System.IO
open Utils.Common

type Part = int*int
type Bridge = Part list
type State = { Bridge:Bridge; Available:Part Set; LastElement:int; }

let partHas element (part:Part) = fst part = element || snd part = element

let bridgeStrength (bridge:Bridge) = bridge |> Seq.sumBy (fun (a, b) -> a + b)

let printBridge (bridge:Part seq) =
    let b = String.Join("--", bridge |> Seq.rev |> Seq.map (fun p -> (fst p).ToString() + "/" + (snd p).ToString()))
    printfn "%s" b

let rec iterBridges (s:State) proj aggr =
    let nextParts =
        s.Available
        |> Seq.filter (partHas s.LastElement)
        |> Seq.distinct
        |> List.ofSeq

    match nextParts with
    | [] -> s.Bridge |> proj
    | _ ->
        nextParts
        |> Seq.map (fun p ->
            let newBridge = p::s.Bridge

            let newLast =
                match p with
                | (a, b) when a = s.LastElement -> b
                | (a, b) when b = s.LastElement -> a
                | _ -> invalidOp "error"

            iterBridges {
                Bridge = newBridge;
                Available = s.Available.Remove p;
                LastElement = newLast; } proj aggr
            )
        |> aggr

let solve1 input =
    let i = input |> Seq.sort |> Set.ofSeq
    iterBridges { Bridge = []; Available = i; LastElement = 0; }
        (fun b -> bridgeStrength b) (Seq.max)

let solve2 input =
    let i = input |> Seq.sort |> Set.ofSeq
    let (_, s) =
        iterBridges { Bridge = []; Available = i; LastElement = 0; }
            (fun b -> ((b |> Seq.length), (bridgeStrength b)) ) (Seq.max)
    s

let readInput file:Part seq =
    File.ReadAllLines file
    |> Seq.map (fun l ->
        match splitString [|'/'|] l with
        | [|a; b|] -> ((int a), (int b))
        | _ -> invalidArg "input" "invalid input format")

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"
    let testInput = [(0, 2); (2, 2); (2, 3); (3, 4); (3, 5); (0, 1); (10, 1); (9, 10)]

    printfn "--- Tests ---\n"
    test solve1 [(testInput, 31)]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    printfn "--- Tests ---\n"
    test solve2 [(testInput, 19)]

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0
