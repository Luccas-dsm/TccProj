using System.Collections.Generic;
using System.Linq;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Charts;
using TccProj.Views.Info.Components;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.Info
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoView : ContentPage
    {
        private InfoDispositivoModel Dispositivo { get; set; }
        private List<DadosModel> Dados { get; set; }
        private AppServices AppService = new AppServices();
        public InfoView(InfoDispositivoModel dispositivo)  
        {
            InitializeComponent();
            this.Dispositivo = dispositivo;
            //DadosTestes();
            //PreencheInformacoes();
        }
        private async void DadosTestes() => Dados = await AppService.BuscarTestePeloDispositivo(Dispositivo.Seq);

/*
 
        private void PreencheInformacoes()
        {
            var mock = new MockDados();

            foreach (var item in mock.PreencheDados())
            {
                switch (item.Titulo)
                {
                    case "CPU":
                        CpuInfo.Children.Add(new BoxInfo("CPU", Dispositivo.CPU));
                        break;
                    case "RAM":
                        RamInfo.Children.Add(new BoxInfo("RAM", Dispositivo.MemoriaRam));
                        break;
                    case "SO":
                        SoInfo.Children.Add(new BoxInfo("SO", Dispositivo.SistemaOperacional));
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
        }*/

        private void btnGraficos_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraficosView());
        }
    }
}