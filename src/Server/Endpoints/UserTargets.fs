module Endpoints.UserTargets

open Records
open System
open Queries
open Commands

let createDailyUserTargets (command: CreateUserDailyTargetsCommand) = async {
    Context.db.Users.FindOne(fun user -> user.Id = command.UserId)
    |> UserTargets.createDailyUserTargets
    |> Context.db.UserTargets.Insert
    |> ignore
}

let getDailyUserTargets (query:GetDailyUserTargetsQuery) = async {
        return
            Context.db.UserTargets.Find(fun userTargets ->
                userTargets.UserId = query.UserId &&
                userTargets.Date = (query.Date |> string))
            |> List.ofSeq
            |> List.tryExactlyOne
}