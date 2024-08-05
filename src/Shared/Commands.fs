namespace Commands

open System

type UpdateUserWeightCommand = {
    UserId : Guid
    Weight : float
}

type CreateUserTargetsCommand = {
    UserId : Guid
    Date : DateOnly
}

type DeleteUserTargetsByDateCommand = {
    UserId : Guid
    Date : DateOnly
}