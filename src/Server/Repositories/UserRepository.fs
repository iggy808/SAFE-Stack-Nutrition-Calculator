module UserRepository
open Records
open System.Linq


let GetUser () =
    let users = Context.db.Users.FindAll() 
    if users.Any()
    then Some (users.First())
    else None

let CreateUser (user: User) =
    Context.db.Users.Insert (user) |> ignore
    Ok()