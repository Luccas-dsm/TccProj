using Autofac;
using System;
using TccProj.Services;
using TccProj.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj
{
    public partial class App : Application
    {
        static readonly ContainerBuilder builder = new ContainerBuilder();

        public App()
        {
            InitializeComponent();

           
            MainPage = new NavigationPage(new MainPage());

        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public static void RegisterType<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            builder.RegisterType<T>().As<TInterface>();
           // builder.RegisterType<IUsuarioService, UsuarioService>();
        }
    }
}
