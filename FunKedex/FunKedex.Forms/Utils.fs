namespace FunKedex.Forms

module Utils =
    open System
    open System.Net.Http
    open ModernHttpClient
    open Xamarin.Forms

    let firstCharToUpper s =
        match s with
        | "" | null -> 
            ""
        | _ ->
            s |> String.mapi (fun i c -> match i with
                                         | 0 -> ((c.ToString()).ToUpper()).[0]
                                         | _ -> c)

    let modernImageSource (uri : Uri) =
        ImageSource.FromStream ((fun () ->
            let httpClient = new HttpClient(new NativeMessageHandler())
            httpClient.GetStreamAsync (uri) 
            |> Async.AwaitTask 
            |> Async.RunSynchronously
        ))