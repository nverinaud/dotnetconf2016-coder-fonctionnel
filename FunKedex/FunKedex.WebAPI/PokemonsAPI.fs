namespace FunKedex.WebAPI

module PokemonsAPI =
    open Suave
    open Suave.Filters
    open Suave.Operators
    open Suave.Successful
    open FunKedex.Domain.Serialization
    open Newtonsoft.Json

    let private JSON x =
        JsonConvert.SerializeObject (x)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

    let getPokemons (loadPokemonsAsync : FunKedex.Domain.Pokemons.LoadPokemonsAsync) = 
        (fun httpContext -> async { 
            let! rawPokemons = loadPokemonsAsync ()
            let result = rawPokemons |> List.map serializePokemon
            return! JSON result httpContext
        })
