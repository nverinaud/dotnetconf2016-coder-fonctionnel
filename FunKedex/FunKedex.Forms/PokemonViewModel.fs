namespace FunKedex.Forms

module ViewModels =
    open Xamarin.Forms
    open System
    open FunKedex.Domain.Pokemons

    type PokemonViewModel = {
        name: string
        image: ImageSource
        height : string
        weight : string
        types : string
    }

    let pokemonViewModel (pokemon : Pokemon) = {
        name = sprintf "#%d %s" pokemon.id (Utils.firstCharToUpper pokemon.name)
        image = Utils.modernImageSource <| Uri(pokemon.spriteUrl)
        height = (string (pokemon.height * 10)) + " cm"
        weight = (string ((float pokemon.weight) / 10.0)) + " kg"
        types = String.Join(", ", pokemon.types |> List.map (fun (PokemonType t) -> t) |> List.map Utils.firstCharToUpper)
    }
