namespace Utils

module Common =
    open System
    open System.Text.RegularExpressions

    type timedBuilder(id) =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        let logTime = printfn "Took %f seconds" stopwatch.Elapsed.TotalSeconds
        member this.Bind(x, f) =
            let r = f x
            logTime
            r
        member this.Return(x) =
            match id with
            | Some i -> printfn "Result %d: %A\n" i x
            | None -> printfn ""
            logTime
            x

    let timeResult id = new timedBuilder(Some id)
    let timeIt = new timedBuilder(None)

    let wrapColor color printAction =
        let old = System.Console.ForegroundColor
        System.Console.ForegroundColor <- color
        printAction()
        System.Console.ForegroundColor <- old

    let testN findResultFun data =
        let testResults =
            data
            |> List.map (fun (input, expected) -> 
                let result = findResultFun input
                printf "input: %A, result: %A, " input result
                if (result = expected) then
                    wrapColor ConsoleColor.Green (fun () -> printfn "OK")
                else
                    wrapColor ConsoleColor.Red (fun () -> printfn "expected: %A" expected)
                result = expected
            )
        let successful = (testResults |> List.filter id |> List.length)
        let color = match successful = data.Length with | true -> ConsoleColor.Green | false -> ConsoleColor.Red
        wrapColor color (fun () -> printfn "Success %A/%A\n" successful data.Length)

    let test f data = testN (fun a -> f a) data

    let splitString (chars:char seq) (input:string) =
        input.Split(chars |> Array.ofSeq, StringSplitOptions.RemoveEmptyEntries)

    let cartesian xs ys = ys |> Seq.collect (fun y -> xs |> Seq.map (fun x -> (x, y)))
    let buildGrid width height = seq {0..height-1} |> cartesian (seq {0..width-1})

    let (|FirstRegexGroup|_|) pattern input =
        let m = Regex.Match(input,pattern) 
        match (m.Success) with
        | true -> Some m.Groups.[1].Value
        | false -> None  

    let (|RegexMatch|_|) pattern input =
        let m = Regex.Match(input,pattern) 
        match (m.Success) with
        | true -> Some ()
        | false -> None  

    let (|StartsWith|_|) (pattern:string) (input:string)  =
        match input.StartsWith(pattern) with
        | true -> Some ()
        | false -> None
