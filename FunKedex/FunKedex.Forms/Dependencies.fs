namespace FunKedex.Forms

module Dependencies =
    let getAllPokemonsAsync = FunKedex.WebClient.getAllPokemons // wanna use mocks instead ? -> FunKedex.Domain.Pokemocks.getFakePokemonsAsync
