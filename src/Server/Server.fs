module Server

open SAFE
open Saturn
open Shared
open Context
open TargetsRepository
open UserRepository

let context = new Context()
let dailyTargetsRepository = new TargetsRepository(context)
let userRepository = new UserRepository(context)

let nutritionApi ctx = {

    // Daily Targets
    getDailyTargets = fun _ -> async { return dailyTargetsRepository.GetAllDailyTargets () }
    createDailyTargets = fun dailyTargets -> async {
        return
            match dailyTargetsRepository.CreateDailyTargets dailyTargets with
                | Ok() -> dailyTargets
                | Error e -> failwith e
    }

    // User Information
    getUser = fun _ -> async { return userRepository.GetUser () }
    createUser = fun user -> async {
        return
            match userRepository.CreateUser (user) with
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