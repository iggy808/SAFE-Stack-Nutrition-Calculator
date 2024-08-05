module Endpoints.User

open Records
open Commands

// Todo: Add error handling
let getUser = async {
    return
        Context.db.Users.FindAll()
        |> Seq.tryExactlyOne
}

let createUser (user:User) = async {
    user
    |> User.isValid
    |> function
        | true -> Context.db.Users.Insert user |> ignore
        | false -> ()
}

let updateUserWeight (command:UpdateUserWeightCommand) = async {
    Context.db.Users.FindAll()
    |> Seq.tryExactlyOne
    |> function
       | Some user ->
            { user with Weight = command.Weight }
            |> Context.db.Users.Update
            |> ignore
       | None -> ()
}