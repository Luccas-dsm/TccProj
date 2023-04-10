using System;
using TccProj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace TccProj.Views.QrCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeView : ContentPage
    {
        InfoDispositivoModel Dispositivo { get; set; }

        public event EventHandler<string> BarcodeReaded;
        public QrCodeView(InfoDispositivoModel dispositivo)
        {
            InitializeComponent();
            this.Dispositivo = dispositivo;
        }
        void ZXingView_BarcodeReaded(object sender, string e)
        {
            //   lblResultado.Text = "QRCODE: " + e;
        }


        private async void btnQrcodeLeitura_Clicked(object sender, EventArgs e)
        {

            QrCodeScan partial = new QrCodeScan(Dispositivo);
            partial.BarcodeReaded += ZXingView_BarcodeReaded;
            await Navigation.PushModalAsync(partial);

        }

        private async void btnQrcodeGravacao_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new QrCodeGravarView());
        }
    }
}
