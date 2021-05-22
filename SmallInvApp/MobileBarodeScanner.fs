module MobileBarodeScanner

open ZXing.Mobile
open ZXing.Net.Mobile.Forms

let scanner = new MobileBarcodeScanner()
scanner.CameraUnsupportedMessage <- "Camara no soportada Aweonao"
scanner.BottomText <- "Escaner y la contechetumare..."

let escanear() = 
    async{
         let! resultado = scanner.Scan() |> Async.AwaitTask
         return resultado} |> Async.RunSynchronously
