namespace Records
open System

[<CLIMutable>]
type User = {
    Id: Guid;
    Name: string;
    Age: int;
    Height: int;
    Weight: float;
    ActivityFactor: float;
}

module User =
    let isValid (name:string) =
        true