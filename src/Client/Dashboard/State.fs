module Dashboard.State

open Records
open SAFE
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
    | CreateUser of ApiCall<User, Result<unit, string>>
    | UpdateUserWeight of ApiCall<UpdateUserWeightCommand option, Result<unit, string>>
    | GetCurrentDayUserTargets of ApiCall<GetUserTargetsByDateQuery, Result<UserTargets option,string>>
    | CreateCurrentDayUserTargets of ApiCall<CreateUserTargetsCommand, Result<unit, string>>
    | DeleteCurrentDayUserTargets of ApiCall<DeleteUserTargetsByDateCommand, Result<unit, string>>