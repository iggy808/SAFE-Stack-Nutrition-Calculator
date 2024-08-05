namespace Shared

open System
open Records
open Queries
open Commands

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
    getUser: unit -> Async<User option>
    createUser: User -> Async<unit>
    updateUserWeight: UpdateUserWeightCommand -> Async<unit>
    getUserTargetsByDate: GetUserTargetsByDateQuery -> Async<UserTargets option>
    createUserTargets: CreateUserTargetsCommand -> Async<unit>
    deleteUserTargetsByDate: DeleteUserTargetsByDateCommand -> Async<unit>
}