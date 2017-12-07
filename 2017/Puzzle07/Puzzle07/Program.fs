open System
open System.IO
open System.Text.RegularExpressions
open Utils.Common
open System.Collections.Generic

type Process = string * int * string list

let processRegex = Regex(@"^(?<name>\w+)\s\((?<weight>\d+)\)(\s->\s(?<subs>.*))?$", RegexOptions.Compiled)
let parseLine line:Process =
    let m = processRegex.Match(line)
    let w = int(m.Groups.Item("weight").Value)
    let s = splitString [| ' '; ',' |] (m.Groups.Item("subs").Value) |> Array.toList
    (m.Groups.Item("name").Value, w, s)

let readInput file =
    File.ReadAllLines file
    |> Array.map parseLine
    |> Array.toList

let solve (input:Process list) =
    let subs =
        input
        |> Seq.map (fun (_, _, s) -> s)
        |> Seq.concat
        |> HashSet
    input
        |> List.filter (fun (n, _, _) -> not(subs.Contains(n)))
        |> List.head

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"
    let testInput =
        "pbga (66)\nxhth (57)\nebii (61)\nhavc (66)\nktlj (57)\nfwft (72) -> ktlj, cntj, xhth\n"
        + "qoyq (66)\npadx (45) -> pbga, havc, qoyq\ntknk (41) -> ugml, padx, fwft\njptl (61)\n"
        + "ugml (68) -> gyxo, ebii, jptl\ngyxo (61)\ncntj (57)"
        |> splitString [| '\n'; '\r' |]
        |> Array.map parseLine
        |> Array.toList

    printfn "--- Tests ---"
    test parseLine [
        ("pbga (66)", ("pbga", 66, []));
        ("fwft (72) -> ktlj, cntj, xhth", ("fwft", 72, ["ktlj"; "cntj"; "xhth";]));
    ]

    test solve [(testInput, ("tknk", 41, ["ugml"; "padx"; "fwft"]))]

    let result1 = timeIt (fun () -> solve input)
    printfn "Result 1: %A" result1

    Console.ReadLine() |> ignore
    0 // return an integer exit code
