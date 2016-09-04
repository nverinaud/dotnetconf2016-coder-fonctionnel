# Démo Coder Fonctionnel - dotNetConf 2016 Edition

Code source de la démo de ma présentation "Coder F#nctionnel" pour l'édition dotNetConf 2016 à Strasbourg.

Voir la présentation: [dotnetconf2016-coder-fonctionnel.nverinaud.com](http://dotnetconf2016-coder-fonctionnel.nverinaud.com)

## Instruction d'installation

- Installer MySQL (non nécessaire si vous voulez tester les apps mobiles avec les mocks)
    * user: root
    * pass: root
    * port: 3306
    * server: localhost
    * créer une base "pokemons" vide
    * tout cela est modifiable, pour cela changez simplement la connection string dans le projet MySQL et le script `:)`
- Ouvrir la solution "FunKedex.sln" et restaurer les packets NuGet
- Lancer le script "FunKedex.Scripts/ImportFromPokeAPI.fsx"
    * télécharge les données de PokeAPI et les insert dans MySQL
- Builder le projet `FunKedex.WebAPI` et lancer le serveur avec votre terminal / console
	* Mono: `mono FunKedex/FunKedex.WebAPI/bin/Debug/FunKedex.WebAPI.exe`
	* Win : `FunKedex\FunKedex.WebAPI\bin\Debug\FunKedex.WebAPI.exe`
- Lancer les apps mobiles et enjoy le monde merveilleux des vrais Pokémons ayant jamais existés !
        
## License

MIT