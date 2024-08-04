module Endpoints.User

open Records

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

let updateUserWeight command = async {
    Context.db.Users.FindAll()
    |> Seq.tryExactlyOne
    |> function
       | Some user -> Context.db.Users.Update user |> ignore
       | None -> ()
}