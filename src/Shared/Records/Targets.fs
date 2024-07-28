namespace Records
open System

[<CLIMutable>]
type Targets = {
    Id: Guid;
    Date: DateOnly;
}

module Targets =
    let isValid (date: DateOnly) =
        true

    let create (date: DateOnly) = {
        Id = Guid.NewGuid()
        Date = date
    }