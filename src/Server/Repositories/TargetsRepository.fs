module TargetsRepository

open Context
open Records

type TargetsRepository(context:Context) =

    member _.GetAllDailyTargets () =
        context.Targets.FindAll() |> List.ofSeq

    member _.CreateDailyTargets (dailyTargets: Targets) =
        if Targets.isValid dailyTargets.Date
        then context.Targets.Insert (Targets.create dailyTargets.Date) |> ignore
        Ok()