open System
open System.IO
open Utils
open Microsoft.CSharp.RuntimeBinder

type Direction = Up | Down | Left | Right
type TurnDirection = L | St | R
type Position = int * int
type Cart = { position : Position; direction : Direction; nextTurn : TurnDirection }
type OptionalCollision = Cart of Cart | Collision of (Cart seq)
type Track = H | V | I | S | BS
type Tracks = Map<Position, Track>

let mapWidth (m : Map<Position, Track>) : int = m |> Seq.map (fun kv -> fst kv.Key) |> Seq.max
let mapHeight (m : Map<Position, Track>) : int = m |> Seq.map (fun kv -> snd kv.Key) |> Seq.max

let posToOrder width (x, y) = y * width + x

let nextPosition (x, y) d =
    match d with
    | Up -> (x, y - 1)
    | Down -> (x, y + 1)
    | Left -> (x - 1, y)
    | Right -> (x + 1, y)

let nextTurnDir t = match t with | L -> St | St -> R | R -> L

let sDirChange d = match d with Up -> Right | Down -> Left | Left -> Down | Right -> Up
let bsDirChange d = match d with Up -> Left | Down -> Right | Left -> Up | Right -> Down
let iDirChange d turn =
    match d with
    | Up -> match turn with | L -> Left | St -> Up | R -> Right
    | Down -> match turn with | L -> Right | St -> Down | R -> Left
    | Left -> match turn with | L -> Down | St -> Left | R -> Right
    | Right -> match turn with | L -> Up | St -> Right | R -> Down

let moveCart (m : Tracks) (c : Cart) : Cart =
    let nextPos = nextPosition c.position c.direction
    let nextTrack = Map.find nextPos m
    let (nextDir, nextTurn) =
        match nextTrack with
        | S -> (sDirChange c.direction, c.nextTurn)
        | BS -> (bsDirChange c.direction, c.nextTurn)
        | I -> (iDirChange c.direction c.nextTurn, nextTurnDir c.nextTurn)
        | _ -> (c.direction, c.nextTurn)
    { position = nextPos; direction = nextDir; nextTurn = nextTurn }

let moveCartAndCheckCollision (m : Map<Position, Track>) (carts : Cart list) c : OptionalCollision =
    let newCart = moveCart m c
    let dupPos = carts |> List.filter (fun c -> c.position =  newCart.position)
    match dupPos.IsEmpty with
    | true -> Cart newCart
    | false -> Collision dupPos

let moveCarts (m : Map<Position, Track>) (carts : Cart list) : Cart list =
    let orderFun = posToOrder (mapWidth m)
    let moveFun = moveCart m
    carts |> List.sortBy (fun c -> orderFun c.position) |> List.map moveFun

let checkCollision (carts : Cart list) =
    let collided =
        carts
        |> List.groupBy (fun c -> c.position)
        |> List.filter (fun g -> snd g |> List.length > 1)
    match collided.IsEmpty with
    | true -> None
    | false -> Some (fst collided.[0])

let parseChar c = 
    match c with
    | ' ' -> (None, None)
    | '-' -> (Some H, None)
    | '|' -> (Some V, None)
    | '/' -> (Some S, None)
    | '\\' -> (Some BS, None)
    | '^' -> (Some V, Some Up)
    | 'v' -> (Some V, Some Down)
    | '<' -> (Some H, Some Left)
    | '>' -> (Some H, Some Right)
    | '+' -> (Some I, None)
    | _ -> invalidArg "c" "Invalid character"

let parseLine y line =
    let parsed =
        line
        |> Seq.mapi (fun x c ->
            let (mo, co) = parseChar c
            let p = (x, y)
            let mapKV =
                match mo with
                | None -> None
                | Some m -> Some (p, m)
            let cart =
                match co with
                | None -> None
                | Some cDir -> Some { position = p; direction = cDir; nextTurn = L }
            (mapKV, cart))
        |> List.ofSeq
    let mapElements =
        parsed
        |> Seq.map fst
        |> Seq.choose id
        |> List.ofSeq
    let carts =
        parsed
        |> Seq.map snd
        |> Seq.choose id
        |> List.ofSeq
    (mapElements, carts)

let parseInput (input : string seq) : Tracks * Cart list =
    let m =
        input
        |> Seq.mapi parseLine
        |> Seq.fold (fun (currElem, currCarts) (mapElem, carts) ->
            let newElems = List.append currElem mapElem
            let newCarts = List.append currCarts carts
            (newElems, newCarts)) (List.empty, List.empty)
    ((fst m) |> Map.ofList, snd m)

let readInput file = File.ReadAllLines file |> parseInput

let trackChar t = match t with | H -> '-' | V -> '|' | S -> '/' | BS -> '\\' | I -> '+'
let cartChar d = match d with | Up -> '^' | Down -> 'v' | Left -> '<' | Right -> '>'

let draw (m : Tracks) (carts : Cart list) : unit =
    Console.Clear()
    let posFun = posToOrder (mapWidth m)
    let oldColor = Console.ForegroundColor
    Console.ForegroundColor <- ConsoleColor.DarkGray
    m |> Seq.sortBy (fun kv -> posFun kv.Key) |> Seq.iter (fun kv ->
        let (x, y) = kv.Key
        Console.SetCursorPosition(x, y)
        Console.Write(trackChar kv.Value)
    )
    
    Console.ForegroundColor <- ConsoleColor.Cyan
    carts |> Seq.iter (fun c ->
        let (x, y) = c.position
        Console.SetCursorPosition(x, y)
        Console.Write(cartChar c.direction)
    )
    Console.ForegroundColor <- oldColor

type OptionalCollisions = Carts of Cart list | Collision of Cart list

let solve1 ((m, carts) : Map<Position, Track> * Cart list) =
    //let moveFun = moveCarts m
    let orderFun =  posToOrder (mapWidth m)
    let rec step cars =
        let ordered = cars |> List.sortBy orderFun
        let rec moveNextCart (newCarts, oldCarts) =
            match oldCarts with
            | [] -> newCarts
            | [c::cs] ->
                let nCart = moveCartAndCheckCollision (List.append newCarts cs) c
                match nCart with
                | Collision collisions -> Collisions collisions
                | Cart cart -> moveNextCart (List.append newCarts [cart], cs)
        moveNextCart ([], ordered)
    
        //draw m cars
        //Console.ReadLine() |> ignore
    step carts

[<EntryPoint>]
let main _ =
    let input = readInput "input.txt"

    let testInput1 = [
        @"/----\";
        @"|    |";
        @"|    |";
        @"\----/"
    ]

    let testMap = 
        Map.empty
            .Add((0, 0), S)
            .Add((1, 0), H)
            .Add((2, 0), H)
            .Add((3, 0), H)
            .Add((4, 0), H)
            .Add((5, 0), BS)
            .Add((0, 1), V)
            .Add((5, 1), V)
            .Add((0, 2), V)
            .Add((5, 2), V)
            .Add((0, 3), BS)
            .Add((1, 3), H)
            .Add((2, 3), H)
            .Add((3, 3), H)
            .Add((4, 3), H)
            .Add((5, 3), S)

    let testInput2 = [
        @"/-----\";
        @"|     |";
        @"|  /--+--\";
        @"|  |  |  |";
        @"\--+--/  |";
        @"   |     |";
        @"   \-----/";
    ]
    let testInput3 = [
        @"/->-\        ";
        @"|   |  /----\";
        @"| /-+--+-\  |";
        @"| | |  | v  |";
        @"\-+-/  \-+--/";
        @"  \------/   ";
    ]

    Common.test parseInput [(testInput1, (testMap, []))]

    //let parsed2 = parseInput testInput2;
    //printfn "%A" parsed2

    let testResult = solve1 (parseInput testInput3)
    printfn "Test result: %A" testResult

    let result1 = solve1 input
    printfn "Result 1: %A" result1

    Console.ReadLine() |> ignore
    0 // return an integer exit code
