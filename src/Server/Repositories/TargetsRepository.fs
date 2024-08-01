module TargetsRepository
open Records


let GetAllDailyTargets () =
        Context.db.Targets.FindAll() |> List.ofSeq

let CreateDailyTargets (dailyTargets: Targets) =
        if Targets.isValid dailyTargets.Date
        then Context.db.Targets.Insert (Targets.create dailyTargets.Date) |> ignore
        Ok()