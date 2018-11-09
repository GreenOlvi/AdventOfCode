namespace Utils

open System

module KnotHash =

    type State = { position:int; skipSize:int; list:int list }

    let reverseFragment start length l =
        let L = Seq.length l
        if length > L then
            invalidArg "length" "Fragment length should be shorter than list"
        let boundStart = start % L
        let double = Seq.concat [|l; l|]
        let preFragment = double |> Seq.take boundStart
        let postFragment = double |> Seq.skip (boundStart + length)
        let fragment = double |> Seq.skip boundStart |> Seq.take length |> Seq.rev
        let newDouble = Seq.concat [|preFragment; fragment; postFragment|]
        let overflow =
            match (start + length > L) with
            | true -> start + length - L
            | false -> 0
        let newStart =
            match overflow with
            | 0 -> Seq.empty
            | o -> newDouble |> Seq.skip L |> Seq.take o
        let newEnd = newDouble |> Seq.skip overflow |> Seq.take (L - overflow)
        Seq.concat [newStart; newEnd] |> List.ofSeq

    let step (s:State) l :State =
        let newList = reverseFragment s.position l s.list
        {
            position = (s.position + l + s.skipSize) % (List.length s.list);
            skipSize = s.skipSize + 1;
            list = newList;
        }

    let round initialState input = input |> List.fold step initialState

    let rounds count initialState input =
        let rec r state i =
            match i with
            | 0 -> state
            | _ ->
                let s = round state input
                r s (i - 1)
        r initialState count

    let toAscii (string:String) =
        System.Text.ASCIIEncoding.ASCII.GetBytes(string)
        |> List.ofArray
        |> List.map Convert.ToInt32

    let xor values = List.fold (^^^) 0 values

    let bytesToHex (bytes:int list) =
        bytes
        |> List.map (fun (b:int) -> System.String.Format("{0:x2}", b))
        |> String.concat String.Empty

    let initialState = { position = 0; skipSize = 0; list = List.ofSeq {0..255}}

    let knotHash input =
        let newInput = List.concat [input |> toAscii; [17; 31; 73; 47; 23]]
        let state = rounds 64 initialState newInput
        let chunks = state.list |> List.chunkBySize 16
        chunks |> List.map xor |> bytesToHex
