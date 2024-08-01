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
    member val UserTargets = database.GetCollection<UserTargets> "UserTargets"
    member val Users = database.GetCollection<User> "Users"

let db = Context()