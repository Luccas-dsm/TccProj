using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TccProj.Views.QrCode;
using TccProj.Views.NFC;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TccProj.Views.Info;
using TccProj.Models;
using TccProj.Services;
using Xamarin.Forms.Internals;
using static Android.Telephony.CarrierConfigManager;
using Firebase.Auth;

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
            //Retorno();
         
            
        }
        public async void Salvar()
        {
           var user = new UsuarioModel();
            user = user.PreencheDados();
            Usuario = new UsuarioModel();
            Usuario.Seq = await app.SalvarUsuario(user);
        }
        public async void Retorno()
        {
               var resultado =  await app.BuscarUsuario(null, "Marcelly");

            Usuario = resultado;
            Teste.Text = Usuario.Nome;
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
