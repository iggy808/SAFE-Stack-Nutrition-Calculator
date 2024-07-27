namespace Records
open System

[<CLIMutable>]
type DailyTargets = { Id: Guid; Date: DateOnly; }

module DailyTargets =
    let isValid (date: DateOnly) =
        true

    let create (date: DateOnly) = {
        Id = Guid.NewGuid()
        Date = date
    }