namespace FunKedex.Forms

open System
open System.Collections.ObjectModel
open Xamarin.Forms
open FunKedex
open FunKedex.Domain
open FunKedex.Forms.ViewModels

type DetailPage (vm) =
    inherit ContentPage ()

    let createLayoutWithLabels label value =
        let heightLabel = Label(Text = label, HorizontalTextAlignment = TextAlignment.End, WidthRequest = (float 130), FontAttributes = FontAttributes.Bold)
        let heightValueLabel = Label(Text = value)
        let heightLayout = StackLayout(Orientation = StackOrientation.Horizontal)
        heightLayout.Children.Add (heightLabel)
        heightLayout.Children.Add (heightValueLabel)
        heightLayout

    do
        let layout = new RelativeLayout(BackgroundColor = Color.FromRgb(223, 198, 135))
        let box = BoxView(Color = Color.FromRgb(212, 238, 247))
        layout.Children.Add(box
            , Constraint.Constant(float 0)
            , Constraint.Constant(float 0)
            , Constraint.RelativeToParent((fun parent -> parent.Width))
            , Constraint.Constant(float 100)
        )
        let image = Image(Aspect = Aspect.AspectFit)
        image.Source <- vm.image
        layout.Children.Add(image
            , Constraint.RelativeToView(box, (fun parent b -> b.Width / (float 2) - (float 64)))
            , Constraint.RelativeToView(box, (fun parent b -> b.Height - (float 64)))
            , Constraint.Constant(float 128)
            , Constraint.Constant(float 128)
        )
        let heightLayout = createLayoutWithLabels "Taille:" vm.height
        let weightLayout = createLayoutWithLabels "Poids:" vm.weight
        let typesLayout = createLayoutWithLabels "Types:" vm.types
        let infosLayout = StackLayout(Orientation = StackOrientation.Vertical)
        infosLayout.Children.Add(heightLayout)
        infosLayout.Children.Add(weightLayout)
        infosLayout.Children.Add(typesLayout)
        layout.Children.Add(infosLayout
            , Constraint.Constant(float 0)
            , Constraint.RelativeToView(image, (fun parent v -> v.Y + v.Height))
            , Constraint.RelativeToParent((fun parent -> parent.Width))
            , Constraint.RelativeToParent((fun parent -> parent.Height))
        )
        base.Title <- vm.name
        base.Content <- layout

type SummaryPage () =
    inherit ContentPage ()

    let pokemonsList = new ObservableCollection<PokemonViewModel> ()
    let listView = ListView (RowHeight = 64)
    let stack = StackLayout (VerticalOptions = LayoutOptions.FillAndExpand)

    let loadPokemons () = Async.StartImmediate <| async {
        let! vms = async {
            let! pokemons = Dependencies.getAllPokemonsAsync ()
            return pokemons |> List.map pokemonViewModel
        }
        vms |> List.iter pokemonsList.Add
    }

    let configureListView onItemTapped =
        listView.ItemTemplate <- new DataTemplate(typeof<ImageCell>)
        listView.ItemTemplate.SetBinding(ImageCell.TextProperty, Binding.Create<PokemonViewModel>(fun vm -> vm.name :> obj))
        listView.ItemTemplate.SetBinding(ImageCell.ImageSourceProperty, Binding.Create<PokemonViewModel>(fun vm -> vm.image :> obj))
        listView.ItemsSource <- pokemonsList
        listView.ItemTapped.Subscribe (fun x ->
            match x.Item with
            | :? PokemonViewModel as vm ->
                onItemTapped vm
            | _ -> 
                ()
        ) |> ignore

    do
        let nav = base.Navigation
        configureListView (fun vm ->
            nav.PushAsync (new DetailPage(vm)) |> ignore
            ()
        )
        stack.Children.Add (listView)
        base.Content <- stack
        base.Title <- "FunKédex"
        loadPokemons ()

    override this.OnAppearing () =
        base.OnAppearing ()
        listView.SelectedItem <- null
        
type App () = 
    inherit Application ()

    do
        base.MainPage <- new NavigationPage (new SummaryPage ())

