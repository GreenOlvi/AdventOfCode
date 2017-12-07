namespace Utils

open System

module Common =

    let wrapColor color printAction =
        let old = System.Console.ForegroundColor
        System.Console.ForegroundColor <- color
        printAction()
        System.Console.ForegroundColor <- old

    let test f data =
        let testResults =
            data
            |> List.map (fun (input, expected) -> 
                let result = input |> f
                printf "input: %A, result: %A, " input result
                if (result = expected) then
                    wrapColor ConsoleColor.Green (fun () -> printfn "OK")
                else
                    wrapColor ConsoleColor.Red (fun () -> printfn "expected: %A" expected)
                result = expected
            )
        let successful = (testResults |> List.filter (fun i -> i) |> List.length)
        let color = match successful = data.Length with | true -> ConsoleColor.Green | false -> ConsoleColor.Red
        wrapColor color (fun () -> printfn "Success %A/%A\n" successful data.Length)

    let splitString (chars:char[]) (input:string) = input.Split(chars, StringSplitOptions.RemoveEmptyEntries)

    let timeIt action =
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        let result = action()
        stopwatch.Stop()
        printfn "%f seconds\n" stopwatch.Elapsed.TotalSeconds
        result
