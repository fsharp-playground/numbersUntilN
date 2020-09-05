open System
open Argu

type CliError =
    | ArgumentsNotSpecified

type Number =
    | [<AltCommandLine("-n")>] Number of number:int
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Number _ -> "target Number"

let getExitCode result =
    match result with
    | Ok () -> 0
    | Error err ->
        match err with
        | ArgumentsNotSpecified -> 1

let printCounting number = 
   for i = 1 to number do
      printfn " %i" i
   Ok ()


[<EntryPoint>]
let main argv = 
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<Number>(programName = "FSharpCliApp", errorHandler = errorHandler)

    let results = parser.ParseCommandLine argv
    printfn "Got parse results %A" <| results.GetAllResults()

    match parser.ParseCommandLine argv with
    | p when p.Contains(Number) -> printCounting (p.GetResult(Number))
    | _ ->
        printfn "%s" (parser.PrintUsage())
        Error ArgumentsNotSpecified
    |> getExitCode



