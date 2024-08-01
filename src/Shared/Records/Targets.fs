namespace Records
open System

[<CLIMutable>]
type UserTargets = {
    Id: Guid;
    UserId:Guid;
    Date: DateOnly;
}

module UserTargets =
    let isValid (date: DateOnly) =
        true

    let create (date: DateOnly) = {
        Id = Guid.NewGuid()
        Date = date
    }