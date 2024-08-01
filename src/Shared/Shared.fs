namespace Shared

open System
open Records
open Shared.Queries

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
    getDailyUserTargets: GetDailyUserTargetsQuery -> Async<UserTargets>
    getUser: unit -> Async<User option>
    createUser: User -> Async<unit>
}