namespace Shared.Queries

open System

type GetDailyUserTargetsQuery = {
    UserId:Guid
    Date:DateOnly
}