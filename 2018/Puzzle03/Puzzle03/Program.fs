open System
open System.IO
open Utils

type Claim = { id: int; x: int; y: int; width: int; height: int }
type FabricPiece = Single of int | Multiple of int list
type Fabric = Map<(int * int), FabricPiece>

let claimPiece f p id =
    match Map.containsKey p f with
    | false -> Single id
    | true ->
        match f.[p] with
        | Single s -> Multiple [id; s]
        | Multiple ids -> Multiple (id::ids)

let setClaim (f:Fabric) (c:Claim) : Fabric =
    let pieces = Common.cartesian (seq {c.x..(c.x + c.width - 1)}) (seq {c.y..(c.y + c.height - 1)})
    pieces |> Seq.fold (fun f' p -> Map.add p (claimPiece f' p c.id) f') f

let makeClaims claims = claims |> Seq.fold (fun f c -> setClaim f c) Map.empty

let parseLine line =
    let p = line |> Common.splitString ['#'; ' '; '@'; ','; ';'; ':'; 'x'] |> Array.map Int32.Parse
    { id = p.[0]; x = p.[1]; y = p.[2]; width = p.[3]; height = p.[4]; }

let readInput file =
    File.ReadAllLines file
    |> Seq.map parseLine

let solve1 input =
    input
    |> makeClaims
    |> Seq.sumBy (fun kv -> match kv.Value with Multiple _ -> 1 | _ -> 0)

let solve2 input =
    let free = input |> Seq.map (fun c -> c.id) |> Set
    input
    |> makeClaims
    |> Seq.choose (fun kv -> match kv.Value with Multiple ids -> Some ids | _ -> None)
    |> Seq.collect id
    |> Seq.distinct
    |> Seq.fold (fun s i -> Set.remove i s) free
    |> Seq.item 0

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    Common.test parseLine [
        ("#1 @ 1,3: 4x4", {id = 1; x = 1; y = 3; width = 4; height = 4});
        ("#2 @ 3,1: 4x4", {id = 2; x = 3; y = 1; width = 4; height = 4});
        ("#3 @ 5,5: 2x2", {id = 3; x = 5; y = 5; width = 2; height = 2});
    ]

    Common.test (fun (f, c) -> setClaim f c) [
        ((Map.empty, {id = 3; x = 5; y = 5; width = 2; height = 2}),
            Map.empty
                .Add((5, 5), Single 3)
                .Add((6, 5), Single 3)
                .Add((5, 6), Single 3)
                .Add((6, 6), Single 3));
        ((Map.empty.Add((6, 6), Single 1), {id = 3; x = 5; y = 5; width = 2; height = 2}),
            Map.empty
                .Add((5, 5), Single 3)
                .Add((6, 5), Single 3)
                .Add((5, 6), Single 3)
                .Add((6, 6), Multiple [3; 1]));
    ]

    let testInput = [
        {id = 1; x = 1; y = 3; width = 4; height = 4};
        {id = 2; x = 3; y = 1; width = 4; height = 4};
        {id = 3; x = 5; y = 5; width = 2; height = 2};
    ]

    Common.test solve1 [(testInput, 4)]
    Common.test solve2 [(testInput, 3)]

    let result1 = Common.timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    let result2 = Common.timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
