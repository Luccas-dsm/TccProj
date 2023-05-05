using Android.App;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode.Internal;

namespace TccProj.Views.QrCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeGravarView : ContentPage
    {
        ZXingBarcodeImageView barcode;
        InfoDispositivoModel Dispositivo;
        DadosModel Dados;

        AppController AppController = new AppController();
        AppServices AppService = new AppServices();

        public QrCodeGravarView(InfoDispositivoModel infoDispositivo)
        {
            this.Dispositivo = infoDispositivo;
            InitializeComponent();
            Dados = AppController.PreencheGravarQrCode();
            Dados.SeqInfoDispositivo = Dispositivo.Seq;
        }

        private void btnGerar_Clicked(object sender, EventArgs e)
        {

      
                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                double memoryBefore = GC.GetTotalMemory(true);
                barcode = GeradorQr(text.Text);
                double memoryAfter = GC.GetTotalMemory(true);

                Dados.UsoMemoria = (Math.Abs(memoryAfter - memoryBefore));
                Dados.Tamanho = Encoding.UTF8.GetByteCount(barcode.BarcodeValue);

                stopwatch.Stop();
                double ticks = stopwatch.ElapsedTicks;
                double seconds = stopwatch.Elapsed.TotalSeconds;

                var frequenciaHz = AppController.ConversaoDeFrequencia(ticks, seconds);
                Dados.UsoCpu = AppController.TransoformarHzEmGhz(frequenciaHz); // divide por 1 bilhão para converter para GHz

                Dados.TempoResposta = stopwatch.Elapsed;

                _ = AppService.SalvarTeste(new DadosData(Dados));
                stackQrCode.Children.Clear();
                stackQrCode.Children.Add(barcode);
            
        }

        public ZXingBarcodeImageView GeradorQr(string conteudo, int width = 300, int height = 300, int margin = 10)
        {
            ZXingBarcodeImageView barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView",
            };

            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = width;
            barcode.BarcodeOptions.Height = height;
            barcode.BarcodeOptions.Margin = margin;
            barcode.BarcodeValue = conteudo;

            return barcode;
        }

        private async void CompartilharImagem(string imagem)
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share QrCode",
                File = new ShareFile(imagem, "image/png")
            });
        }

        private string PegarCaminhoArquivo()
        {
            string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string caminhoArquivo = Path.Combine(pasta, "exemplo.txt");
            string conteudo = "";

            if (File.Exists(caminhoArquivo))
            {
                using (StreamReader leitor = new StreamReader(caminhoArquivo))
                {
                    conteudo = leitor.ReadToEnd();
                }
            }
            return conteudo;
        }

    }
}