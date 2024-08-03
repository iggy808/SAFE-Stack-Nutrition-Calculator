module Dashboard.Handler

open Dashboard.State

open Elmish
open SAFE
open Shared
open Shared.Queries
open System
open Records

let nutritionApi = Api.makeProxy<INutritionApi> ()

let init () =
    let initialModel = {
        User = NotStarted;
        UserDto = None
        Targets = NotStarted;
        Input = "" }
    let initialCmd = GetUser(Start()) |> Cmd.ofMsg

    initialModel, initialCmd

let update msg model =
    match msg with

    | GetUser msg ->
        match msg with
        | Start() ->
            { model with User = Loading },
            Cmd.OfAsync.perform
                nutritionApi.getUser ()
                (Finished >> GetUser)

        | Finished userInformation ->
            match userInformation with
            | Some user ->
                { model with User = Loaded (userInformation) },
                GetUserTargets(Start(Some user.Id)) |> Cmd.ofMsg

            | None ->
                { model with User = Loaded (userInformation) },
                Cmd.none

    | CreateUser msg ->
        match msg with
        | Start user ->
            model,
            Cmd.OfAsync.perform
                nutritionApi.createUser user
                (Finished >> CreateUser)

        | Finished _ ->
            { model with User = Loading },
            Cmd.OfAsync.perform
                nutritionApi.getUser ()
                (Finished >> GetUser)

    | SetInput value -> { model with Input = value }, Cmd.none

    | GetUserTargets msg ->
        match msg with
        | Start userId ->

            match userId with
            | Some userId ->
                { model with Targets = Loading },
                Cmd.OfAsync.perform
                    nutritionApi.getDailyUserTargets {
                        UserId = userId;
                        Date = DateOnly.FromDateTime(DateTime.Now);
                    }
                    (Finished >> GetUserTargets)

            | None -> { model with Targets = NotStarted }, Cmd.none

        | Finished targets -> { model with Targets = Loaded targets }, Cmd.none