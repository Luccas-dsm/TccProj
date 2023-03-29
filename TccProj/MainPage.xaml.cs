
using System;
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

        UsuarioModel Usuario;
        public MainPage()
        {
            InitializeComponent();
            Salvar();
            Retorno();


        }
        public async void Salvar()
        {
            var user = new UsuarioModel()
            {
                Email = "teste1@email.com",
                Senha = "1234"
            }; 
            //user = user.PreencheDados();
           
            var key = await app.SalvarUsuario(user);
        }
        public async void Retorno()
        {
            var resultado = await app.BuscarUsuario(null, "teste1@email.com");

            Usuario = resultado;
            Teste.Text = Usuario.Email;
        }

        private async void QrBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodeView());

        }
        private async void NfcBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NfcView());
        }

        private async void InfoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoView());
        }
    }
}
