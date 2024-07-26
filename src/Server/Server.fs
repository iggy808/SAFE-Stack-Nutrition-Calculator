module Server

open SAFE
open Saturn
open Shared
open LiteDB
open LiteDB.FSharp

type Storage () =
    let database =
        let mapper = FSharpBsonMapper()
        let connectionString = "Filename=Nutrition.db"
        new LiteDatabase(connectionString, mapper)

    let todos = database.GetCollection<Todo> "todos"

    member _.GetTodos () =
        todos.FindAll () |> List.ofSeq

    member _.AddTodo (todo: Todo) =
        todos.Insert todo |> ignore
        Ok()


let context = new Storage()
let todosApi ctx = {
    getTodos = fun () -> async { return context.GetTodos () }
    addTodo =
        fun todo -> async {
            return
                match context.AddTodo todo with
                | Ok() -> todo
                | Error e -> failwith e
        }
}

let webApp = Api.make todosApi

let app = application {
    use_router webApp
    memory_cache
    use_static "public"
    use_gzip
}

[<EntryPoint>]
let main _ =
    run app
    0