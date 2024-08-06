module Validation

open System
open Commands
open Queries

let validateGetUserTargetsByDateQuery (query:GetUserTargetsByDateQuery) =
    query.UserId <> Guid.Empty

let validateCreateUserTargetsCommand (command:CreateUserTargetsCommand) =
    command.UserId <> Guid.Empty

let validateDeleteUserTargetsByDateCommand (command:DeleteUserTargetsByDateCommand) =
    command.UserId <> Guid.Empty