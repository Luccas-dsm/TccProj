
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
        UsuarioModel Usuario { get; set; }
        public MainPage()
        {
            InitializeComponent();
            
            Login();
            var dispositivo =  appController.InformacoesDispositivo(Usuario.Seq);
          //  Salvar();
            //Retorno();


        }
        public void Login()
        {
            Usuario = new UsuarioModel()
            {
                Seq= "-NS1asXbE9KnGJOaozXu",
                Email= "luccas@gmail.com",
                Senha = "1234"
            };

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
            await Navigation.PushAsync(new QrCodeView(Usuario));

        }
        private async void NfcBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NfcView(Usuario));
        }

        private async void InfoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoView(Usuario));
        }
    }
}
