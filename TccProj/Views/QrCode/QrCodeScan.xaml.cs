﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
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
        InfoDispositivoModel Dispositivo;
        AppServices AppService = new AppServices();
        AppController AppController = new AppController();

        public event EventHandler<string> BarcodeReaded;

        DadosModel Dados;

        public QrCodeScan(InfoDispositivoModel infoDispositivo)
        {
            this.Dispositivo = infoDispositivo;

            Dados = AppController.PreencheEscanearQrCode();
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

            Dados.SeqInfoDispositivo = Dispositivo.Seq;
            Stopwatch stopwatch = new Stopwatch();
            //incio da leitura
            zxing.OnScanResult += (result) =>
            {
                stopwatch.Start();
                double memoryAfter;
                double memoryBefore;
                Device.BeginInvokeOnMainThread(async () =>
                {

                    // Para a analise
                    zxing.IsAnalyzing = false;
                    Dados.Tamanho = result.NumBits;
                    memoryBefore = GC.GetTotalMemory(true);
                    BarcodeReaded?.Invoke(this, result.Text);
                    memoryAfter = GC.GetTotalMemory(true);
                    Dados.UsoMemoria = memoryAfter - memoryBefore;
                    MensagemResuldado(result.Text);

                });


                stopwatch.Stop();
                double ticks = stopwatch.ElapsedTicks;
                double seconds = stopwatch.Elapsed.TotalSeconds;

                var frequenciaHz = AppController.ConversaoDeFrequencia(ticks, seconds);
                Dados.UsoCpu = AppController.TransoformarHzEmGhz(frequenciaHz); // divide por 1 bilhão para converter para GHz

                Dados.TempoResposta = stopwatch.Elapsed;
                _ = AppService.SalvarTeste(new DadosData(Dados));

            };
            //fim da leitura

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

        private async void MensagemResuldado(string resultado)
        {
            await DisplayAlert("Resultado:", resultado, "Ok");
            await Navigation.PopModalAsync(); // retorna para a tela main
        }
    }
}