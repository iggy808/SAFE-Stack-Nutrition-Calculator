namespace Shared

open System
open Records
open Targets

[<CLIMutable>]
type Todo = { Id: Guid; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) = {
        Id = Guid.NewGuid()
        Description = description
    }


type INutritionApi = {
    getDailyTargets: unit -> Async<Targets list>
    createDailyTargets: Targets -> Async<Targets>
}