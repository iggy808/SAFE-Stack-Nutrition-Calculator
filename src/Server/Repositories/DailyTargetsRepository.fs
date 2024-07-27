module DailyTargetsRepository

open Context
open Records

type DailyTargetsRepository(context:Context) =

    member _.GetAllDailyTargets () =
        context.DailyTargets.FindAll() |> List.ofSeq

    member _.CreateDailyTargets (dailyTargets: DailyTargets) =
        if DailyTargets.isValid dailyTargets.Date
        then context.DailyTargets.Insert (DailyTargets.create dailyTargets.Date) |> ignore
        Ok()