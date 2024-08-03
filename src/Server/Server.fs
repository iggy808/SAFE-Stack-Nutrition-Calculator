module Server

open SAFE
open Saturn
open Shared

let nutritionApi ctx = {
    // User Targets
    getDailyUserTargets = fun query -> Endpoints.UserTargets.getDailyUserTargets query
    createDailyUserTargets = fun command -> Endpoints.UserTargets.createDailyUserTargets command

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