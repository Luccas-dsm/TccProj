using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Info;
using TccProj.Views.NFC;
using TccProj.Views.QrCode;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        AppServices app = new AppServices();
        AppController appController = new AppController();
        InfoDispositivoModel Dispositivo { get; set; }
        public HomeView(InfoDispositivoModel dispositivo)
        {
            this.Dispositivo = dispositivo;
            InitializeComponent();

            Login();

        }
        public async void Login()
        {
            UsuarioModel Usuario = new UsuarioModel()
            {
                Seq = "-NS1asXbE9KnGJOaozXu",
                Email = "luccas@gmail.com",
                Senha = "1234"
            };
            Dispositivo = await appController.InformacoesDispositivo(Usuario.Seq);
        }

        public async void Salvar()
        {
            var dispositivo = new InfoDispositivoModel()
            {
                CPU = "adreno",
                Fabricante = "Samsung",
                MemoriaRam = "6gb",
                Modelo = "SM-760G",
                PossuiNFC = true,
                SeqUsuario = "XXX",
                SistemaOperacional = "Android",
                Seq = "X"
            };

            var key = await app.SalvarDispositivo(new InfoDispositivoData(dispositivo));

        }

        private async void QrBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodeView(Dispositivo));

        }
        private async void NfcBtn_Clicked(object sender, EventArgs e)
        {
            if (!CrossNFC.Current.IsAvailable)
            {
                DisplayAlert("Ops!", "O seu dispositivo não possui a tecnologia NFC", "OK");

            }
            else
            {
                await Navigation.PushAsync(new NfcMenuView(Dispositivo));
            }
        }

        private async void InfoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoView(Dispositivo));
        }
    }
}