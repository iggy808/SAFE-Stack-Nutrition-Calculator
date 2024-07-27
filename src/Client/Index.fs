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
                Cmd.OfAsync.perform nutritionApi.createDailyTargets (DailyTargets.create (DateOnly.FromDateTime(DateTime.Now))) (Finished >> SaveTodo)

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

    let dailyTargetsSidebar model dispatch =
        Html.div [
            prop.style [
                style.borderWidth 1
                style.borderColor "black"
            ]
        ]

let view model dispatch =
    Html.div [
        prop.className "h-screen w-screen flex flex-col"
        prop.style [
            style.backgroundSize "cover"
            style.backgroundColor "#C0E5C8"
            style.backgroundPosition "no-repeat center center fixed"
            style.margin 0
        ]

        prop.children [
            Html.div [
                prop.id "title-container"
                prop.className "my-[25px]"
                prop.children [
                     Html.h1 [
                        prop.className "text-center text-5xl font-bold p-4"
                        prop.text "Nutrition Calculator"
                        prop.style [ style.color "#16302B" ]
                    ]
                ]
            ]
            Html.div [
                prop.id "nav-bar-container"
                prop.className "mx-[75px] mt-[10px] mb-[20px] flex flex-row flex-nowrap justify-stretch h-8"
                prop.style [
                    style.border (length.px 1, borderStyle.solid, "black")
                ]
                prop.children [
                    Html.div [
                        prop.className "grow flex items-center justify-center"
                        prop.style [
                            style.border (length.px 1, borderStyle.solid, "black")
                            style.backgroundColor "#A38F8F"
                            style.textAlign.center
                        ]
                        prop.children [
                            Html.a [
                                prop.text "Area 1"
                                prop.href "#"
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className "grow flex items-center justify-center"
                        prop.style [
                            style.border (length.px 1, borderStyle.solid, "black")
                            style.backgroundColor "#A38F8F"
                            style.textAlign.center
                        ]
                        prop.children [
                            Html.a [
                                prop.text "Area 2"
                                prop.href "#"
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className "grow flex items-center justify-center"
                        prop.style [
                            style.border (length.px 1, borderStyle.solid, "black")
                            style.backgroundColor "#A38F8F"
                            style.textAlign.center
                        ]
                        prop.children [
                            Html.a [
                                prop.text "Area 3"
                                prop.href "#"
                            ]
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.id "dashboard-container"
                prop.className "mx-[75px] mb-[50px] flex flex-col"
                prop.style [
                    style.flexGrow 1
                    style.border (length.px 1, borderStyle.solid, "black")
                ]
                prop.children [
                    Html.div [
                        prop.id "dashboard-grid"
                        prop.className "grid grid-cols-3 grid-rows-3 gap-4"
                        prop.style [
                            style.flexGrow 1
                        ]
                        prop.children [
                            Html.div [
                                prop.id "personal-information-container"
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text "Personal Information Container"
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 2
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 3
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.id "daily-targets-container"
                                prop.className "flex items-center justify-center row-start-2 row-end-4 "
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text "Daily Targets Container"
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 5
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 6
                                    ]
                                ]
                            ]
                            (*
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 7
                                    ]
                                ]
                            ]*)
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 8
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.p [
                                        prop.text 9
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            (*
            Html.div [
                prop.className "flex flex-col items-center h-full"
                prop.children [
                    Html.h1 [
                        prop.className "text-center text-5xl font-bold text-white mb-3 rounded-md p-4"
                        prop.text "NutritionCalculator"
                        prop.style [ style.color "#16302B" ]
                    ]
                    ViewComponents.todoList model dispatch
                ]
            ]
            *)
        ]
    ]