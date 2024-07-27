namespace Records
open System

[<CLIMutable>]
type DailyTargets = { Id: Guid; Date: DateTime; }

module DailyTargets =
    let isValid (date: DateTime) =
        true

    let create (date: DateTime) = {
        Id = Guid.NewGuid()
        Date = date
    }