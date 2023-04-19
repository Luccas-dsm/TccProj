using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.Info.Components
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxInfo : ContentView
    {
		public BoxInfo (string title, string description, InfoView view)
		{
			InitializeComponent ();

            MessagingCenter.Send<ContentView>(this, DisplayAlertMessage);

            titulo.Text = title;
			conteudo.Text = description;
			titulo.TextColor = Color.Black;
			conteudo.TextColor = Color.Black;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                await view.DisplayAlert("Informação", $"O {titulo.Text} se refere a média de todos os tempos de resposta referentes a {titulo.Text} ", "OK");
            };


            stackPrincipal.GestureRecognizers.Add(tapGestureRecognizer);
        }
        public static readonly string DisplayAlertMessage = "DisplayAlertMessage";


    }
}