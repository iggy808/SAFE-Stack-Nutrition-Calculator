module Endpoints.User

open Records

let getUser = async {
    return
        Context.db.Users.FindAll()
        |> List.ofSeq
        |> List.tryExactlyOne
        |> function
            | Some user -> Some user
            | None -> None
}

let createUser (user:User) = async {
    user
    |> User.isValid
    |> function
        | true -> Context.db.Users.Insert user |> ignore
        | false -> ()
}

let updateUserWeight command = async {
    return ()
}