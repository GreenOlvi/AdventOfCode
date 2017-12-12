open System
open Utils.Common
open System.Collections.Generic

let toCartesian (id:int):(int*int) =
    match id with
        | 0 -> invalidArg "id" "Should be greater than 1"
        | 1 -> (0, 0)
        | _ ->
            let layer = Math.Floor((Math.Sqrt(float(id) - 1.0) - 1.0) / 2.0) + 1.0
            let n = 8.0 * layer
            let side = 2.0 * layer
            let start = 4.0 * layer * layer - 4.0 * layer + 2.0
            let d = float(id) - start
            let q = Math.Floor((d / n) * 4.0)
            let o = d - q * float(side) - layer + 1.0
            let (l', o') = int layer, int o
            match int(q) with
                | 0 -> (l', o')
                | 1 -> (-o', l')
                | 2 -> (-l', -o')
                | 3 -> (o', -l')
                | _ -> invalidArg "quarter" "Expected value in 0..3"

let posCache = Dictionary<int * int, int>()
posCache.Add((0, 0), 1)

let fillCache (x, y) =
    let layer = max (abs x ) (abs y )
    let layerStart = 4 * layer * layer - 4 * layer + 2
    let layerEnd = layerStart + 8 * layer - 1
    [layerStart .. layerEnd]
        |> List.iter (fun id ->
            let c = toCartesian(id)
            if not(posCache.ContainsKey(c)) then posCache.Add(c, id))

let toSpiral coords =
    match posCache.ContainsKey(coords) with
    | true ->
        posCache.Item(coords)
    | false ->
        fillCache coords
        posCache.Item(coords)

let neighbours = [(0, 1); (1, 1); (1, 0); (1, -1); (0, -1); (-1, -1); (-1, 0); (-1, 1)]

let sumCache = Dictionary<int, int>(dict [(1, 1)])
let rec countSum id =
    match sumCache.ContainsKey(id) with
    | true -> sumCache.[id]
    | false -> 
        let (x, y) = toCartesian id
        let s =
            neighbours
            |> List.map (fun (nx, ny) -> toSpiral (x + nx, y + ny))
            |> List.filter (fun i -> i < id)
            |> List.map (fun i -> countSum(i))
            |> List.sum
        sumCache.Add(id, s)
        s

let solve1 (input:int) =
    let (x, y) = toCartesian input
    abs(x) + abs(y)

let solve2 input =
    let rec loop id =
        let sum = countSum id
        match sum with
        | s when s > input -> s
        | _ -> loop (id + 1)
    loop 1

[<EntryPoint>]
let main _ = 
    let input = 325489

    printfn "--- Tests ---"
    test toCartesian [(34, (0, 3)); (5, (-1, 1))]
    test toCartesian [(1, (0, 0)); (2, (1, 0)); (3, (1, 1)); (5, (-1, 1)); (10, (2, -1)); (23, (0, -2))]
    test solve1 [(1, 0); (12, 3); (23, 2); (1024, 31)]

    let result1 = solve1 input
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---"
    test toSpiral ([1..81] |> List.map (fun id -> (toCartesian id, id)))
    test countSum [
        (1, 1);
        (2, 1);
        (3, 2);
        (5, 5);
        (7, 11);
        (9, 25);
        (11, 54);
        (17, 147)
    ]

    let result2 = solve2 input
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
