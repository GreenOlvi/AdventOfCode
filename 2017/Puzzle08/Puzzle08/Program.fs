open System
open System.IO
open System.Text.RegularExpressions
open Utils.Common
open System.Collections.Generic

type Op =
    | Inc of string * int
    | Dec of string * int

type Cond =
    | Gt of string * int
    | Lt of string * int
    | Ge of string * int
    | Le of string * int
    | Eq of string * int
    | Ne of string * int

type Instruction = Op * Cond

type Registers() = class
    let reg = Dictionary<string, int>()
    let mutable allTimeMax = 0

    let setReg r v =
        if v > allTimeMax then allTimeMax <- v
        reg.[r] <- v

    member x.GetValue r =
        match reg.ContainsKey(r) with
        | true -> reg.[r]
        | false ->
            reg.Add(r, 0)
            0
    
    member x.Operation (o:Op) =
        match o with
        | Inc (r, v) ->
            let v' = x.GetValue(r) + v
            setReg r v'
        | Dec (r, v) ->
            let v' = x.GetValue(r) - v
            setReg r v'
        x

    member x.Condition (c:Cond) =
        match c with
        | Gt (r, v) -> x.GetValue(r) > v
        | Lt (r, v) -> x.GetValue(r) < v
        | Ge (r, v) -> x.GetValue(r) >= v
        | Le (r, v) -> x.GetValue(r) <= v
        | Eq (r, v) -> x.GetValue(r) = v
        | Ne (r, v) -> x.GetValue(r) <> v
        | _ -> invalidArg "c" "Invalid condition"

    member x.RunInstruction (op, cond) =
        match x.Condition cond with
        | true -> x.Operation op
        | false -> x
    
    member x.MaxValue = reg.Values |> Seq.max
    member x.AllTimeMax = allTimeMax
end

let instructionRegex = Regex(@"^(?<name>\w+)\s(?<op>inc|dec)\s(?<val>-?\d+)\sif\s(?<cname>\w+)\s(?<cop>\S+)\s(?<cval>-?\d+)$", RegexOptions.Compiled);
let parseInstruction line =
    let m = instructionRegex.Match line
    let name = m.Groups.["name"].Value
    let value = m.Groups.["val"].Value |> int
    let cName = m.Groups.["cname"].Value
    let cVal = m.Groups.["cval"].Value |> int

    let op = match m.Groups.["op"].Value with
    | "inc" -> Inc (name, value)
    | "dec" -> Dec (name, value)
    | _ -> invalidArg "op" "Invalid operation"

    let cond = match m.Groups.["cop"].Value with
    | ">" -> Gt (cName, cVal)
    | "<" -> Lt (cName, cVal)
    | ">=" -> Ge (cName, cVal)
    | "<=" -> Le (cName, cVal)
    | "==" -> Eq (cName, cVal)
    | "!=" -> Ne (cName, cVal)
    | _ -> invalidArg "cond" "Invalid condition"

    (op, cond)

let readInput file =
    File.ReadAllLines file
    |> Array.map parseInstruction
    |> Array.toList

let solve (input:Instruction list) =
    let initR = Registers()
    let out = input |> List.fold (fun (r:Registers) i -> r.RunInstruction i) initR
    (out.MaxValue, out.AllTimeMax)

[<EntryPoint>]
let main _ = 
    let input = readInput "input.txt"
    printfn "%A" input

    printfn "--- Tests ---\n"
    test parseInstruction [
        ("b inc 5 if a > 1", (Inc ("b", 5), Gt ("a", 1)));
        ("a inc 1 if b < 5", (Inc ("a", 1), Lt ("b", 5)));
        ("c dec -10 if a >= 1", (Dec ("c", -10), Ge ("a", 1)));
        ("c inc -20 if c == 10", (Inc ("c", -20), Eq ("c", 10)));
    ]

    let result = timeIt (fun () -> solve input)
    printfn "Result 1: %A\nResult 2: %A" (fst result) (snd result)

    Console.ReadLine() |> ignore
    0 // return an integer exit code
