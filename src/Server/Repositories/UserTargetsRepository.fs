module UserTargetsRepository
open System
open System.Linq
open Shared.Queries
open Records


let CreateDailyTargets userId =
    let user = Context.db.Users.FindOne(fun user -> user.Id = userId)

    { Id = Guid.Empty; UserId = userId; Date = DateOnly.FromDateTime(DateTime.Now) } // Todo: compute targets

let GetDailyUserTargets (query:GetDailyUserTargetsQuery) =
        let dailyTargets = Context.db.UserTargets.Find(fun userTarget ->
                userTarget.UserId = query.UserId &&
                userTarget.Date = DateOnly.FromDateTime(DateTime.Now)) |> List.ofSeq

        // Unsure how to perform a null check / option match on the value returned from FindOne - will need to research
        // For now, hacky list check will do
        if dailyTargets.Any()
        then dailyTargets.Head
        else CreateDailyTargets query.UserId