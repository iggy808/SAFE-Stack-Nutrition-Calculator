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
                (GetCurrentDayUserTargets(Start({
                    UserId = user.Id;
                    Date = DateOnly.FromDateTime(DateTime.Now)
                })))
                |> Cmd.ofMsg

            | None ->
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

    | GetCurrentDayUserTargets msg ->
        match msg with
        | Start query ->
            { model with Targets = Loading },
            Cmd.OfAsync.perform
                nutritionApi.getUserTargetsByDate query
                (Finished >> GetCurrentDayUserTargets)

        | Finished result ->
            match result with
            | Ok targets ->
                match targets with
                | Some targets ->
                    { model with Targets = Loaded (Some targets) },
                    Cmd.none
                | None ->
                    // If no targets exist for the current day, create them
                    let userId =
                        match model.User with
                        | NotStarted -> None
                        | Loading -> None
                        | Loaded user ->
                            match user with
                            | Some user -> Some user.Id
                            | None -> None

                    let command =
                        match userId with
                        | Some userId ->
                            (CreateCurrentDayUserTargets(Start({
                                UserId = userId;
                                Date = DateOnly.FromDateTime(DateTime.Now)
                            })))
                            |> Cmd.ofMsg
                        | None -> Cmd.none

                    { model with Targets = Loaded (None) },
                    command

            // Todo: Update program flow after error encountered here.
            | Error message ->
                // If there was an server error while getting user targets, log it.
                Browser.Dom.console.log message
                { model with Targets = Loaded (None) },
                Cmd.none

    | CreateCurrentDayUserTargets msg ->
        match msg with
        | Start command ->
            { model with Targets = NotStarted },
            Cmd.OfAsync.perform
                nutritionApi.createUserTargets command
                (Finished >> CreateCurrentDayUserTargets)

        | Finished result ->
            match result with
            | Ok () ->
                match model.User with
                | NotStarted -> { model with Targets = NotStarted }, Cmd.none
                | Loading -> { model with Targets = NotStarted }, Cmd.none
                | Loaded user ->
                    match user with
                    | Some user ->
                        { model with Targets = NotStarted },
                        (GetCurrentDayUserTargets(Start({
                            UserId = user.Id;
                            Date = DateOnly.FromDateTime(DateTime.Now)
                        })))
                        |> Cmd.ofMsg
                    | None -> { model with Targets = NotStarted }, Cmd.none

            // Todo: Update program flow after error encountered here.
            | Error message ->
                Browser.Dom.console.log message
                model,
                Cmd.none

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

        | Finished result ->
            match result with
            | Ok () ->
                match model.User with
                | NotStarted -> model, Cmd.none
                | Loading -> model, Cmd.none
                | Loaded user ->
                    match user with
                    | None -> model, Cmd.none
                    | Some user ->
                        { model with Targets = NotStarted },
                        (DeleteCurrentDayUserTargets(Start({
                            UserId = user.Id
                            Date = DateOnly.FromDateTime(DateTime.Now)
                        }))) |> Cmd.ofMsg

            // Todo: Update program flow after error encountered here.
            | Error message ->
                Browser.Dom.console.log message
                model,
                Cmd.none

    // Called when updating user weight
    | DeleteCurrentDayUserTargets msg ->
        match msg with
        | Start command ->
            { model with Targets = NotStarted },
            Cmd.OfAsync.perform
                nutritionApi.deleteUserTargetsByDate command
                (Finished >> DeleteCurrentDayUserTargets)

        // Getting user serves two purposes:
        //  1. To retrieve updated user data and post on User Information Widget
        //  2. To trigger a fetch and subsequent creation of new user target data with the updated weight
        | Finished result ->
            // If there was an server error while deleting the user targets, log it.
            match result with
            | Error message -> Browser.Dom.console.log message
            | Ok _ -> ()

            { model with Targets = NotStarted },
            (GetUser(Start())) |> Cmd.ofMsg
                