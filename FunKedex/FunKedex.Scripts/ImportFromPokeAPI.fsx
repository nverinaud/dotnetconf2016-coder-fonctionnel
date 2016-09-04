
#I "../packages"
#r "FSharp.Data.2.3.2/lib/portable-net45+netcore45/FSharp.Data.dll"
#r "MySql.Data.6.9.9/lib/net45/MySql.Data.dll"

open System
open System.IO
open FSharp.Data
open MySql.Data.MySqlClient
open MySql.Data.Types

type Pokemon = {
    id : int
    name : string
    height : int
    weight : int
    spriteUrl : string
    types : string list
}

let pikachu = {
    id = 25
    name = "pikachu"
    height = 40
    weight = 2
    spriteUrl = ""
    types = []
}

type Pokemons = JsonProvider<"http://pokeapi.co/api/v2/pokemon">
type PokemonDetails = JsonProvider<"http://pokeapi.co/api/v2/pokemon/1">

let ``les seuls pokemons ayant jamais existés`` () = 
    printfn "Downloading pokemons list..."
    Pokemons.AsyncLoad "http://pokeapi.co/api/v2/pokemon/?limit=151"

let allPokemons () =
    ``les seuls pokemons ayant jamais existés`` ()
    |> Async.RunSynchronously
    |> (fun x -> x.Results)
    |> Array.toSeq
    |> Seq.map (fun x -> async {
            printfn "Downloading %s..." x.Name
            let! rawPoke = PokemonDetails.AsyncLoad x.Url 
            return { 
                id = rawPoke.Id 
                name = rawPoke.Name 
                height = rawPoke.Height 
                weight = rawPoke.Weight 
                spriteUrl = rawPoke.Sprites.FrontDefault 
                types = rawPoke.Types |> Array.toList |> List.map (fun t -> t.Type.Name) 
            } 
        })
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.toList
  
let executeNonQuery connection sql =
    let cmd = new MySqlCommand (sql, connection)
    cmd.ExecuteNonQuery ()

let createPokeTable connection =
    printfn "Creating pokemons table..."
    executeNonQuery connection """
        CREATE TABLE IF NOT EXISTS pokemons
        (
            id INT PRIMARY KEY NOT NULL,
            name VARCHAR(255) NOT NULL,
            height INT NOT NULL,
            weight INT NOT NULL,
            sprite_url VARCHAR(255) NOT NULL,
            types TEXT NOT NULL
        )
    """

let deletePokeTable connection =
    printfn "Removing old pokemons..."
    executeNonQuery connection "DELETE FROM pokemons"

let sqlForPokemon pokemon =
    sprintf "(%d, '%s', %d, %d, '%s', '%s')" pokemon.id pokemon.name pokemon.height pokemon.weight pokemon.spriteUrl (String.Join (";", pokemon.types))

let insertPokemons connection pokemons =
    printfn "Inserting %d pokemons..." (pokemons |> List.length)
    let values = String.Join (",", pokemons |> List.map sqlForPokemon)
    let sql = sprintf "INSERT INTO pokemons (id, name, height, weight, sprite_url, types) VALUES %s;" values
    executeNonQuery connection sql

let go () =
    let connectionString = "server=localhost;user=root;database=pokemons;port=3306;password=root;"
    let connection = new MySqlConnection (connectionString)
    try
        connection.Open ()
        createPokeTable connection |> ignore
        deletePokeTable connection |> ignore
        let pokemons = allPokemons ()
        insertPokemons connection pokemons |> ignore
        printfn "SUCCESS: %d pokemons have been inserted." (pokemons |> List.length)
    with
        | ex -> printfn "%A" ex

go ()