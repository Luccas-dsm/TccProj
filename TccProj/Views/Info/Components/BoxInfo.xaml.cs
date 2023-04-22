using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.Info.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxInfo : ContentView
    {
        public BoxInfo(string title, string description, InfoView view)
        {
            InitializeComponent();

            MessagingCenter.Send<ContentView>(this, DisplayAlertMessage);

            titulo.Text = title;
            conteudo.Text = description;
            titulo.TextColor = Color.Black;
            conteudo.TextColor = Color.Black;


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                switch (titulo.Text)
                {
                    case "CPU":
                        await DisplayCPU(view);
                        break;
                    case "RAM":
                        await DisplayRAM(view);
                        break;
                    case "SO":
                        await DisplaySO(view);
                        break;
                    default:
                        await view.DisplayAlert("Informação", $"O campo {titulo.Text} se refere a média de todos os tempos de resposta referentes a {titulo.Text} ", "OK");
                        break;
                }
            };

            stackPrincipal.GestureRecognizers.Add(tapGestureRecognizer);


        }

        public static readonly string DisplayAlertMessage = "DisplayAlertMessage";

        public async System.Threading.Tasks.Task DisplayCPU(InfoView view) => await view.DisplayAlert("Informação", $"Essa informação é refente a base da CPU.", "OK");
        public async System.Threading.Tasks.Task DisplayRAM(InfoView view) => await view.DisplayAlert("Informação", $"Essa informação é refente a quantidade total de memoria Ram nativa do dispositivo.", "OK");
        public async System.Threading.Tasks.Task DisplaySO(InfoView view) => await view.DisplayAlert("Informação", $"Essa informação é refente a versão do sistema operacional utilizado.", "OK");


    }
}