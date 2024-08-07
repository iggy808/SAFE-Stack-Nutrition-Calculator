module Endpoints.User

open Records
open Commands

let getUser =
    Context.db.Users.FindAll()
    |> Seq.tryExactlyOne

let createUser (user:User) =
    Context.db.Users.Insert user
    |> ignore
    |> Ok

let updateUserWeight (command:UpdateUserWeightCommand) =
    Context.db.Users.FindAll()
    |> Seq.tryExactlyOne
    |> function
       | Some user ->
            { user with Weight = command.Weight }
            |> Context.db.Users.Update
            |> ignore
            |> Ok
       | None -> Error "No users exist within the database, or more than one user exist within the database."