using System;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Home;
using Xamarin.Forms;

namespace TccProj
{
    public partial class MainPage : ContentPage
    {
        AppServices AppServices = new AppServices();
        AppController appController = new AppController();
        InfoDispositivoModel Dispositivo { get; set; }
        public MainPage()
        {
            InitializeComponent();


        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(eEmail.Text) && !string.IsNullOrEmpty(ePass.Text))
            {
                var usuario = await AppServices.ValidaUsuario(eEmail.Text, ePass.Text);

                if (!string.IsNullOrEmpty(usuario))
                {
                    Dispositivo = await appController.InformacoesDispositivo(usuario);

                    await Navigation.PushAsync(new HomeView(Dispositivo));
                }
                else
                    await DisplayAlert("Ops!", "Usuário ou Senha está incorreto", "OK");
            }
            else
                await DisplayAlert("Ops!", "Usuário e a senha precisam estar preenchidos", "OK");
        }

        private async void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(eEmail.Text) && !string.IsNullOrEmpty(ePass.Text))
            {
                if (!await AppServices.ValidaEmailUsuario(eEmail.Text))
                {
                    UsuarioModel usuario = new UsuarioModel()
                    {
                        Email = eEmail.Text,
                        Senha = ePass.Text
                    };

                    var seqUsuario = await AppServices.SalvarUsuario(new UsuarioData(usuario));
                    await DisplayAlert("Obaa! Gente nova.", "Seja bem vindo!", "OK");
                    Dispositivo = await appController.InformacoesDispositivo(seqUsuario);
                    await Navigation.PushAsync(new HomeView(Dispositivo));
                }
                else
                {
                    await DisplayAlert("Ops!", "Usuário já Cadastrado", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ops!", "Usuário e a senha precisam estar preenchidos", "OK");
            }
        }
    }
}
