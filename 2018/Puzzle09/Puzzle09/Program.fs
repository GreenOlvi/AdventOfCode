open System
open System.Text.RegularExpressions
open Utils

let parseInput input =
    let r = new Regex(@"(?<players>\d+) players; last marble is worth (?<value>\d+) points", RegexOptions.Compiled)
    let m = r.Match(input)
    match m.Success with
    | false -> invalidArg "input" "Input has invalid format"
    | true -> (m.Groups.["players"].Value |> int, m.Groups.["value"].Value |> int)

let modulo len i =
    match i % len with
    | r when r >= 0 -> r
    | r -> r + abs i

let addAt i e l =
    let len = List.length l
    match i with
    | 0 ->  List.concat [l; [e]]
    | _ ->
        List.concat [
            List.take i l;
            [e];
            l |> List.skip i |> List.take (len - i)
        ]

let removeAt i l =
    let newList = List.concat [
        List.take i l;
        l |> List.skip (i + 1) |> List.take (List.length l - i - 1);
    ]
    (l.[i], newList)

let emptyScores players = seq {1..players} |> Seq.fold (fun m p -> m |> Map.add p 0) Map.empty

let addScore (player:int) (i:int) (scores:Map<int,int>) : Map<int,int> =
    let newScore = (scores |> Map.find player) + i
    scores |> Map.add player newScore

let printMarbles i marbles curr =
    printf "[%i] " i
    marbles |> Seq.iteri (fun c m -> if c = curr then printf "%3d*" m else printf "%3d " m)
    printfn ""

let solve1 (players, last) =
    let playerFun i = 
        let pMod = modulo players
        1 + pMod (i - 1)

    let rec addMarble (marbles, current, (scores : Map<int,int>)) i =
        let moduloLen = modulo (List.length marbles)
        match i with
        | m when m % 23 = 0 ->
            let newPos = (moduloLen (current - 7))
            let (s, newMarbles) = removeAt newPos marbles
            let newScores = scores |> addScore (playerFun i) (i + s)
            printMarbles (playerFun i) newMarbles newPos
            (newMarbles, newPos, newScores)
        | _ ->
            let newPos = 1 + moduloLen (current + 1)
            let newMarbles = addAt newPos i marbles
            printMarbles (playerFun i) newMarbles newPos
            (newMarbles, newPos, scores)

    let (_, _, scores) =
        seq {1..last}
        |> Seq.fold addMarble ([0], 0, emptyScores players)

    scores
    |> Seq.map (fun kv -> kv.Value)
    |> Seq.max

[<EntryPoint>]
let main _ =
    let input = @"477 players; last marble is worth 70851 points" |> parseInput

    Common.test (fun (i, e, l : int seq) -> addAt i e (l |> List.ofSeq)) [
        ((0, 9, Seq.empty), [9]);
        ((0, 9, seq {0..4}), [0; 1; 2; 3; 4; 9]);
        ((1, 9, seq {0..4}), [0; 9; 1; 2; 3; 4]);
        ((2, 9, seq {0..4}), [0; 1; 9; 2; 3; 4]);
        ((5, 9, seq {0..4}), [0; 1; 2; 3; 4; 9]);
    ]

    Common.test (fun (i, l) -> removeAt i (l |> List.ofSeq)) [
        ((0, seq {0..4}), (0, [1; 2; 3; 4]));
        ((1, seq {0..4}), (1, [0; 2; 3; 4]));
        ((4, seq {0..4}), (4, [0; 1; 2; 3]));
        ((0, seq {0..0}), (0, []));
    ]
    
    Common.test (fun i -> i |> parseInput |> solve1) [
        ("9 players; last marble is worth 25 points", 32);
        //("10 players; last marble is worth 1618 points", 8317);
        //("13 players; last marble is worth 7999 points", 146373);
        //("17 players; last marble is worth 1104 points", 2764);
        //("21 players; last marble is worth 6111 points", 54718);
        //("30 players; last marble is worth 5807 points", 37305);
    ]

    //Common.timeResult 1 { return solve1 input } |> ignore

    Console.ReadLine() |> ignore
    0 // return an integer exit code
