namespace Commands

open System

type UpdateUserWeightCommand = {
    UserId : Guid
    Weight : float
}