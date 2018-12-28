open System
open System.IO
open Utils

type Point = int * int * int * int
type Constellation = Point list
type Constellations = Constellation list

let dist ((a1, a2, a3, a4) : Point) ((b1, b2, b3, b4) : Point) : int =
    Math.Abs(a1 - b1) + Math.Abs(a2 - b2) + Math.Abs(a3 - b3) + Math.Abs(a4 - b4)

let isPartOf (p : Point) (c : Constellation) : bool = c |> Seq.exists (fun cp -> dist p cp <= 3 )

let findMatching (p : Point) (cs : Constellation seq) = cs |> Seq.filter (fun c -> isPartOf p c) |> List.ofSeq

let addPoint state point =
    let matching = findMatching point state
    match matching with
    | [] ->
        [point]::state
    | m::ms ->
        let merged =  point::m |> List.append (ms |> List.collect (fun c -> c))
        state |> List.except matching |> List.append [merged]

let solve (input : Point seq) : int =
    input |> Seq.fold addPoint [] |> Seq.length

let parseInput line =
    let s = Common.splitString [','] line |> Array.map Int32.Parse
    (s.[0], s.[1], s.[2], s.[3])

let readInput file = File.ReadAllLines file |> Seq.map parseInput

[<EntryPoint>]
let main _ =
    Common.test (fun (a, b) -> isPartOf a b) [
        (((0, 0, 0, 0), [(0, 0, 0, 2)]), true);
        (((0, 0, 0, 0), [(0, 4, 0, 0)]), false);
        (((0, 0, 0, 0), [(0, 0, 0, 6); (1, 0, 0, 1)]), true);
        (((0, 0, 0, 0), [(0, 0, 0, 6); (6, 0, 0, 2)]), false);
    ]

    Common.test (fun (s, p) -> addPoint s p) [
        (([], (0, 0, 0, 0)), [[(0, 0, 0, 0)]]);
        (([[(0, 0, 0, 0)]], (0, 0, 0, 2)),
            [[(0, 0, 0, 2); (0, 0, 0, 0)]]);
        (([[(0, 0, 0, 0)]; [(0, 0, 0, 5)]], (0, 0, 1, 5)),
            [[(0, 0, 1, 5); (0, 0, 0, 5)]; [(0, 0, 0, 0)]]);
        (([[(0, 0, 0, 0)]; [(0, 0, 0, 5)]], (0, 0, 0, 3)),
            [[(0, 0, 0, 5); (0, 0, 0, 3); (0, 0, 0, 0)]]);
    ]

    Common.test solve [
        ([(0,0,0,0); (3,0,0,0); (0,3,0,0); (0,0,3,0); (0,0,0,3); (0,0,0,6); (9,0,0,0); (12,0,0,0);], 2);
        ([(-1,2,2,0); (0,0,2,-2); (0,0,0,-2); (-1,2,0,0); (-2,-2,-2,2); (3,0,2,-1); (-1,3,2,2); (-1,0,-1,0); (0,2,1,-2); (3,0,0,0)], 4);
        ([(1,-1,0,1); (2,0,-1,0); (3,2,-1,0); (0,0,3,1); (0,0,-1,-1); (2,3,-2,0); (-2,2,0,0); (2,-2,0,-1); (1,-1,0,-1); (3,2,0,2)], 3);
        ([(1,-1,-1,-2); (-2,-2,0,1); (0,2,1,3); (-2,3,-2,1); (0,2,3,-2); (-1,-1,1,-2); (0,-2,-1,0); (-2,2,3,-1); (1,2,2,0); (-1,-2,0,-2)], 8);
    ]

    let input = readInput "input.txt"
    let result = solve input
    printfn "Result: %A" result

    Console.ReadLine() |> ignore
    0 // return an integer exit code
