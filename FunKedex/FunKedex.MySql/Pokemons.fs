namespace FunKedex.MySql

//FSI: build the project before sending to FSI :-)
//#I "../packages"
//#r "MySql.Data.6.9.9/lib/net45/MySql.Data.dll"
//#I "bin/Debug"
//#r "FunKedex.Domain.dll"

module Pokemons =
    open System
    open FunKedex.Domain
    open FunKedex.Domain.Pokemons
    open MySql.Data.MySqlClient

    let private connectionString = "server=localhost;user=root;database=pokemons;port=3306;password=root;"

    let loadPokemonsAsync () = async {
        let connection = new MySqlConnection (connectionString)
        try
            connection.Open ()
            let cmd = new MySqlCommand("SELECT * FROM pokemons", connection)
            use! reader = cmd.ExecuteReaderAsync () |> Async.AwaitTask

            let rows = seq {
                while reader.Read() do
                    yield { id = reader.GetInt32 0 |> int
                            ; name = reader.GetString 1
                            ; height = reader.GetInt32 2 |> int
                            ; weight = reader.GetInt32 3 |> int
                            ; spriteUrl = reader.GetString 4
                            ; types = (reader.GetString 5).Split [|';'|] |> Array.toList |> List.map (fun t -> PokemonType t)
                        }
            }

            return rows |> Seq.toList
        with
            | ex ->
                printfn "Error connecting to the database. Error = %A" ex
                return []
    }
