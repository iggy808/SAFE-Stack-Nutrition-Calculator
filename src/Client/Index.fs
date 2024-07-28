module Index

open Elmish
open SAFE
open Shared
open System
open Records
open Browser.CssExtensions

type Model = {
    UserInformation: RemoteData<User>
    Targets: RemoteData<Targets list>
    Input: string
}

type Msg =
    | SetInput of string
    | LoadDailyTargets of ApiCall<unit, Targets list>
    | SaveTodo of ApiCall<string, Targets>
    | LoadUserInformation of ApiCall<unit, User>

let nutritionApi = Api.makeProxy<INutritionApi> ()

let init () =
    let initialModel = { UserInformation = NotStarted; Targets = NotStarted; Input = "" }
    //let initialCmd = LoadDailyTargets(Start()) |> Cmd.ofMsg
    let initialCmd = LoadUserInformation(Start()) |> Cmd.ofMsg

    initialModel, initialCmd

let update msg model =
    match msg with
    | LoadUserInformation msg ->
        match msg with
        | Start() ->
            let loadUserInformation_command = Cmd.OfAsync.perform nutritionApi.getUser () (Finished >> LoadUserInformation)
            { model with UserInformation = Loading }, loadUserInformation_command
        | Finished userInformation ->
            { model with UserInformation = Loaded userInformation }, Cmd.none


    | SetInput value -> { model with Input = value }, Cmd.none
    | LoadDailyTargets msg ->
        match msg with
        | Start() ->
            let loadDailyTargets_command = Cmd.OfAsync.perform nutritionApi.getDailyTargets () (Finished >> LoadDailyTargets)

            { model with Targets = Loading }, loadDailyTargets_command
        | Finished dailyTargets -> { model with Targets = Loaded dailyTargets }, Cmd.none
    | SaveTodo msg ->
        match msg with
        | Start todoText ->
            let saveTodoCmd =
                let todo = Todo.create todoText
                Cmd.OfAsync.perform nutritionApi.createDailyTargets (Targets.create (DateOnly.FromDateTime(DateTime.Now))) (Finished >> SaveTodo)

            { model with Input = "" }, saveTodoCmd
        | Finished DailyTargets ->
            {
                model with
                    Targets = model.Targets |> RemoteData.map (fun dailyTargets -> dailyTargets @ [ DailyTargets ])
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
                        match model.Targets with
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

    let navBarWidget model dispatch =
        Html.div [
            prop.id "nav-bar-container"
            prop.className "mx-[75px] mt-[10px] mb-[20px] flex flex-row flex-nowrap justify-stretch h-8"
            prop.style [
                //style.border (length.px 1, borderStyle.solid, "black")
            ]
            prop.children [
                Html.div [
                    prop.className "grow flex items-center justify-center"
                    prop.style [
                        //style.border (length.px 1, borderStyle.solid, "black")
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
                        //style.border (length.px 1, borderStyle.solid, "black")
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
                        //style.border (length.px 1, borderStyle.solid, "black")
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

    let personalInformationWidget model dispatch =
        Html.div [
            prop.id "personal-information-widget"
            prop.className "m-[10px] grow flex flex-col"
            prop.style [
                style.border (length.px 1, borderStyle.solid, "transparent")
                style.borderRadius (length.px 10)
                style.backgroundColor "#85B79D"
            ]

            prop.children [
                Html.h2 [
                    prop.id "personal-information-greeting"
                    prop.className "text-3xl font-bold p-4"
                    prop.text ("Hello, " + "Evan" + "!")
                    prop.style [
                        style.color "#16302B"
                        style.borderBottom (length.px 1, borderStyle.solid, "#16302B")
                    ]
                ]
                Html.div [
                    prop.id "personal-information-details"
                    prop.className "mx-4 mt-4 grow flex flex-col"
                    prop.style [
                        //style.border (length.px 1, borderStyle.solid, "black")
                    ]
                    prop.children [
                        Html.div [
                            prop.className "grow"
                            prop.children [
                                Html.label [
                                    prop.style [style.color "#16302B"]
                                    prop.className "text-xl font-bold"
                                    prop.text "Age: "
                                ]
                                Html.label [
                                    prop.className "text-xl"
                                    prop.text 23
                                ]
                            ]

                        ]
                        Html.div [
                            prop.className "grow"
                            prop.children [
                                Html.label [
                                    prop.style [style.color "#16302B"]
                                    prop.className "text-xl font-bold"
                                    prop.text "Height (in inches): "
                                ]
                                Html.label [
                                    prop.className "text-xl"
                                    prop.text 71
                                ]
                            ]
                        ]
                        Html.div [
                            prop.className "grow"
                            prop.children [
                                Html.label [
                                    prop.style [style.color "#16302B"]
                                    prop.className "text-xl font-bold"
                                    prop.text "Weight (in pounds): "
                                ]
                                Html.label [
                                    prop.className "text-xl"
                                    prop.text 270
                                ]
                            ]
                        ]
                        Html.div [
                            prop.className "grow"
                            prop.children [
                                Html.label [
                                    prop.style [style.color "#16302B"]
                                    prop.className "text-xl font-bold"
                                    prop.text "Activity Factor: "
                                ]
                                Html.label [
                                    prop.className "text-xl"
                                    prop.text 1.3
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]

    let dailyTargetsWidget model dispatch =
        Html.div [
            prop.id "daily-targets-widget"
            prop.className "m-[10px] grow flex flex-col"
            prop.style [
                style.border (length.px 1, borderStyle.solid, "transparent")
                style.borderRadius (length.px 10)
                style.backgroundColor "#85B79D"
            ]
            prop.children [
                Html.h2 [
                    prop.id "daily-targets-date"
                    prop.className "text-3xl font-bold p-4"
                    prop.text ("Targets - " + (DateOnly.FromDateTime(DateTime.Now).ToString()))
                    prop.style [
                        style.color "#16302B"
                        style.borderBottom (length.px 1, borderStyle.solid, "#16302B")
                    ]

                ]
                Html.div [
                    prop.id "daily-targets-content"
                    prop.className "mx-4 mt-4 flex flex-col grow"
                    prop.children [
                        Html.div [
                            prop.className "grow flex flex-col"
                            prop.style [
                                //style.border (length.px 1, borderStyle.solid, "black")
                            ]
                            prop.children [
                                Html.label [
                                    prop.className "text-2xl basis-1/4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text "Target Calories"
                                ]
                                Html.label [
                                    prop.className "text-2xl basis-3/4 ml-4 mt-4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text ("500000" + " calories")
                                ]
                            ]        
                        ]
                        Html.div [
                            prop.className "grow flex flex-col"
                            prop.style [
                                //style.border (length.px 1, borderStyle.solid, "black")
                            ]
                            prop.children [
                                Html.label [
                                    prop.className "text-2xl basis-1/4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text "Target Protein Intake"
                                ]
                                Html.label [
                                    prop.className "text-2xl basis-3/4 ml-4 mt-4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text ("500000" + " grams")
                                ]
                            ]        
                        ]
                        Html.div [
                            prop.className "grow flex flex-col"
                            prop.style [
                                //style.border (length.px 1, borderStyle.solid, "black")
                            ]
                            prop.children [
                                Html.label [
                                    prop.className "text-2xl basis-1/4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text "Target Fat Intake"
                                ]
                                Html.label [
                                    prop.className "text-2xl basis-3/4 ml-4 mt-4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text ("500000" + " grams")
                                ]
                            ]        
                        ]
                        Html.div [
                            prop.className "grow flex flex-col"
                            prop.style [
                                //style.border (length.px 1, borderStyle.solid, "black")
                            ]
                            prop.children [
                                Html.label [
                                    prop.className "text-2xl basis-1/4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text "Target Carb Intake"
                                ]
                                Html.label [
                                    prop.className "text-2xl basis-3/4 ml-4 mt-4"
                                    prop.style [
                                        style.color "#16302B"
                                    ]
                                    prop.text ("500000" + " grams")
                                ]
                            ]        
                        ]
                    ]
                ]
            ]
        ]

    let userInformationFormModal model dispatch =
        Html.div [
            prop.id "user-information-form-modal"
            prop.className "fixed hidden insert-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full"
            prop.children [
                Html.div [
                    prop.className "relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white"
                    prop.children [
                        Html.div [
                            prop.className "mt-3 text-center"
                            prop.children [
                                Html.div [
                                    prop.className "mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-purple-100"
                                    prop.text "nice"
                                ]
                                Html.h3 [
                                    prop.className "text-lg leading-6 font-medium text-gray-900"
                                    prop.text "nice"
                                ]
                                Html.div [
                                    prop.className "mt-2 px-7 py-3"
                                    prop.children [
                                        Html.p [
                                            prop.className "text-sm text-gray-500"
                                            prop.text "nice"
                                        ]
                                    ]
                                ]
                                Html.div [
                                    prop.className "items-center px-4 py-3"
                                    prop.children [
                                        Html.button [
                                            prop.id "nicebutton"
                                            prop.className "px-4 py-2 bg-purple-500 text-white text-base font-medium rounded-md w-full shadow-sm hover:bg-purple-600 focus:outline-none focus:ring-2 focus:ring-purple-300"
                                            prop.text "nice"
                                            prop.onClick (fun _ ->
                                                (Browser.Dom.document.getElementById "user-information-form-modal").style.display <- "none"
                                            )
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
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
            ViewComponents.userInformationFormModal model dispatch
            Html.button [
                prop.id "testbutton"
                prop.text "nice"
                prop.onClick (fun _ ->
                    (Browser.Dom.document.getElementById "user-information-form-modal").style.display <- "block"
                )
            ]
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
            ViewComponents.navBarWidget model dispatch
            Html.div [
                prop.id "dashboard-container"
                prop.className "mx-[75px] mb-[50px] flex flex-col"
                prop.style [
                    style.flexGrow 1
                    //style.border (length.px 1, borderStyle.solid, "black")
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
                                prop.className "flex flex-col"
                                prop.children [
                                    
                                    ViewComponents.personalInformationWidget model dispatch
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
                                prop.className "flex flex-col row-start-2 row-end-4 "
                                prop.children [
                                    ViewComponents.dailyTargetsWidget model dispatch
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