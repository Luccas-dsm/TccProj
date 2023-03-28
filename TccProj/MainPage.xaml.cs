using System;
using TccProj.Models;
using TccProj.Services;
using TccProj.Views.Inicio;
using Xamarin.Forms;

namespace TccProj
{
    public partial class MainPage : ContentPage
    {
        AppServices AppService = new AppServices();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (await AppService.ValidaUsuario(eEmail.Text, ePass.Text))
                await Navigation.PushAsync(new InicioView());
            else
                await DisplayAlert("Ops!", "Usuário ou Senha está incorreto", "OK");



        }

        private async void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(eEmail.Text) && !string.IsNullOrEmpty(ePass.Text))
            {
                if (!await AppService.ValidaEmailUsuario(eEmail.Text))
                {
                    UsuarioModel usuario = new UsuarioModel()
                    {
                        Email = eEmail.Text,
                        Senha = ePass.Text
                    };

                    await AppService.SalvarUsuario(usuario);
                    await DisplayAlert("Obaa! Gente nova.", "Seja bem vindo!", "OK");
                    await Navigation.PushAsync(new InicioView());
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
