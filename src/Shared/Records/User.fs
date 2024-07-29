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
with static member Default = {
        Id = Guid.Empty;
        Name = "";
        Age =  -1;
        Height = -1;
        Weight = -1;
        ActivityFactor = -1;
    }


module User =
    let isValid (name:string) =
        true