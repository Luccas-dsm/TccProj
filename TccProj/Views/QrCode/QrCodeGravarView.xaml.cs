using Android.App;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode.Internal;

namespace TccProj.Views.QrCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeGravarView : ContentPage
    {
        ZXingBarcodeImageView barcode;
        public QrCodeGravarView()
        {
            InitializeComponent();

            barcode = GeradorQr(text.Text);
            text.TextChanged += Text_TextChanged;
            void Text_TextChanged(object sender, TextChangedEventArgs e) // faz o binding do texto digitado pelo usuário
                                    => barcode.BarcodeValue = e.NewTextValue;

            stackPrincipal.Children.Add(barcode);



        }

        private void btnAnexar_Clicked(object sender, EventArgs e)
        {
            //CompartilharImagem(barcode.BarcodeValue);
            PegarCaminhoArquivo();



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