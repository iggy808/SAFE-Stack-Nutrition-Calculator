module Endpoints.UserTargets

open Records
open Shared.Queries
open System

let createDailyTargets userId =
    let user = Context.db.Users.FindOne(fun user -> user.Id = userId)

    let proteinPerDay_grams = (Calculations.convertPoundsToKilograms user.Weight) * 1.2
    let proteinPerDay_calories = proteinPerDay_grams * 4.0
    let fatPerDay_grams = (Calculations.convertInchesToCentimeters user.Height) - 100.0
    let fatPerDay_calories = fatPerDay_grams * 9.0

    let basalMetabolicRate = Calculations.calculateBasalMetabolicRate user.Weight user.Height user.Age
    let maintenanceCalories = basalMetabolicRate * user.ActivityFactor
    let carbsPerDay_calories = maintenanceCalories - proteinPerDay_calories - fatPerDay_calories
    let carbsPerDay_grams = carbsPerDay_calories / 4.0

    let userDailyTargets = {
        Id = Guid.NewGuid();
        UserId = userId;
        Date = DateOnly.FromDateTime(DateTime.Now)
        MaintenanceCalories = maintenanceCalories
        ProteinGramsPerDay = proteinPerDay_grams
        FatGramsPerDay = fatPerDay_grams
        CarbsGramsPerDay = carbsPerDay_grams
    }

    Context.db.UserTargets.Insert userDailyTargets |> ignore

    userDailyTargets

let fetchUserDailyTargets userId =
    Context.db.UserTargets.Find(fun userTarget ->
        userTarget.UserId = userId &&
        userTarget.Date = DateOnly.FromDateTime(DateTime.Now))
    |> List.ofSeq
    |> List.tryExactlyOne

let getDailyUserTargets (query:GetDailyUserTargetsQuery) = async {
        return
            fetchUserDailyTargets query.UserId
            |> function
                | Some userTargets -> userTargets
                | None -> createDailyTargets query.UserId
}