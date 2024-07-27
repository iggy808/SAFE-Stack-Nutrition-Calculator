module Index

open Elmish
open SAFE
open Shared
open System
open Records

type Model = {
    DailyTargets: RemoteData<DailyTargets list>
    Input: string
}

type Msg =
    | SetInput of string
    | LoadDailyTargets of ApiCall<unit, DailyTargets list>
    | SaveTodo of ApiCall<string, DailyTargets>

let nutritionApi = Api.makeProxy<INutritionApi> ()

let init () =
    let initialModel = { DailyTargets = NotStarted; Input = "" }
    let initialCmd = LoadDailyTargets(Start()) |> Cmd.ofMsg

    initialModel, initialCmd

let update msg model =
    match msg with
    | SetInput value -> { model with Input = value }, Cmd.none
    | LoadDailyTargets msg ->
        match msg with
        | Start() ->
            let loadDailyTargets_command = Cmd.OfAsync.perform nutritionApi.getDailyTargets () (Finished >> LoadDailyTargets)

            { model with DailyTargets = Loading }, loadDailyTargets_command
        | Finished dailyTargets -> { model with DailyTargets = Loaded dailyTargets }, Cmd.none
    | SaveTodo msg ->
        match msg with
        | Start todoText ->
            let saveTodoCmd =
                let todo = Todo.create todoText
                Cmd.OfAsync.perform nutritionApi.createDailyTargets (DailyTargets.create DateTime.Now) (Finished >> SaveTodo)

            { model with Input = "" }, saveTodoCmd
        | Finished DailyTargets ->
            {
                model with
                    DailyTargets = model.DailyTargets |> RemoteData.map (fun dailyTargets -> dailyTargets @ [ DailyTargets ])
            },
            Cmd.none

open Feliz

module ViewComponents =
    let todoAction model dispatch =
        Html.div [
            prop.className "flex flex-col sm:flex-row mt-4 gap-4"
            prop.children [
                Html.input [
                    prop.className
                        "shadow appearance-none border rounded w-full py-2 px-3 outline-none focus:ring-2 ring-teal-300 text-grey-darker"
                    prop.value model.Input
                    prop.placeholder "What needs to be done?"
                    prop.autoFocus true
                    prop.onChange (SetInput >> dispatch)
                    prop.onKeyPress (fun ev ->
                        if ev.key = "Enter" then
                            dispatch (SaveTodo(Start model.Input)))
                ]
                Html.button [
                    prop.className
                        "flex-no-shrink p-2 px-12 rounded bg-teal-600 outline-none focus:ring-2 ring-teal-300 font-bold text-white hover:bg-teal disabled:opacity-30 disabled:cursor-not-allowed"
                    prop.disabled (Todo.isValid model.Input |> not)
                    prop.onClick (fun _ -> dispatch (SaveTodo(Start model.Input)))
                    prop.text "Add"
                ]
            ]
        ]

    let todoList model dispatch =
        Html.div [
            prop.className "bg-white/80 rounded-md shadow-md p-4 w-5/6 lg:w-3/4 lg:max-w-2xl"
            prop.children [
                Html.ol [
                    prop.className "list-decimal ml-6"
                    prop.children [
                        match model.DailyTargets with
                        | NotStarted -> Html.text "Not Started."
                        | Loading -> Html.text "Loading..."
                        | Loaded DailyTargets ->
                            for dailyTargets in DailyTargets do
                                Html.li [ prop.className "my-1"; prop.text (dailyTargets.Date.ToString()) ]
                    ]
                ]

                todoAction model dispatch
            ]
        ]

let view model dispatch =
    Html.section [
        prop.className "h-screen w-screen"
        prop.style [
            style.backgroundSize "cover"
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
            style.backgroundPosition "no-repeat center center fixed"
        ]

        prop.children [
            Html.a [
                prop.href "https://safe-stack.github.io/"
                prop.className "absolute block ml-12 h-12 w-12 bg-teal-300 hover:cursor-pointer hover:bg-teal-400"
                prop.children [ Html.img [ prop.src "/favicon.png"; prop.alt "Logo" ] ]
            ]

            Html.div [
                prop.className "flex flex-col items-center justify-center h-full"
                prop.children [
                    Html.h1 [
                        prop.className "text-center text-5xl font-bold text-white mb-3 rounded-md p-4"
                        prop.text "NutritionCalculator"
                    ]
                    ViewComponents.todoList model dispatch
                ]
            ]
        ]
    ]