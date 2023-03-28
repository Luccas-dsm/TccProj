using System;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Info;
using TccProj.Views.NFC;
using TccProj.Views.QrCode;
using Xamarin.Forms;

namespace TccProj.Views.Inicio
{
    public partial class InicioView : ContentPage
    {
        AppServices app = new AppServices();

        UsuarioModel Usuario;
        public InicioView()
        {
            InitializeComponent();
            //Salvar();
           // Update();
         //   Retorno();


        }
        //public async void Update()
        //{
        //    var user = new UsuarioModel();
        //   // user = user.PreencheDados();
        //    Usuario = new UsuarioModel();
        //    Usuario.Nome = await app.UpdateUsuario("Luccas");
        //}
        //public async void Salvar()
        //{
        //    var user = new UsuarioModel();
        //    user = user.PreencheDados();
        //    Usuario = new UsuarioModel();
        //    Usuario.Seq = await app.SalvarUsuario(user);
        //}
        //public async void Retorno()
        //{
        //    var resultado = await app.BuscarUsuario(null, "Marcelly");

        //    Usuario = resultado;
        //    Teste.Text = Usuario.Nome;
        //}

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
