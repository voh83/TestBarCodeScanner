module BarcodeSettings

open ZXing.Mobile
open ZXing.Net.Mobile.Forms
open System.Collections.Generic


let result = 

    let barcodeList = new List<ZXing.BarcodeFormat> ([
        ZXing.BarcodeFormat.EAN_13;
        ZXing.BarcodeFormat.EAN_8;
        ZXing.BarcodeFormat.QR_CODE
    ])  
      
    let options = new MobileBarcodeScanningOptions()
    options.PossibleFormats <- barcodeList

    let overlay = new ZXingDefaultOverlay()
    overlay.ShowFlashButton <- false
    overlay.TopText <- "Situar Escaner sobre el codigo"
    overlay.BottomText <- "Se escaneara automaticamente el codigo"
    overlay.Opacity <- 0.75
    overlay.BindingContext <- overlay

    let scannerPage = new ZXingScannerPage(options)
    scannerPage.IsScanning <- true
    scannerPage.add_OnScanResult(
       ZXingScannerPage.ScanResultDelegate(
            fun res ->  printfn "SmallInvApp Escaneo: %s" (res.Text)
                        
       ) 
    ) 
   

 
    
    
    
    





    

