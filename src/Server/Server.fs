module Server

open SAFE
open Saturn
open Shared
open System

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
    getUser = fun _ -> async {
        return
            Endpoints.User.getUser
    }
    createUser = fun command -> async {
        return
            Validation.validateCreateUserCommand command
            |> function
               | true -> Endpoints.User.createUser command
               | false -> Error "User data is invalid."
    }
    updateUserWeight = fun command -> async {
        return
            Validation.validateUpdateUserWeightCommand command
            |> function
               | true -> Endpoints.User.updateUserWeight command
               | false -> Error "Update user weight command is invalid."
    }

    getUserWeightHistory = fun query -> async {
        return
            Validation.validateGetUserWeightHistoryQuery query
            |> function
               | true -> Endpoints.UserTargets.getUserWeightHistory query
               | false -> Error "Get user weight history query is invalid."
    }
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