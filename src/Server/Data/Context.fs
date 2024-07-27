module Context

open Records
open LiteDB.FSharp
open LiteDB
open Shared

type Context () =
    let database =
        let mapper = FSharpBsonMapper()
        let connectionString = "Filename=Nutrition.db"
        new LiteDatabase(connectionString, mapper)

    member val todos = database.GetCollection<Todo> "todos"
    member val DailyTargets = database.GetCollection<DailyTargets> "DailyTargets"

    (*
    member _.GetTodos () =
        todos.FindAll () |> List.ofSeq

    member _.AddTodo (todo: Todo) =
        todos.Insert todo |> ignore
        Ok()
    *)
