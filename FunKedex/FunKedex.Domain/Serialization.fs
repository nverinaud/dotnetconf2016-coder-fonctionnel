namespace FunKedex.Domain

module Serialization =
    open FunKedex.Domain.Pokemons
    open System.Runtime.Serialization

    [<DataContract>]
    type SerializedPokemon = {
        [<DataMember(Name = "id")>]
        id : int
        [<DataMember(Name = "name")>]
        name : string
        [<DataMember(Name = "height")>]
        height : int
        [<DataMember(Name = "weight")>]
        weight : int
        [<DataMember(Name = "spriteUrl")>]
        spriteUrl : string
        [<DataMember(Name = "types")>]
        types : string list
    }

    let serializePokemon (p : Pokemon) : SerializedPokemon = 
        { id = p.id
        ; name = p.name
        ; height = p.height
        ; weight = p.weight
        ; spriteUrl = p.spriteUrl
        ; types = p.types |> List.map (fun (PokemonType t) -> t) }

    let deserializePokemon (p : SerializedPokemon) : Pokemon =
        { id = p.id
        ; name = p.name
        ; height = p.height
        ; weight = p.weight
        ; spriteUrl = p.spriteUrl
        ; types = p.types |> List.map (fun t -> PokemonType t) }
