namespace Commands

open System

type UpdateUserWeightCommand = {
    UserId : Guid
    Weight : float
}

type CreateUserDailyTargetsCommand = {
    UserId : Guid
    Date: DateOnly
}