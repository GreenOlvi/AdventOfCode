open System
open System.IO
open System.Text.RegularExpressions
open Utils

type State = Set<int64>

let parseState text : State =
    text
    |> Seq.mapi (fun i p -> match p with '#' -> Some (int64 i) | _ -> None)
    |> Seq.choose id
    |> set

let parseInitialState line =
    let regex = new Regex(@"^initial state: (?<state>[\.#]+)$", RegexOptions.Compiled)
    let m = regex.Match(line)
    match m.Success with 
    | false -> invalidArg "line" "Invalid init line format"
    | true -> m.Groups.["state"].Value |> parseState

let parsePotsToNum pots =
    pots |> Seq.mapi (fun i p ->
        match (p = '#') with
        | true -> 1 <<< (4 - i)
        | false -> 0
    ) |> Seq.sum

let parseRuleLine line =
    let lineRegex = new Regex(@"^(?<val>[\.#]+) => (?<plant>[\.#])$")
    let m = lineRegex.Match(line)
    match m.Success with
    | false -> invalidArg "line" "Invalid rule line format"
    | true ->
        let plant = m.Groups.["plant"].Value = "#"
        let num = parsePotsToNum m.Groups.["val"].Value
        (num, plant)

let parseRules lines = lines |> Seq.map parseRuleLine |> Map

let parseInput lines =
    let init = parseInitialState (Array.item 0 lines)
    let rules = parseRules (lines |> Array.skip 2)
    (init, rules)

let readInput file = File.ReadAllLines file

let buildNum bits =
    bits
    |> Seq.mapi (fun i b -> match b with true -> 1 <<< (4 - i) | false -> 0)
    |> Seq.sum

let checkRule rules i = match (rules |> Map.tryFind i) with Some i -> i | None -> false

let cycle rules (state : State) =
    match (Set.isEmpty state) with
    | true -> Set.empty
    | false ->
        let isSet i = state |> Set.contains i
        let min = Set.minElement state - 2L
        let max = Set.maxElement state + 2L
        seq {min..max}
        |> Seq.map (fun i ->
            let num = seq {i-2L .. i+2L} |> Seq.map isSet |> buildNum
            let newPot = checkRule rules num
            match newPot with
            | true -> Some i
            | false -> None
            )
        |> Seq.choose id
        |> set

let solve1 initialState rules =
    let c = cycle rules
    let r = seq {0..19} |> Seq.fold (fun s _ -> c s) initialState
    r |> Seq.sum

let deparseState (state : State) =
    let offset = state |> Set.minElement
    let max = state |> Set.maxElement
    let ch =
        seq {offset .. max}
        |> Seq.map (fun e -> match (state |> Set.contains e) with true -> '#' | false -> '.' )
    (String.Join(String.Empty, ch), offset)

let solve2 initialState rules =
    let c = cycle rules

    let addCache state newState offset cache = cache |> Map.add state (newState, offset)
    let lookupCache state cache = cache |> Map.tryFind state

    let rec loop state offset cache (i : int64) =
        match i with
        | 0L -> (state, offset)
        | _ -> 
            match (cache |> lookupCache state) with
            | Some (s, o) -> 
                match (state = s) with
                | false -> loop s (offset + o) cache (i - 1L)
                | true -> (state, offset + (o * i))
            | None -> 
                let (s, o) = c (parseState state) |> deparseState
                loop s (offset + o) (cache |> addCache state s o) (i - 1L)

    let (s, o) = deparseState initialState
    let (rs, ro) = loop s o Map.empty 50_000_000_000L
    rs |> parseState |> Seq.map (fun e -> e + ro) |> Seq.sum

[<EntryPoint>]
let main _ =
    let testInput =[|
        "initial state: #..#.#..##......###...###";
        "";
        "...## => #";
        "..#.. => #";
        ".#... => #";
        ".#.#. => #";
        ".#.## => #";
        ".##.. => #";
        ".#### => #";
        "#.#.# => #";
        "#.### => #";
        "##.#. => #";
        "##.## => #";
        "###.. => #";
        "###.# => #";
        "####. => #"
    |]

    let testInit = set [0L; 3L; 5L; 8L; 9L; 16L; 17L; 18L; 22L; 23L; 24L]
    let testRules =
        [3; 4; 8; 10; 11; 12; 15; 21; 23; 26; 27; 28; 29; 30]
        |> List.map (fun e -> (e, true))
        |> Map

    Common.test parseRuleLine [
        ("..... => #", (0, true));
        ("....# => .", (1, false));
        ("...#. => .", (2, false));
        ("..#.. => #", (4, true));
        (".#... => #", (8, true));
        ("#.... => .", (16, false));
        ("##.#. => .", (26, false));
    ]

    Common.test parseInput [
        (testInput, (testInit, testRules))
    ]

    Common.test buildNum [
        ([true; false; false; false; false], 16)
        ([false; true; false; false; false], 8)
        ([false; false; true; false; false], 4)
        ([false; false; false; true; false], 2)
        ([false; false; false; false; true], 1)
        ([false; false; false; false; false], 0)
        ([true; true; false; true; false], 26)
    ]

    Common.test (fun (init, rules) -> cycle rules init) [
        ((set [], [(4, true)] |> Map), set [])
        ((set [0L], [(4, true)] |> Map), set [0L])
        ((set [0L], [(8, true)] |> Map), set [1L])
        ((set [0L; 1L; 3L], [(26, true); (16, true)] |> Map), set [2L; 5L])
        ((testInit, testRules), set[0L; 4L; 9L; 15L; 18L; 21L; 24L])
    ]

    Common.test deparseState [
        (set [0L; 1L; 3L], ("##.#", 0L));
        (set [5L; 7L; 9L], ("#.#.#", 5L));
        (set [-5L; -7L; -9L], ("#.#.#", -9L));
    ]

    Common.test (fun (s, r) -> solve1 s r) [
        ((testInit, testRules), 325L)
    ]

    let (init, rules) = readInput "input.txt" |> parseInput

    let result1 = Common.timeAction (fun () -> solve1 init rules)
    printfn "Result 1: %A" result1

    let result2 = Common.timeAction (fun () -> solve2 init rules)
    printfn "Result 2: %A" result2

    Console.ReadLine() |> ignore
    0 // return an integer exit code
