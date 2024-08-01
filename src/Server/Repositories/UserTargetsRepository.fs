module UserTargetsRepository
open System
open System.Linq
open Shared.Queries
open Records
open Calculations


let CreateDailyTargets userId =
    let user = Context.db.Users.FindOne(fun user -> user.Id = userId)

    let basalMetabolicRate = Calculations.calculateBasalMetabolicRate user.Weight user.Height user.Age
    let maintenanceCalories = basalMetabolicRate * user.ActivityFactor
    let proteinPerDay_grams = (Calculations.convertPoundsToKilograms user.Weight) * 1.2
    let proteinPerDay_calories = proteinPerDay_grams * 4.0
    let fatPerDay_grams = (Calculations.convertInchesToCentimeters user.Height) - 100.0
    let fatPerDay_calories = fatPerDay_grams * 9.0
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

let GetDailyUserTargets (query:GetDailyUserTargetsQuery) =
        let dailyTargets = Context.db.UserTargets.Find(fun userTarget ->
                userTarget.UserId = query.UserId &&
                userTarget.Date = DateOnly.FromDateTime(DateTime.Now)) |> List.ofSeq

        // Unsure how to perform a null check / option match on the value returned from FindOne - will need to research
        // For now, hacky list check will do
        if dailyTargets.Any()
        then dailyTargets.Head
        else CreateDailyTargets query.UserId