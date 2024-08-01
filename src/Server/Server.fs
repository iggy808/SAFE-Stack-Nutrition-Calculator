module Server

open SAFE
open Saturn
open Shared

let nutritionApi ctx = {

    // Daily Targets
    getTargets = fun _ -> async { return UserTargetsRepository.GetDailyUserTargets () }


    // User Information
    getUser = fun _ -> async { return UserRepository.GetUser () }
    createUser = fun user -> async {
        return
            match UserRepository.CreateUser (user) with
            | Ok() -> ()
            | Error e -> failwith e
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