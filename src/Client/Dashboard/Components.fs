module Dashboard.Components

open Dashboard.State

open Feliz
open SAFE
open System
open Records
open Browser.CssExtensions
open Commands

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
                (*prop.onKeyPress (fun ev ->
                    if ev.key = "Enter" then
                        dispatch (SaveTodo(Start model.Input)))*)
            ]
            (*
            Html.button [
                prop.className
                    "flex-no-shrink p-2 px-12 rounded bg-teal-600 outline-none focus:ring-2 ring-teal-300 font-bold text-white hover:bg-teal disabled:opacity-30 disabled:cursor-not-allowed"
                prop.disabled (Todo.isValid model.Input |> not)
                (*prop.onClick (fun _ -> dispatch (SaveTodo(Start model.Input)))*)
                prop.text "Add"
            ]
            *)
        ]
    ]

(*
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
*)

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

let personalInformationWidget (model:Model) =
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
                match model.User with
                | NotStarted -> prop.text "Hello!"
                | Loading -> prop.text "Hello!"
                | Loaded user ->
                    match user with
                    | Some user -> prop.text ("Hello, " + user.Name + "!")
                    | None -> prop.text "Hello!"
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
                                match model.User with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded user ->
                                    match user with
                                    | Some user -> prop.text user.Age
                                    | None -> prop.text "N/A"
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
                                match model.User with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded user ->
                                    match user with
                                    | Some user -> prop.text user.Height
                                    | None -> prop.text "N/A"
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
                                match model.User with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded user ->
                                    match user with
                                    | Some user -> prop.text user.Weight
                                    | None -> prop.text "N/A"
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
                                match model.User with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded user ->
                                    match user with
                                    | Some user -> prop.text user.ActivityFactor
                                    | None -> prop.text "N/A"
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let dailyTargetsWidget (model: Model) dispatch =
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
                                prop.text "Maintenance Calories"
                            ]
                            Html.label [
                                prop.className "text-2xl basis-3/4 ml-4 mt-4"
                                prop.style [
                                    style.color "#16302B"
                                ]
                                match model.Targets with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded targets ->
                                    match targets with
                                    | Some targets -> prop.text targets.MaintenanceCalories
                                    | None -> prop.text "N/A"
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
                                match model.Targets with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded targets ->
                                    match targets with
                                    | Some targets -> prop.text (string targets.ProteinGramsPerDay + " grams")
                                    | None -> prop.text "N/A"
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
                                match model.Targets with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded targets ->
                                    match targets with
                                    | Some targets -> prop.text (string targets.FatGramsPerDay + " grams")
                                    | None -> prop.text "N/A"
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
                                match model.Targets with
                                | NotStarted -> prop.text "N/A"
                                | Loading -> prop.text "N/A"
                                | Loaded targets ->
                                    match targets with
                                    | Some targets -> prop.text (string targets.CarbsGramsPerDay + " grams")
                                    | None -> prop.text "N/A"
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

                                                    dispatch (CreateUser(Start(newUser)))
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

let userWeightFormModal (model: Model) dispatch =
    Html.div [
        prop.id "update-user-weight-form-modal"
        // Dim website when modal is visible
        prop.className "fixed h-full w-full hidden bg-gray-600/50 flex flex-col"
        prop.children [
            Html.div [
                prop.className "relative mt-[75px] mx-auto p-5 w-1/4 h-3/8 shadow-lg rounded-full shadow-lg"
                prop.style [style.backgroundColor "#E3D0D8"]
                prop.children [
                    Html.h3 [
                        prop.className "text-2xl font-bold"
                        prop.text "Update User Weight"
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
                                        prop.text "New Weight (lb)"
                                    ]
                                    Html.input [
                                        prop.id "update-user-weight-form-element"
                                        prop.className "rounded-md w-full shadow"
                                        prop.style [
                                            style.backgroundColor "#85B79D"
                                            style.color "#16302B"
                                        ]
                                        prop.type' "text"
                                        prop.onChange (fun value ->
                                            let elem = Browser.Dom.document.getElementById "update-user-weight-form-element"
                                            elem.setAttribute ("value", value)
                                        )
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className "mx-auto w-1/4"
                                prop.children [
                                    Html.button [
                                        prop.id "update-user-weight-form-submit-button"
                                        prop.className "mt-1 px-4 py-2 text-white text-base font-medium rounded-md w-full shadow-sm focus:outline-none focus:ring-2 focus:ring-purple-300"
                                        prop.style [style.backgroundColor "#8C5F66"]
                                        prop.type' "button"
                                        prop.text "Submit"
                                        prop.onClick (fun _ ->
                                            // Hide modal
                                            (Browser.Dom.document.getElementById "update-user-weight-form-modal").style.display <- "none"

                                            // Collect updated user weight from form
                                            let userWeight = (Browser.Dom.document.getElementById "update-user-weight-form-element").getAttribute "value" |> float
                                                
                                            dispatch (UpdateUserWeight(Start(
                                               match model.User with
                                               | NotStarted -> None
                                               | Loading -> None
                                               | Loaded user ->
                                                    match user with
                                                    | Some user -> Some { UserId = user.Id; Weight = userWeight }
                                                    | None -> None
                                            )))
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