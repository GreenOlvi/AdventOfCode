open System
open Utils

let getPower serial (x, y) =
    let rackId = x + 10
    (((rackId * y) + serial) * rackId / 100) % 10 - 5

let toIndex width (x, y) = width * y + x
let toCoords width index = (index % width, index / width)

let buildGrid serial width height =
    Common.buildGrid width height
    |> Seq.map (fun c -> getPower serial c)
    |> Array.ofSeq

let buildSumGrid width height (grid : int array) : int array =
    let a = Array.zeroCreate (width * height)
    let getIndex = toIndex width
    Common.buildGrid width height
    |> Seq.iter (fun (x, y) ->
        let v = grid.[getIndex (x, y)]
        a.[getIndex (x, y)] <-
            match y with
            | 0 ->
                match x with
                | 0 -> v
                | _ -> v + a.[getIndex (x - 1, 0)]
            | _ ->
                match x with
                | 0 -> v + a.[getIndex (0, y - 1)]
                | _ -> v + a.[getIndex (x - 1, y)] + a.[getIndex (x, y - 1)] - a.[getIndex (x - 1, y - 1)])
    a

let buildGridN n width height (sumGrid : int array) =
    //let elements =
    //    Common.buildGrid n n
    //    |> Seq.map (toIndex width)
    //    |> List.ofSeq
    let getIndex = toIndex width
    Common.buildGrid (width - n) (height - n)
    |> Seq.map (
        fun (x, y) -> 
        //sumGrid.[getIndex (x, y)] + )
    //|> Seq.map (fun c -> (c, elements |> List.map (fun e -> grid |> Array.item ((getIndex c) + e))))

let solve1 input =
    buildGrid input 300 300
    |> buildSumGrid 300 300
    |> buildGridN 3 300 300
    |> Seq.map (fun (c, l) -> (c, l |> List.sum))
    |> Seq.maxBy (fun (_, s) -> s)
    
let solve2 input =
    let g = buildGrid input 300 300 |> Array.ofSeq
    seq {1..300}
    |> Seq.map (fun n ->
        printfn "N=%d" n
        g
        |> buildGridN n 300 300
        |> Seq.map (fun (c, l) -> (c, l |> List.sum, n))
        |> Seq.maxBy (fun (_, s, _) -> s))
    |> Seq.maxBy (fun (_, s, _) -> s)

[<EntryPoint>]
let main _ =
    let input = 5468

    Common.test (fun (w, h) -> Common.buildGrid w h |> List.ofSeq) [
        ((1, 1), [(0, 0)]);
        ((2, 1), [(0, 0); (1, 0)]);
        ((1, 2), [(0, 0); (0, 1)]);
        ((2, 2), [(0, 0); (1, 0); (0, 1); (1, 1)]);
    ]

    Common.test (fun (c, sn) -> getPower sn c) [
        (((3, 5), 8), 4);
        (((122, 79), 57), -5);
        (((217, 196), 39), 0);
        (((101, 153), 71), 4);
    ]

    Common.test (fun (w, h, g) -> buildSumGrid w h g) [
        ((1, 1, [|1|]), [|1|]);
        ((2, 1, [|1; 2|]), [|1; 3|]);
        ((1, 2, [|1; 2|]), [|1; 3|]);
        ((2, 2, [|1; 2; 3; 4|]), [|1; 3; 4; 10|]);
        ((3, 3, [|1; 1; 1; 1; 1; 1; 1; 1; 1|]), [|1; 2; 3; 2; 4; 6; 3; 6; 9|]);
    ]

    Common.test solve1 [(42, ((21, 61), 30))]
    //Common.test solve2 [
    //    (18, ((90, 269), 113, 16));
    //]

    Common.timeResult 1 { return solve1 input } |> ignore
    Common.timeResult 2 { return solve2 input } |> ignore

    Console.ReadLine() |> ignore
    0 // return an integer exit code
