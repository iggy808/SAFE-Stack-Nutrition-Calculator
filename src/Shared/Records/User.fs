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
    let isNameValid name = String.IsNullOrWhiteSpace name |> not
    let isAgeWithinValidRange age = age > 0 && age < 120
    let isHeightWithinValidRange height = height > 0 && height < 144
    let isWeightWithinValidRange weight = weight > 0.0 && weight < 1000.0
    let isActivityFactorWithinValidRange activityFactor = activityFactor > 0.0 && activityFactor < 3.0

    let isValid (user:User) =
        user.Id <> Guid.Empty &&
        user.Name |> isNameValid &&
        user.Age |> isAgeWithinValidRange &&
        user.Height |> isHeightWithinValidRange &&
        user.Weight |> isWeightWithinValidRange &&
        user.ActivityFactor |> isActivityFactorWithinValidRange

    let Default = {
        Id = Guid.Empty;
        Name = "";
        Age =  -1;
        Height = -1;
        Weight = -1;
        ActivityFactor = -1;
    }