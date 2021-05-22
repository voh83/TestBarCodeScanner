// Copyright Fabulous contributors. See LICENSE.md for license.
namespace TestBarCodeScanner


open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms
open ZXing.Net.Mobile.Forms


module App = 

    type Model = 
      {      
        ScannedText:string}

    type Msg = 
        | ScannResult
        | SampleText of string

    let initModel = {ScannedText="--" }

    let init () = initModel, Cmd.none


    let update msg model =
        match msg with
        | SampleText smplText -> {model with ScannedText = (sprintf "Scan: %s" smplText)}, Cmd.none //Muestra texto 
 
    //Create a BarCode view using the Default configuration
    let barCodeScan disp = 
        let bcs = new ZXingScannerView()
        bcs.IsScanning <- true
        bcs.add_OnScanResult(
            ZXingScannerView.ScanResultDelegate( // Triggered when the scanning events happend 
                fun result -> printfn "%s" result.Text // print the scanned result text in the console log (can be reviewed with android device monitor DDMS
                              disp (SampleText (result.Text)) // Dispatch the Sample Text 
            ))
        bcs

    let view (model: Model) dispatch =
            //View.External(result)
           View.ContentPage(
              content = View.StackLayout(
                children = [ 
                    View.Frame(backgroundColor=Color.FromHex("#2196f3"), padding=Thickness.op_Implicit(24.0), cornerRadius=0.0, 
                        content = View.Label( 
                            text="Bar Code Example", 
                            horizontalTextAlignment=TextAlignment.Center, 
                            textColor = Color.White, 
                            fontSize = FontSize.fromValue(24.0)
                        )
                    )
                    View.Label(text=model.ScannedText, horizontalOptions = LayoutOptions.Center)
                    View.External(barCodeScan dispatch)
                    

                ]
              )
    ) 

    // Note, this declaration is needed if you enable LiveUpdate
    let program =
        XamarinFormsProgram.mkProgram init update view
        
#if DEBUG
        |> Program.withConsoleTrace
#endif

type App () as app = 
    inherit Application ()
    

    let runner = 
        App.program
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/tools.html#live-update for further  instructions.
    //
    //do runner.EnableLiveUpdate()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/models.html#saving-application-state for further  instructions.
#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


