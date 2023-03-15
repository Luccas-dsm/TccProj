using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace TccProj.Views.QrCode
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeView : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        public event EventHandler<string> BarcodeReaded;

        public QrCodeView()
        {
            InitializeComponent();
            btnQrcode.Clicked += async (sender, e) =>
            {

                QrCodeScan partial = new QrCodeScan();
                partial.BarcodeReaded += ZXingView_BarcodeReaded;

                await Navigation.PushModalAsync(partial);

            };
        }
        void ZXingView_BarcodeReaded(object sender, string e)
        {
            lblResultado.Text = "QRCODE: " + e;
        }

    }
}
