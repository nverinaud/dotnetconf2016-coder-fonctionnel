namespace FunKedex.Domain

module Pokemons =
    type PokemonId = int
    type PokemonType = PokemonType of string
    type PokemonTypes = PokemonType list

    type Pokemon = {
        id : PokemonId
        name : string
        height : int
        weight : int
        spriteUrl : string
        types : PokemonTypes
    }

    type LoadPokemonsAsync = unit -> Async<Pokemon list>


