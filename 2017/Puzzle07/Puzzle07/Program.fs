open System
open System.IO
open System.Text.RegularExpressions
open Utils.Common
open System.Collections.Generic

type Process = {name:string; weight:int; subs:string list}
type ProcessNode = {name:string; weight:int; subs:ProcessNode list}

type Tree (input:IDictionary<string, Process>) = class
    let rootProcess =
        let subs =
            input.Values
            |> Seq.map (fun p -> p.subs)
            |> Seq.concat
            |> Set.ofSeq
        input.Values
            |> Seq.filter (fun p -> not(subs.Contains p.name))
            |> Seq.head

    let root =
        let rec buildNode (p:Process) =
            match p.subs with
            | [] -> {name = p.name; weight = p.weight; subs = []}
            | subList ->
                let subs = 
                    subList
                    |> List.map (fun p -> input.[p])
                    |> List.map (fun p -> buildNode p)
                {name = p.name; weight = p.weight; subs = subs}
        buildNode rootProcess

    let trySearchTree p =
        let rec search (node) =
            match p(node) with
            | true -> Some node
            | false -> Seq.tryPick search node.subs
        search root

    let searchTree p =
        match trySearchTree(p) with
        | Some n -> n
        | None -> invalidArg "node" "Node not found"

    member x.Root = root
    member x.TryGetNode(name) = trySearchTree (fun node -> node.name = name)
    member x.GetNode(name) = searchTree (fun node -> node.name = name)
end

let rec totalWeight (node:ProcessNode) =
    node.weight + List.sumBy (fun n -> totalWeight n) node.subs
let isBalanced (node:ProcessNode) =
    node.subs
    |> List.map (fun n -> totalWeight n)
    |> List.groupBy id
    |> List.length = 1

let getUnbalancedSub (node:ProcessNode) =
    let groups =
        node.subs
        |> List.groupBy (fun n -> totalWeight n)
    let targetWeight = groups |> List.find (fun g -> (snd g).Length > 1) |> fst
    let unbalancedSub = groups |> List.find (fun g -> (snd g).Length = 1) |> snd |> List.head
    (unbalancedSub, targetWeight)

let processRegex = Regex(@"^(?<name>\w+)\s\((?<weight>\d+)\)(\s->\s(?<subs>.*))?$", RegexOptions.Compiled)
let parseLine line:Process =
    let m = processRegex.Match(line)
    let w = int(m.Groups.Item("weight").Value)
    let s = splitString [| ' '; ',' |] (m.Groups.Item("subs").Value) |> Array.toList
    {name = m.Groups.Item("name").Value; weight = w; subs = s}

let parseInput lines =
    lines
    |> Array.map parseLine
    |> Array.map (fun p -> (p.name, p))
    |> dict
    |> Tree

let readInput file = File.ReadAllLines file |> parseInput

let solve1 (input:Tree) = input.Root.name
let solve2 (input:Tree) = 
    let rec loop node =
        let (n, weight) = getUnbalancedSub node
        match isBalanced(n) with
        | true -> weight - List.sumBy (fun n -> totalWeight n) n.subs
        | false -> loop n
    loop input.Root

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"
    let testInput =
        "pbga (66)\nxhth (57)\nebii (61)\nhavc (66)\nktlj (57)\nfwft (72) -> ktlj, cntj, xhth\n"
        + "qoyq (66)\npadx (45) -> pbga, havc, qoyq\ntknk (41) -> ugml, padx, fwft\njptl (61)\n"
        + "ugml (68) -> gyxo, ebii, jptl\ngyxo (61)\ncntj (57)"
        |> splitString [| '\n'; '\r' |]
        |> parseInput

    printfn "--- Tests ---"
    test parseLine [
        ("pbga (66)", {name = "pbga"; weight = 66; subs = []});
        ("fwft (72) -> ktlj, cntj, xhth", {name = "fwft"; weight = 72; subs = ["ktlj"; "cntj"; "xhth";]});
    ]

    test solve1 [(testInput, "tknk")]

    let result1 = timeIt (fun () -> solve1 input)
    printfn "Result 1: %A\n" result1

    printfn "--- Tests ---"

    test (fun n ->
        match testInput.TryGetNode(n) with
        | Some p -> Some p.name
        | None -> None) [
        ("xxxx", None);
        ("pbga", Some "pbga");
        ("tknk", Some "tknk");
    ]

    test totalWeight [
        (testInput.GetNode("gyxo"), 61);
        (testInput.GetNode("ugml"), 251);
        (testInput.Root, 778);
    ]

    test solve2 [(testInput, 60)]

    let result2 = timeIt (fun () -> solve2 input)
    printfn "Result 2: %A\n" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
