module Index

open Elmish
open SAFE
open Shared
open System
open Records
open Browser.CssExtensions

type Model = {
    UserInformation: RemoteData<User>
    UserInformationDto: User
    Targets: RemoteData<Targets list>
    Input: string
}

type Msg =
    | SetInput of string
    | LoadDailyTargets of ApiCall<unit, Targets list>
    | SaveTodo of ApiCall<string, Targets>
    | SubmitUserInformationForm of User
    | GetUserInformation of ApiCall<unit, User>
    | PostUserInformation of ApiCall<User, unit>

let nutritionApi = Api.makeProxy<INutritionApi> ()

let init () =
    let initialModel = {
        UserInformation = NotStarted;
        UserInformationDto = User.Default
        Targets = NotStarted;
        Input = "" }
    //let initialCmd = LoadDailyTargets(Start()) |> Cmd.ofMsg
    let initialCmd = GetUserInformation(Start()) |> Cmd.ofMsg

    initialModel, initialCmd

let update msg model =
    match msg with
    | GetUserInformation msg ->
        match msg with
        | Start() ->
            Browser.Dom.console.log "Get user from server started"
            let loadUserInformation_command = Cmd.OfAsync.perform nutritionApi.getUser () (Finished >> GetUserInformation)
            { model with UserInformation = Loading }, loadUserInformation_command

        | Finished userInformation ->
            Browser.Dom.console.log "User fetched from server"
            Browser.Dom.console.log ("User Id: " + userInformation.Id.ToString() + "User Name: " + userInformation.Name)
            { model with UserInformation = Loaded userInformation }, Cmd.none

    | PostUserInformation msg ->
        match msg with
        | Start user ->
            Browser.Dom.console.log "Post user to server started"
            Browser.Dom.console.log ("user info dto in update method - Pre Post: " + user.ToString())

            let updateUserInformation_command = Cmd.OfAsync.perform nutritionApi.createUser user (Finished >> PostUserInformation)
            model, updateUserInformation_command

        | Finished _ ->
            Browser.Dom.console.log "User posted to server"
            let loadUserInformation_command = Cmd.OfAsync.perform nutritionApi.getUser () (Finished >> GetUserInformation)
            { model with UserInformation = Loading }, loadUserInformation_command

    | SubmitUserInformationForm user ->
        { model with UserInformationDto =  user }, Cmd.none


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

    let userInformationFormModal (model: Model) dispatch =
        Html.div [
            prop.id "user-information-form-modal"
            // Dim website when modal is visible
            prop.className "fixed h-full w-full hidden bg-gray-600/50 flex flex-col"
            prop.children [
                Html.div [
                    prop.className "relative mt-[75px] mx-auto p-5 w-1/4 h-3/8 shadow-lg rounded-full shadow-lg"
                    prop.style [style.backgroundColor "#E3D0D8"]
                    prop.children [
                        Html.div [
                            prop.className "text-center mt-3"
                            prop.children [
                                Html.h3 [
                                    prop.className "text-2xl font-bold"
                                    prop.text "User Information Form"
                                    prop.style [style.color "#8C5F66"]
                                ]
                                Html.form [
                                    prop.children [
                                        Html.div [
                                            prop.className "flex flex-col text-left text-lg my-3 mx-20"
                                            prop.children [
                                                Html.label [
                                                    prop.className "font-bold"
                                                    prop.style [style.color "#8C5F66"]
                                                    prop.text "Name"
                                                ]
                                                Html.input [
                                                    prop.id "user-information-form-element-name"
                                                    prop.className "rounded-md w-full shadow"
                                                    prop.style [
                                                        style.backgroundColor "#85B79D"
                                                        style.color "#16302B"
                                                    ]
                                                    prop.type' "text"
                                                    prop.onChange (fun value ->
                                                        let elem = Browser.Dom.document.getElementById "user-information-form-element-name"
                                                        elem.setAttribute ("value", value)
                                                    )
                                                ]
                                            ]
                                        ]
                                        Html.div [
                                            prop.className "flex flex-col text-left text-lg my-3 mx-20"
                                            prop.children [
                                                Html.label [
                                                    prop.className "font-bold"
                                                    prop.style [style.color "#8C5F66"]
                                                    prop.text "Age"
                                                ]
                                                Html.input [
                                                    prop.id "user-information-form-element-age"
                                                    prop.className "rounded-md w-full shadow"
                                                    prop.style [
                                                        style.backgroundColor "#85B79D"
                                                        style.color "#16302B"
                                                    ]
                                                    prop.type' "number"
                                                    prop.onChange (fun value ->
                                                        let elem = Browser.Dom.document.getElementById "user-information-form-element-age"
                                                        elem.setAttribute ("value", value)
                                                    )
                                                ]
                                            ]
                                        ]
                                        Html.div [
                                            prop.className "flex flex-col text-left text-lg my-3 mx-20"
                                            prop.children [
                                                Html.label [
                                                    prop.className "font-bold"
                                                    prop.style [style.color "#8C5F66"]
                                                    prop.text "Height (in inches)"
                                                ]
                                                Html.input [
                                                    prop.id "user-information-form-element-height"
                                                    prop.className "rounded-md w-full shadow"
                                                    prop.style [
                                                        style.backgroundColor "#85B79D"
                                                        style.color "#16302B"
                                                    ]
                                                    prop.type' "number"
                                                    prop.onChange (fun value ->
                                                        let elem = Browser.Dom.document.getElementById "user-information-form-element-height"
                                                        elem.setAttribute ("value", value)
                                                    )
                                                ]
                                            ]
                                        ]
                                        Html.div [
                                            prop.className "flex flex-col text-left text-lg my-3 mx-20"
                                            prop.children [
                                                Html.label [
                                                    prop.className "font-bold"
                                                    prop.style [style.color "#8C5F66"]
                                                    prop.text "Weight (in pounds)"
                                                ]
                                                Html.input [
                                                    prop.id "user-information-form-element-weight"
                                                    prop.className "rounded-md w-full shadow"
                                                    prop.style [
                                                        style.backgroundColor "#85B79D"
                                                        style.color "#16302B"
                                                    ]
                                                    prop.type' "number"
                                                    prop.onChange (fun value ->
                                                        let elem = Browser.Dom.document.getElementById "user-information-form-element-weight"
                                                        elem.setAttribute ("value", value)
                                                    )
                                                ]
                                            ]
                                        ]
                                        Html.div [
                                            prop.className "flex flex-col text-left text-lg my-3 mx-20"
                                            prop.children [
                                                Html.label [
                                                    prop.className "font-bold"
                                                    prop.style [style.color "#8C5F66"]
                                                    prop.text "Activity Factor"
                                                ]
                                                Html.input [
                                                    prop.id "user-information-form-element-activity-factor"
                                                    prop.className "rounded-md w-full shadow"
                                                    prop.style [
                                                        style.backgroundColor "#85B79D"
                                                        style.color "#16302B"
                                                    ]
                                                    prop.type' "number"
                                                    prop.onChange (fun value ->
                                                        let elem = Browser.Dom.document.getElementById "user-information-form-element-activity-factor"
                                                        elem.setAttribute ("value", value)
                                                    )
                                                ]
                                            ]
                                        ]
                                        Html.div [
                                            prop.className "mx-auto w-1/4"
                                            prop.children [
                                                Html.button [
                                                    prop.id "user-information-form-submit-button"
                                                    prop.className "mt-1 px-4 py-2 text-white text-base font-medium rounded-md w-full shadow-sm focus:outline-none focus:ring-2 focus:ring-purple-300"
                                                    prop.style [style.backgroundColor "#8C5F66"]
                                                    prop.type' "button"
                                                    prop.text "Submit"
                                                    prop.onClick (fun _ ->
                                                        // Hide modal
                                                        (Browser.Dom.document.getElementById "user-information-form-modal").style.display <- "none"

                                                        // Call update method in order to post user data to the server
                                                        let newUser = {
                                                            Id = Guid.NewGuid();
                                                            Name = (Browser.Dom.document.getElementById "user-information-form-element-name").getAttribute "value";
                                                            Age = (Browser.Dom.document.getElementById "user-information-form-element-age").getAttribute "value" |> int;
                                                            Height = (Browser.Dom.document.getElementById "user-information-form-element-height").getAttribute "value" |> int;
                                                            Weight = (Browser.Dom.document.getElementById "user-information-form-element-weight").getAttribute "value" |> float;
                                                            ActivityFactor = (Browser.Dom.document.getElementById "user-information-form-element-activity-factor").getAttribute "value" |> float;
                                                        }
                                                        dispatch (SubmitUserInformationForm(newUser))

                                                        dispatch (PostUserInformation(Start(newUser)))
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
            ]
        ]

let view (model: Model) dispatch =
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
            ViewComponents.userInformationFormModal model dispatch
            match model.UserInformation with
                | NotStarted -> ()
                | Loading -> ()
                | Loaded userInformation ->
                    if userInformation.Id = Guid.Empty
                    then (Browser.Dom.document.getElementById "user-information-form-modal").style.display <- "block"
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