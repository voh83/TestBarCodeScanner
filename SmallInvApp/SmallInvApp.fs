// Copyright Fabulous contributors. See LICENSE.md for license.
namespace SmallInvApp


open Fabulous
open Fabulous.XamarinForms
open MobileBarodeScanner   
open Xamarin.Forms
open ZXing.Mobile
open ZXing.Net.Mobile.Forms


module App = 

    type Model = 
      {      
        ScannedText:string}

    type Msg = 
       (* | Increment 
        | Decrement 
        | Reset
        | SetStep of int
        | TimerToggled of bool
        | TimedTick *)
        | ScannResult
        | SampleText of string

    let initModel = {ScannedText="--" }

    let init () = initModel, Cmd.none


    let update msg model =
        match msg with
        | ScannResult -> {model with ScannedText = escanear().Text}, Cmd.none
        | SampleText smplText -> {model with ScannedText = (sprintf "Texto de Prueba: %s" smplText)}, Cmd.none
      (*  | Increment -> { model with Count = model.Count + model.Step }, Cmd.none
        | Decrement -> { model with Count = model.Count - model.Step }, Cmd.none
        | ScannResult res -> {model with ScannedText = (sprintf "Code: %s" res.Text)}, Cmd.none
        | Reset -> init ()
        | SetStep n -> { model with Step = n }, Cmd.none
        | TimerToggled on -> { model with TimerOn = on }, (if on then timerCmd else Cmd.none)
        | TimedTick -> 
            if model.TimerOn then 
                { model with Count = model.Count + model.Step }, timerCmd
            else 
                model, Cmd.none*)
    
   // let scanRsDel = ZXingScannerView.ScanResultDelegate(
     //   fun res -> {model with ScannedText = res.Text} |> ignore
    // *)

  
    //let timerTicker dispatch = 
     //   let timer = new System.Timers.Timer(5000.0)
     //   timer.Elapsed.Subscribe(fun _ -> dispatch (SampleText (System.DateTime.Now.Ticks.ToString()))) |> ignore
     //   timer.Enabled <- true
     //   timer.Start()
       
    let barCodeScan disp = 
        let bcs = new ZXingScannerView()
        bcs.IsScanning <- true
        bcs.add_OnScanResult(
            ZXingScannerView.ScanResultDelegate(
                fun result -> printfn "%s" result.Text
                              disp (SampleText (result.Text))
            ))
        bcs
   


    let view (model: Model) dispatch =
            //View.External(result)
           View.ContentPage(
              content = View.StackLayout(
                children = [ 
                    
                    (*View.Label(text = sprintf "%d" model.Count, horizontalOptions = LayoutOptions.Center, width=200.0, horizontalTextAlignment=TextAlignment.Center)
                    View.Button(text = "Increment", command = (fun () -> dispatch Increment), horizontalOptions = LayoutOptions.Center)
                    View.Button(text = "Decrement", command = (fun () -> dispatch Decrement), horizontalOptions = LayoutOptions.Center)
                    
                    View.Label(text = sprintf "Escaneo: %s" model.ScannedText, horizontalOptions = LayoutOptions.Center)
                    View.Label(text = "Timer", horizontalOptions = LayoutOptions.Center)
                    View.Switch(isToggled = model.TimerOn, toggled = (fun on -> dispatch (TimerToggled on.Value)), horizontalOptions = LayoutOptions.Center)
                    View.Slider(minimumMaximum = (0.0, 10.0), value = double model.Step, valueChanged = (fun args -> dispatch (SetStep (int (args.NewValue + 0.5)))), horizontalOptions = LayoutOptions.FillAndExpand)
                    View.Label(text = sprintf "Step size: %d" model.Step, horizontalOptions = LayoutOptions.Center) 
                    View.Button(text = "Reset", horizontalOptions = LayoutOptions.Center, command = (fun () -> dispatch Reset), commandCanExecute = (model <> initModel))
                    *)
                    //View.Button(text = "BarCode Scanner", command = (fun () -> dispatch Increment ) , horizontalOptions = LayoutOptions.Center)
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
                    //View.Button(text = "Escanear", horizontalOptions = LayoutOptions.Center, command = (fun () -> dispatch ScannResult))
                    

                ]
              )
    ) 

    // Note, this declaration is needed if you enable LiveUpdate
    let program =
        XamarinFormsProgram.mkProgram init update view
        //|> Program.withSubscription (fun _ -> Cmd.ofSub barCodeScan)
        //|> Program.withSubscription (fun _ -> Cmd.ofSub timerTicker )
        
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


