module UserRepository

open Context
open System.Linq

type UserRepository (context: Context) =

    member _.GetUser () =
        context.Users.FindAll().FirstOrDefault()