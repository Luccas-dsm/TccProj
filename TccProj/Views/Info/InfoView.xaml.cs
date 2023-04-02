using TccProj.Models;
using TccProj.Views.Charts;
using TccProj.Views.Info.Components;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.Info
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoView : ContentPage
    {
        UsuarioModel Usuario { get; set; }
        public InfoView(UsuarioModel usuario)
        {
            InitializeComponent();
            PreencheInformacoes();
            this.Usuario = usuario;
        }

        private  void PreencheInformacoes()
        {
            var mock = new MockDados();

            foreach (var item in mock.PreencheDados())
            {
                switch (item.Titulo)
                {
                    case "CPU":             
                        CpuInfo.Children.Add(new BoxInfo(item.Titulo, item.Conteudo));
                        break;
                    case "RAM":
                        RamInfo.Children.Add(new BoxInfo(item.Titulo, item.Conteudo));
                        break;
                    case "SO":
                        SoInfo.Children.Add(new BoxInfo(item.Titulo, item.Conteudo));
                        break;
                    case "QRCode":
                        QrCodeInfo.Children.Add(new BoxInfo(item.Descricao, item.Conteudo));
                        break;
                    case "NFC":
                        NfcInfo.Children.Add(new BoxInfo(item.Descricao, item.Conteudo));
                        break;
                    case "Beacon":
                        BeaconInfo.Children.Add(new BoxInfo(item.Descricao, item.Conteudo));
                        break;
                }
            }
        }

        private void btnGraficos_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraficosView());
        }
    }
}