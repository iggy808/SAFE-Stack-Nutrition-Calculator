module Server

open SAFE
open Saturn
open Shared
open Context
open TargetsRepository
open UserRepository

let nutritionApi ctx = {

    // Daily Targets
    getDailyTargets = fun _ -> async { return TargetsRepository.GetAllDailyTargets () }
    createDailyTargets = fun dailyTargets -> async {
        return
            match TargetsRepository.CreateDailyTargets dailyTargets with
                | Ok() -> dailyTargets
                | Error e -> failwith e
    }

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