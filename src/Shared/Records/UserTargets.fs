namespace Records
open System

[<CLIMutable>]
type UserTargets = {
    Id: Guid
    UserId:Guid
    Date: DateOnly
    MaintenanceCalories: float
    ProteinGramsPerDay: float
    FatGramsPerDay: float
    CarbsGramsPerDay: float
}

module UserTargets =
    let isValid (date: DateOnly) =
        true