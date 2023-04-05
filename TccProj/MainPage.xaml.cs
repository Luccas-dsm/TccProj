
using System;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Info;
using TccProj.Views.NFC;
using TccProj.Views.QrCode;
using Xamarin.Forms;

namespace TccProj
{
    public partial class MainPage : ContentPage
    {
        AppServices app = new AppServices();
        AppController appController = new AppController();
        InfoDispositivoModel Dispositivo { get; set; }
        public MainPage()
        {
            InitializeComponent();
            
            Login();
       
          //  Salvar();
            //Retorno();


        }
        public async void Login()
        {
            UsuarioModel Usuario = new UsuarioModel()
            {
                Seq= "-NS1asXbE9KnGJOaozXu",
                Email= "luccas@gmail.com",
                Senha = "1234"
            };
            Dispositivo = await appController.InformacoesDispositivo(Usuario.Seq);
        }

        public async void Salvar()
        {

            //var user = new UsuarioModel()
            //{
            //    Email = "luccas@gmail.com",
            //    Senha = "1234"
            //};         
            //var key = await app.SalvarUsuario(new UsuarioData(user));

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
            await Navigation.PushAsync(new NfcView(Dispositivo));
        }

        private async void InfoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoView(Dispositivo));
        }
    }
}
