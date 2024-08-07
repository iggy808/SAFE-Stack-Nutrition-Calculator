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
    createUser: User -> Async<Result<unit, string>>
    updateUserWeight: UpdateUserWeightCommand -> Async<Result<unit, string>>
    getUserTargetsByDate: GetUserTargetsByDateQuery -> Async<Result<UserTargets option, string>>
    createUserTargets: CreateUserTargetsCommand -> Async<Result<unit, string>>
    deleteUserTargetsByDate: DeleteUserTargetsByDateCommand -> Async<Result<unit,string>>
}