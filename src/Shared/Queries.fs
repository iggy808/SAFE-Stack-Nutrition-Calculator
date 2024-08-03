namespace Queries

open System

type GetDailyUserTargetsQuery = {
    UserId:Guid
    Date:DateOnly
}