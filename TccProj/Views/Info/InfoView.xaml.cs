using System.Collections.Generic;
using System.Linq;
using TccProj.Controller;
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
        private InfoDispositivoModel Dispositivo { get; set; }
        private List<TramentoDeDadosModel> TratamentoQrCode { get; set; }
        private List<TramentoDeDadosModel> TratamentoNfc { get; set; }
        private AppController AppController = new AppController();
        public InfoView(InfoDispositivoModel dispositivo)
        {
            InitializeComponent();
            this.Dispositivo = dispositivo;

            PreencheInformacoes();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NfcInfo.GestureRecognizers.Clear();
            QrCodeInfo.GestureRecognizers.Clear();


            if (!Dispositivo.PossuiNFC)
                frameNfc.BackgroundColor = Color.FromHex("#F8F8F8");

            NfcInfo.GestureRecognizers.Add(btnChart_NFC());
            QrCodeInfo.GestureRecognizers.Add(btnChart_QrCode());
        }

        private TapGestureRecognizer btnChart_NFC()
        {

            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                if (Dispositivo.PossuiNFC)
                {
                    await Navigation.PushModalAsync(new GraficosView(TratamentoNfc));
                }
                else
                {
                    await DisplayAlert("Ops!", "Este dispositivo não possui NFC", "OK");
                }
            };
            return tapGestureRecognizer;
        }
        private TapGestureRecognizer btnChart_QrCode()
        {

            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await Navigation.PushModalAsync(new GraficosView(TratamentoQrCode));
            };
            return tapGestureRecognizer;
        }

        private void PreencheInformacoes()
        {
            var listaDados = AppController.TratamentoDeDados(Dispositivo).Result;

            TratamentoNfc = listaDados.Where(x => x.Tecnologia.Equals("NFC")).ToList();
            TratamentoQrCode = listaDados.Where(x => x.Tecnologia.Equals("QrCode")).ToList();

            CpuInfo.Children.Add(new BoxInfo("CPU", Dispositivo.CPU, this));
            RamInfo.Children.Add(new BoxInfo("RAM", Dispositivo.MemoriaRam, this));
            SoInfo.Children.Add(new BoxInfo("SO", Dispositivo.SistemaOperacional, this));
            if (listaDados.Count() > 0)
            {
                foreach (var item in listaDados)
                {
                    switch (item.Tecnologia)
                    {

                        case "QrCode":
                            QrCodeInfo.Children.Add(new BoxInfo(item.Tipo, string.Format(item.Media.ToString("N6") + " seg"), this));

                            break;
                        case "NFC":
                            if (Dispositivo.PossuiNFC)
                                NfcInfo.Children.Add(new BoxInfo(item.Tipo, string.Format(item.Media.ToString("N6") + " seg"), this));
                            else
                                NfcInfo.Children.Add(new BoxInfo(item.Tipo,"Não Possui", this));
                            break;

                    }
                }
            }
        }

        private void btnGraficos_Clicked(object sender, System.EventArgs e)
        {
            if (!Dispositivo.PossuiNFC)
                DisplayAlert("Atenção!", "Este dispositivo não possui a tecnologia NFC, os dados sobre essa tecnologia serão igual a 0", "OK");

            Navigation.PushAsync(new GraficoComparativoView(TratamentoQrCode.OrderBy(o => o.Tipo).ToList(), TratamentoNfc.OrderBy(o => o.Tipo).ToList()));
        }
    }
}