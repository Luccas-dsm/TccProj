
using Microcharts;

using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;


namespace TccProj.Views.Charts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraficosView : ContentPage
    {
        List<Entry> entradas = new List<Entry>
        {
            new Entry(200)
            {
                Color=SKColor.Parse("#FF1943"),
                Label ="Janeiro",
                ValueLabel = "200"
            },
            new Entry(400)
            {
                Color = SKColor.Parse("00BFFF"),
                Label = "Abril",
                ValueLabel = "400"
            },
            new Entry(-80)
            {
                Color =  SKColor.Parse("#00CED1"),
                Label = "Setembro",
                ValueLabel = "-80"
            },
            new Entry(-200)
            {
                Color =  SKColor.Parse("#00CED1"),
                Label = "Novembro",
                ValueLabel = "-200"
            },
         };
        
        public GraficosView()
        {
            InitializeComponent();
            Grafico1.Chart = new LineChart()
            {
                Entries = entradas
            };

        }
    }
}