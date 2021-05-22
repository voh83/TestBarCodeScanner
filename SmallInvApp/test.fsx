module test


let printTask str =
    async {
        printfn "%s" str
        } |> Async.StartAsTask

printTask "hola"