namespace Queries

open System

type GetUserTargetsByDateQuery = {
    UserId:Guid
    Date:DateOnly
}