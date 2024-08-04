module Dashboard.Handler

open Dashboard.State

open Elmish
open SAFE
open Shared
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

// Todo: Refactor daily target creation to be UI driven to prevent the spaghetti code below
//       Ex/
//          1. Add button to User Daily Targets widget that will generate targets
//          2. Upon clickin the button, the user will be prompted with the update weight modal
//              - Users may either update weight, or skip the update and use the weight currently in the user record
let update msg model =
    match msg with

    | GetUser msg ->
        match msg with
        | Start() ->
            { model with User = Loading },
            Cmd.OfAsync.perform
                nutritionApi.getUser ()
                (Finished >> GetUser)

        | Finished user ->
            match user with
            | Some user ->
                { model with User = Loaded (Some user) },
                GetUserTargets(Start(Some user.Id)) |> Cmd.ofMsg

            | None ->
                { model with User = Loaded (user) },
                Cmd.none

    // Todo: Remove this when refactoring (details in Todo above update method)
    | GetUpdatedUser msg ->
        match msg with
        | Start() ->
            { model with User = Loading },
            Cmd.OfAsync.perform
                nutritionApi.getUser ()
                (Finished >> GetUpdatedUser)

        | Finished user ->
            { model with User = Loaded (user) },
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
                        UserId = userId
                        Date = DateOnly.FromDateTime(DateTime.Now)
                    }
                    (Finished >> GetUserTargets)

            | None -> { model with Targets = NotStarted }, Cmd.none

        | Finished targets ->
            match targets with
            | Some targets ->
                { model with Targets = Loaded (Some targets) },
                Cmd.none
            | None ->
                { model with Targets = Loaded (None) },
                Cmd.none

    | CreateUserTargets msg ->
        match msg with
        | Start userId ->
            match userId with
            | Some userId ->
                { model with Targets = Loading },
                Cmd.OfAsync.perform
                    nutritionApi.createDailyUserTargets { UserId = userId; Date = DateOnly.FromDateTime(DateTime.Now) }
                    (Finished >> CreateUserTargets)
            | None ->
                model,
                Cmd.none

        | Finished _ ->
            match model.User with
            | NotStarted -> { model with Targets = NotStarted }, Cmd.none
            | Loading -> { model with Targets = NotStarted }, Cmd.none
            | Loaded user ->
                match user with
                | Some user ->
                    { model with Targets = Loading },
                    (GetUserTargets(Start(Some user.Id))) |> Cmd.ofMsg
                | None -> { model with Targets = NotStarted }, Cmd.none

    | UpdateUserWeight msg ->
        match msg with
        | Start command ->
            match command with
            | Some command ->
                model,
                Cmd.OfAsync.perform
                    nutritionApi.updateUserWeight command
                    (Finished >> UpdateUserWeight)

            | None ->
                model,
                Cmd.none

        | Finished _ ->
            match model.User with
            | NotStarted -> model, Cmd.none
            | Loading -> model, Cmd.none
            | Loaded user ->
                match user with
                | None -> model, Cmd.none
                | Some user ->
                    { model with Targets = Loading },
                    (CreateUserTargets(Start(Some user.Id))) |> Cmd.ofMsg