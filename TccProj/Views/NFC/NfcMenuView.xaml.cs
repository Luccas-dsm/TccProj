using TccProj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcMenuView : ContentPage
    {
        InfoDispositivoModel Dispositivo { get; set; }
        public NfcMenuView(InfoDispositivoModel dispositivo)
        {
            InitializeComponent();
            this.Dispositivo = dispositivo;
        }

        private async void btnNfcGravacao_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NfcGravarView(Dispositivo));
        }

        private async  void btnNfcLeitura_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NfcScanView(Dispositivo));

        }
    }
}