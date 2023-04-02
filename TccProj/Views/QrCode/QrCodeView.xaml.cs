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
        UsuarioModel Usuario { get; set; }

        public event EventHandler<string> BarcodeReaded;
        public QrCodeView(UsuarioModel usuario)
        {
            InitializeComponent();
            this.Usuario = usuario;
        }
        void ZXingView_BarcodeReaded(object sender, string e)
        {
            //   lblResultado.Text = "QRCODE: " + e;
        }


        private async void btnQrcodeLeitura_Clicked(object sender, EventArgs e)
        {

            QrCodeScan partial = new QrCodeScan();
            partial.BarcodeReaded += ZXingView_BarcodeReaded;
            await Navigation.PushModalAsync(partial);

        }

        private void btnQrcodeGravacao_Clicked(object sender, EventArgs e)
        {

        }
    }
}
