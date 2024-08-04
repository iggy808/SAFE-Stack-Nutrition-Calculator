module Dashboard.State

open Records
open SAFE
open System
open Commands

type Model = {
    User: RemoteData<User option>
    UserDto: User option
    Targets: RemoteData<UserTargets option>
    Input: string
}

type Msg =
    | SetInput of string
    | GetUser of ApiCall<unit, User option>
    | GetUpdatedUser of ApiCall<unit, User option>
    | CreateUser of ApiCall<User, unit>
    | GetUserTargets of ApiCall<Guid option, UserTargets option>
    | CreateUserTargets of ApiCall<Guid option, unit>
    | UpdateUserWeight of ApiCall<UpdateUserWeightCommand option, unit>