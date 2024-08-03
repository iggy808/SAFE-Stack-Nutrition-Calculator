module Dashboard.State

open Records
open SAFE
open System

type Model = {
    User: RemoteData<User option>
    UserDto: User option
    Targets: RemoteData<UserTargets>
    Input: string
}

type Msg =
    | SetInput of string
    | GetUser of ApiCall<unit, User option>
    | CreateUser of ApiCall<User, unit>
    | GetUserTargets of ApiCall<Guid option, UserTargets>