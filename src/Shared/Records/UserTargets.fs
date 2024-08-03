namespace Records
open System

[<CLIMutable>]
type UserTargets = {
    Id: Guid
    UserId:Guid
    Date: string // Date cannot be stored as DateOnly within LiteDB / Bson
    MaintenanceCalories: float
    ProteinGramsPerDay: float
    FatGramsPerDay: float
    CarbsGramsPerDay: float
}

module UserTargets =
    let isValid (date: DateOnly) =
        true