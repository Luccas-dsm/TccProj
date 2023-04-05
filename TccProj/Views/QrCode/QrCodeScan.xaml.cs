using Java.Time.Temporal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace TccProj.Views.QrCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeScan : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        public event EventHandler<string> BarcodeReaded;

        public QrCodeScan()
        {
            InitializeComponent();
            //Opções de Leitura
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                PossibleFormats = new List<ZXing.BarcodeFormat>
                {
                    ZXing.BarcodeFormat.QR_CODE//ZXing.BarcodeFormat.EAN_8, ZXing.BarcodeFormat.EAN_13
                }
            };

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Options = options
            };

            long tamanho = 0;
            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            //incio da leitura
            zxing.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(async () =>
            {

                // Para a analise
                zxing.IsAnalyzing = false;
                tamanho = result.NumBits;
                    BarcodeReaded?.Invoke(this, result.Text);

                await Navigation.PopModalAsync(); // retorna para a tela main

            });

            //fim da leitura

            stopwatch.Stop();


            var x = stopwatch.Elapsed;

        //    Console.WriteLine("Tempo: " + stopwatch. + " ms");
            overlay = new ZXingDefaultOverlay
            {
                TopText = "Escolhe um QRCode para leitura",
                BottomText = "O Código sera lido automaticamente",
                //ShowFlashButton = zxing.HasTorch, //Lanterna
            };
       
            var abort = new Button
            {
                Text = "Cancelar",
                VerticalOptions = LayoutOptions.End,
                TextColor = Color.FromHex("#FFF"),
                BackgroundColor = Color.FromHex("#4F51FF")
            };

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    abort.HeightRequest = 40;
                    break;
                case Device.Android:
                    abort.HeightRequest = 50;
                    break;
            }

            abort.Clicked += (object s, EventArgs e) =>
            {
                zxing.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                });
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            grid.Children.Add(zxing);
            grid.Children.Add(overlay);
            grid.Children.Add(abort);

            Content = grid;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;

            base.OnDisappearing();
        }
    }
}