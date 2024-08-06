module Dashboard.State

open Records
open SAFE
open System
open Commands
open Queries

type Model = {
    User: RemoteData<User option>
    UserDto: User option
    Targets: RemoteData<UserTargets option>
    Input: string
}

type Msg =
    | SetInput of string
    | GetUser of ApiCall<unit, User option>
    | CreateUser of ApiCall<User, unit>
    | UpdateUserWeight of ApiCall<UpdateUserWeightCommand option, unit> // does this need to be option?
    | GetCurrentDayUserTargets of ApiCall<GetUserTargetsByDateQuery, Result<UserTargets option,string>>
    | CreateCurrentDayUserTargets of ApiCall<CreateUserTargetsCommand, Result<unit, string>>
    | DeleteCurrentDayUserTargets of ApiCall<DeleteUserTargetsByDateCommand, Result<unit, string>>