open System
open System.IO
open Utils
open Utils

type Node = {
    ChildCount : int;
    MetaCount : int;
    Children : Node list;
    Meta : int list;
}

type ParseState = { Children : Node list; Input : int list }

let rec parseNode (input : int list) : (Node * int list) =
    let childCount = List.item 0 input
    let metaCount = List.item 1 input
    let s =
        match childCount with
        | 0 -> { Children = []; Input = List.skip 2 input }
        | c when c > 0 -> 
            seq {0..childCount - 1}
            |>  Seq.fold (fun s _ ->
                    let (child, rest) = parseNode s.Input
                    { Children = List.append s.Children [child]; Input = rest }
                ) { Children = []; Input = List.skip 2 input }
        | _ -> invalidOp "Ivalid child count"

    let meta = s.Input |> List.take metaCount
    ({
        ChildCount = childCount;
        MetaCount = metaCount;
        Children = s.Children;
        Meta = meta;
    }, List.skip metaCount s.Input)

let parseInput file =
    File.ReadAllText file
    |> Common.splitString [' ']
    |> Seq.map Int32.Parse
    |> List.ofSeq

let solve1 tree =
    let rec visitNode (node : Node) =
        let childrenMeta =
            node.Children
            |> List.fold (fun s c -> s + (visitNode c)) 0
        childrenMeta + (node.Meta |> List.sum)
    visitNode tree

let solve2 tree =
    let rec visitNode (node : Node) =
        match List.isEmpty node.Children with
        | true -> List.sum node.Meta
        | false ->
            node.Meta
            |> List.map (fun m ->
                let index = m - 1
                match index with
                | i when i < 0 -> 0
                | i when i >= List.length node.Children -> 0
                | i -> visitNode (List.item i node.Children)
                )
            |> List.sum
    visitNode tree

[<EntryPoint>]
let main _ =
    let input = parseInput "input.txt"
    let testInput = [2; 3; 0; 3; 10; 11; 12; 1; 1; 0; 1; 99; 2; 1; 1; 2]

    let testTree = fst (parseNode testInput)
    Common.test solve1 [(testTree, 138)]
    Common.test solve2 [(testTree, 66)]
    
    let tree = Common.timeIt { return (fst (parseNode input)) }

    Common.timeResult 1 { return solve1 tree } |> ignore
    Common.timeResult 2 { return solve2 tree } |> ignore

    Console.ReadLine() |> ignore
    0 // return an integer exit code
