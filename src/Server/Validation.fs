module Validation

open System
open Commands
open Queries
open Records

let validateGetUserTargetsByDateQuery (query:GetUserTargetsByDateQuery) =
    query.UserId <> Guid.Empty

let validateCreateUserTargetsCommand (command:CreateUserTargetsCommand) =
    command.UserId <> Guid.Empty

let validateDeleteUserTargetsByDateCommand (command:DeleteUserTargetsByDateCommand) =
    command.UserId <> Guid.Empty

let validateCreateUserCommand (command:User) =
    User.isValid command

let validateUpdateUserWeightCommand (command:UpdateUserWeightCommand) =
    command.UserId <> Guid.Empty &&
    User.isWeightWithinValidRange command.Weight

let validateGetUserWeightHistoryQuery (query:GetUserWeightHistoryQuery) =
    query.UserId <> Guid.Empty