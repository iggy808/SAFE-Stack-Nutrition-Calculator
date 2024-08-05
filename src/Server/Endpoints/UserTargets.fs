module Endpoints.UserTargets

open Records
open Queries
open Commands
open System

// Todo: Add error handling
let createUserTargets (command: CreateUserTargetsCommand) = async {
    Context.db.Users.FindOne(fun user -> user.Id = command.UserId)
    |> UserTargets.createDailyUserTargets
    |> Context.db.UserTargets.Insert
    |> ignore
}

let getUserTargetsByDate (query:GetUserTargetsByDateQuery) = async {
        return
            Context.db.UserTargets.Find(fun userTargets ->
                userTargets.UserId = query.UserId &&
                userTargets.Date = (query.Date |> string))
            |> List.ofSeq
            |> List.tryExactlyOne
}

let deleteUserTargetsByDate (command: DeleteUserTargetsByDateCommand) = async {
    Context.db.UserTargets.Delete (fun userTargets ->
        userTargets.UserId = command.UserId &&
        userTargets.Date = (command.Date |> string))
    |> ignore
}