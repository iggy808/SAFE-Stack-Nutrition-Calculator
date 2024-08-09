namespace Records
open System

[<CLIMutable>]
type UserTargets = {
    Id: Guid
    UserId:Guid
    Date: string // Date cannot be stored as DateOnly within LiteDB / Bson
    CurrentWeight: float
    MaintenanceCalories: float
    ProteinGramsPerDay: float
    FatGramsPerDay: float
    CarbsGramsPerDay: float
}

module UserTargets =
    let isValid (date: DateOnly) =
        true

    let createDailyUserTargets user =
        let proteinPerDay_grams = (Calculations.convertPoundsToKilograms user.Weight) * 1.2 |> round
        let proteinPerDay_calories = proteinPerDay_grams * 4.0
        let fatPerDay_grams = (Calculations.convertInchesToCentimeters user.Height) - 100.0 |> round
        let fatPerDay_calories = fatPerDay_grams * 9.0

        let basalMetabolicRate = Calculations.calculateBasalMetabolicRate user.Weight user.Height user.Age
        let maintenanceCalories = basalMetabolicRate * user.ActivityFactor |> round
        let carbsPerDay_calories = maintenanceCalories - proteinPerDay_calories - fatPerDay_calories
        let carbsPerDay_grams = carbsPerDay_calories / 4.0 |> round

        {
            Id = Guid.NewGuid()
            UserId = user.Id
            Date = DateOnly.FromDateTime(DateTime.Now) |> string
            CurrentWeight = user.Weight
            MaintenanceCalories = maintenanceCalories
            ProteinGramsPerDay = proteinPerDay_grams
            FatGramsPerDay = fatPerDay_grams
            CarbsGramsPerDay = carbsPerDay_grams
        }