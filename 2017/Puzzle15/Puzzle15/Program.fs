open System
open Utils

let generator (factor:uint64) (prev:uint64) : uint64 = (factor * prev) % 2147483647UL

let genA = generator 16807UL
let genB = generator 48271UL

let rec pickyGen gen m input =
    let r = gen input
    match (r &&& m) with
    | 0UL -> r
    | _ -> pickyGen gen m r

let rec pickyGenA input = pickyGen genA 3UL input
let rec pickyGenB input = pickyGen genB 7UL input

let runRounds (gA, sA) (gB, sB) count =
    let round (pA, pB, c) =
        let a = gA pA
        let b = gB pB
        let eq = (a &&& 65535UL) = (b &&& 65535UL)
        let nc = match eq with | true -> c + 1 | false -> c
        (a, b, nc)
    let (_, _, c) = seq {1..count} |> Seq.fold (fun acc i -> round acc) (sA, sB, 0)
    c

let solve1 sA sB = runRounds (genA, sA) (genB, sB) 40000000

let solve2 sA sB = runRounds (pickyGenA, sA) (pickyGenB, sB) 5000000

[<EntryPoint>]
let main argv =
    let inputA = 618UL
    let inputB = 814UL

    Common.test genA [
        (65UL, 1092455UL);
        (1092455UL, 1181022009UL);
        (1181022009UL, 245556042UL);
        (245556042UL, 1744312007UL);
        (1744312007UL, 1352636452UL);
    ]

    Common.test genB [
        (8921UL, 430625591UL);
        (430625591UL, 1233683848UL);
        (1233683848UL, 1431495498UL);
        (1431495498UL, 137874439UL);
        (137874439UL, 285222916UL);
    ]

    Common.test pickyGenA [
        (65UL, 1352636452UL);
        (1352636452UL, 1992081072UL);
        (1992081072UL, 530830436UL);
        (530830436UL, 1980017072UL);
        (1980017072UL, 740335192UL);
    ]

    Common.test pickyGenB [
        (8921UL, 1233683848UL);
        (1233683848UL, 862516352UL);
        (862516352UL, 1159784568UL);
        (1159784568UL, 1616057672UL);
        (1616057672UL, 412269392UL);
    ]

    Common.test (runRounds (genA, 65UL) (genB, 8921UL)) [(5, 1)]

    Common.test (runRounds (pickyGenA, 65UL) (pickyGenB, 8921UL)) [
        (1055, 0); (1056, 1); (5000000, 309);
    ]

    let result1 = Common.timeIt (fun () -> solve1 inputA inputB)
    printfn "Result 1: %A" result1

    let result2 = Common.timeIt (fun () -> solve2 inputA inputB)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
