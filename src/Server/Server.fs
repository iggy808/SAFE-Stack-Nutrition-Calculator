module Server

open SAFE
open Saturn
open Shared

let nutritionApi ctx = {
    // User Targets
    getUserTargetsByDate = fun query -> Endpoints.UserTargets.getUserTargetsByDate query
    createUserTargets = fun command -> Endpoints.UserTargets.createUserTargets command
    deleteUserTargetsByDate = fun command -> Endpoints.UserTargets.deleteUserTargetsByDate command

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