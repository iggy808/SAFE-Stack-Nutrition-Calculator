module Server

open SAFE
open Saturn
open Shared

let nutritionApi ctx = {
    // User Targets
    getUserTargetsByDate = fun query -> async {
        return Validation.validateGetUserTargetsByDateQuery query
        |> function
           | true -> Endpoints.UserTargets.getUserTargetsByDate query
           | false -> Error "Get user targets by date query is invalid."
    }
    createUserTargets = fun command -> async {
        return
            Validation.validateCreateUserTargetsCommand command
            |> function
               | true -> Endpoints.UserTargets.createUserTargets command
               | false -> Error "Create user targets command is invalid."
    }
    deleteUserTargetsByDate = fun command -> async {
            return
                Validation.validateDeleteUserTargetsByDateCommand command
                |> function
                   | true -> Endpoints.UserTargets.deleteUserTargetsByDate command
                   | false -> Error "Delete user targets command is invalid."
    }

    // User
    getUser = fun _ -> Endpoints.User.getUser
    createUser = fun user -> Endpoints.User.createUser user
    updateUserWeight = fun command -> Endpoints.User.updateUserWeight command
}

let webApp = Api.make nutritionApi

let app = application {
    use_router webApp
    memory_cache
    use_static "public"
    use_gzip
}

[<EntryPoint>]
let main _ =
    run app
    0