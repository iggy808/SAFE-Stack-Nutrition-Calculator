module App

open Elmish
open Elmish.React
open Dashboard

open Fable.Core.JsInterop

importSideEffects "./index.css"

#if DEBUG
open Elmish.HMR
#endif

Program.mkProgram Dashboard.Handler.init Dashboard.Handler.update Index.view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "elmish-app"

|> Program.run