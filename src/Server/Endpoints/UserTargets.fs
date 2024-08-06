module Endpoints.UserTargets

open Records
open Queries
open Commands
open System

// Todo: Add error handling
let createUserTargets (command: CreateUserTargetsCommand) =
    Context.db.Users.FindOne(fun user -> user.Id = command.UserId)
    |> UserTargets.createDailyUserTargets
    |> Context.db.UserTargets.Insert
    |> ignore
    |> Ok

let getUserTargetsByDate (query:GetUserTargetsByDateQuery) =
    Context.db.UserTargets.Find(fun userTargets ->
        userTargets.UserId = query.UserId &&
        userTargets.Date = (query.Date |> string))
    |> Seq.tryExactlyOne
    |> Ok

let private verifyUserTargetsDeleted recordsDeleted =
    match recordsDeleted with
    | 1 -> Ok ()
    | 0 -> Error "No records deleted"
    | _ -> Error "Unexpected number of records deleted. This is a big problem."

let deleteUserTargetsByDate (command: DeleteUserTargetsByDateCommand) =
    Context.db.UserTargets.Delete (fun userTargets ->
        userTargets.UserId = command.UserId &&
        userTargets.Date = (command.Date |> string))
    |> verifyUserTargetsDeleted