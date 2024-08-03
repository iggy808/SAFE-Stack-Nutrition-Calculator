module Endpoints.UserTargets

open Records
open System
open System.Linq
open Queries
open Commands

let createDailyUserTargets (command: CreateUserDailyTargetsCommand) = async {
    let user = Context.db.Users.FindOne(fun user -> user.Id = command.UserId)

    let proteinPerDay_grams = (Calculations.convertPoundsToKilograms user.Weight) * 1.2
    let proteinPerDay_calories = proteinPerDay_grams * 4.0
    let fatPerDay_grams = (Calculations.convertInchesToCentimeters user.Height) - 100.0
    let fatPerDay_calories = fatPerDay_grams * 9.0

    let basalMetabolicRate = Calculations.calculateBasalMetabolicRate user.Weight user.Height user.Age
    let maintenanceCalories = basalMetabolicRate * user.ActivityFactor
    let carbsPerDay_calories = maintenanceCalories - proteinPerDay_calories - fatPerDay_calories
    let carbsPerDay_grams = carbsPerDay_calories / 4.0

    let userDailyTargets = {
        Id = Guid.NewGuid()
        UserId = command.UserId
        Date = command.Date
        MaintenanceCalories = maintenanceCalories
        ProteinGramsPerDay = proteinPerDay_grams
        FatGramsPerDay = fatPerDay_grams
        CarbsGramsPerDay = carbsPerDay_grams
    }

    Context.db.UserTargets.Insert userDailyTargets |> ignore

    let q = Context.db.UserTargets.FindAll() |> List.ofSeq

    let z = 4

    ()
}

let hasExactlyOneUserTargets (dailyUserTargets:UserTargets list) =
    dailyUserTargets.Length = 1

let fetchUserDailyTargets (query:GetDailyUserTargetsQuery) =
    let dailyUserTargets = Context.db.UserTargets.Find(fun userTarget ->
        userTarget.UserId = query.UserId &&
        userTarget.Date = query.Date) |> List.ofSeq

    if hasExactlyOneUserTargets dailyUserTargets
    then Some dailyUserTargets.Head
    else None

let getDailyUserTargets (query:GetDailyUserTargetsQuery) = async {
        return
            fetchUserDailyTargets query
            |> function
               | Some userTargets -> Some userTargets
               | None -> None
}