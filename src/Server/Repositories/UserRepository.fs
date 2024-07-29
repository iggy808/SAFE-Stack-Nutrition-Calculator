module UserRepository

open Context
open System
open Records
open System.Linq

type UserRepository (context: Context) =
    member _.GetUser () =
        let users = context.Users.FindAll() 
        if users.Count() = 0
        then User.Default
        else users.First()

    member _.CreateUser (user: User) =
        context.Users.Insert (user) |> ignore
        Ok()