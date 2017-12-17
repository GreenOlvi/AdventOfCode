open System
open Utils.Common

let insertAfter index element (list:'a list) =
    let before = Seq.take (index + 1) list |> List.ofSeq
    let after = list |> Seq.skip (index + 1) |> List.ofSeq
    Seq.concat [before; [element]; after] |> List.ofSeq

let stepOn steps elements start =
    (start + steps) % elements

let solve1 input =
    let rec runSteps (buffer:int list) pos n limit =
        match n with
        | a when a = limit -> buffer
        | _ ->
            let i = stepOn input buffer.Length pos
            let newBuffer = insertAfter i n buffer
            runSteps newBuffer (i + 1) (n + 1) limit
    let buffer = runSteps [0] 0 1 2018
    buffer.[buffer |> List.findIndex (fun i -> i = 2017) |> (+) 1]

let solve2 input =
    let limit = 50000001
    let rec loop elems atOne pos = 
        match elems with
        | a when a = limit -> atOne
        | _ ->
            let i = stepOn input elems pos
            let el = if i = 0 then elems else atOne
            loop (elems + 1) el i
    loop 1 0 0

[<EntryPoint>]
let main _ = 
    let input = 344

    printfn "--- Tests ---\n"
    test3 insertAfter [
        ((2, 4, [0; 1; 2; 3]), [0; 1; 2; 4; 3]);
        ((3, 4, [0; 1; 2; 3]), [0; 1; 2; 3; 4]);
        ((0, 4, [0; 1; 2; 3]), [0; 4; 1; 2; 3]);
        ((0, 1, [0]), [0; 1]);
    ];

    test3 stepOn [
        ((10, 15, 2), 12);
        ((10, 15, 8), 3);
    ];

    test solve1 [(3, 638)]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A" result1

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
