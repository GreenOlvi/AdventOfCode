open System
open System.IO
open Utils

type Direction = N | W | S | E
type Control =
    | StartString // ^
    | EndString   // $
    | StartOption // (
    | EndOption   // )
    | NextOption  // |

type Token = Dir of Direction | Cont of Control

type Act =
    | D of Direction
    | O of Act list list
type Actions = Act list

type CharSeq = seq<char>

let parseToken (input : CharSeq) =
    let c = input |> Seq.head
    let rest = input |> Seq.skip 1
    match c with
    | '^' -> (Cont StartString, rest)
    | '$' -> (Cont EndString, rest)
    | '(' -> (Cont StartOption, rest)
    | ')' -> (Cont EndOption, input)
    | '|' -> (Cont NextOption, input)
    | 'N' -> (Dir N, rest)
    | 'W' -> (Dir W, rest)
    | 'S' -> (Dir S, rest)
    | 'E' -> (Dir E, rest)
    | _ -> invalidArg "c" (sprintf "Invalid character '%c'" c)


let parseInput input : Actions =
    let rec parseActions input =
        let rec parseOptions input parsed =
            let (o, r) = parseActions input
            match parseToken r with
            | (Cont c, rest) when c = NextOption -> parseOptions (rest |> Seq.skip 1) (List.append parsed [o])
            | (Cont c, rest) when c = EndOption -> (parsed, (rest |> Seq.skip 1))
            | (t, _) -> invalidOp (sprintf "Expected NextOption or EndOption, got %A" t)

        let rec parseMore input parsed =
            match parseToken input with
            | (Dir d, rest) -> parseMore rest (List.append parsed [D d])
            | (Cont c, rest) when (c = EndString || c = NextOption || c = EndOption) -> (parsed, rest)
            | (Cont c, rest) when c = StartOption -> 
                let (o, r) = parseOptions rest []
                (List.append parsed [O o], r)
            | (t, _) -> invalidOp (sprintf "Unexpected token %A" t)

        parseMore input []

    match parseToken input with
    | (Cont c, rest) when c = StartString ->
        let (actions, _) = parseActions rest
        actions
    | (t, _) -> invalidOp (sprintf "Expected StartString token, got %A" t)

let readInput file = File.ReadAllText file |> parseInput

[<EntryPoint>]
let main _ =
    Common.test parseInput [
        ("^WNE$", [D W; D N; D E]);
        ("^ENWWW(NEEE|SSE(EE|N))$", [D E; D N; D W; D W; D W; O [[D N; D E; D E; D E]; [D S; D S; D E; O [[D E; D E]; [D N]]]]]);
    ]

    //let input = readInput "input.txt"

    Console.ReadLine() |> ignore
    0 // return an integer exit code
