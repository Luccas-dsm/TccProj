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
		public BoxInfo (string title, string description)
		{
			InitializeComponent ();

			titulo.Text = title;
			conteudo.Text = description;
			titulo.TextColor = Color.Black;
			conteudo.TextColor = Color.Black;
		}
	}
}