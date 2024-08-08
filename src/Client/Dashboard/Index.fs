module Dashboard.Index

open Dashboard.State

open SAFE
open Browser.CssExtensions
open Feliz

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
            Components.navBarWidget model dispatch
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
                        prop.className "grid grid-cols-3 grid-rows-3 gap-2 grow"
                        prop.style [
                            style.gridTemplateColumns [ length.fr 1; length.fr 2; length.fr 1; ]
                        ]
                        prop.children [
                            Html.div [
                                prop.id "personal-information-container"
                                prop.className "flex flex-col"
                                prop.children [
                                    Components.personalInformationWidget model
                                ]
                            ]
                            Html.div [
                                prop.className "flex items-center justify-center"
                                prop.style [style.backgroundColor "#85B79D"]
                                prop.children [
                                    Html.canvas [
                                        prop.id "weight-chart-container"
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
                                    Components.dailyTargetsWidget model dispatch
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
            Components.userInformationFormModal model dispatch
            match model.User with
                | NotStarted -> ()
                | Loading -> ()
                | Loaded user ->
                    // If no user exists within the database, display the Create User modal
                    match user with
                    | None -> (Browser.Dom.document.getElementById "user-information-form-modal").style.display <- "block"
                    | Some user -> ()

            Components.userWeightFormModal model dispatch
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