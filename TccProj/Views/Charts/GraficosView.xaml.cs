
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using TccProj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;


namespace TccProj.Views.Charts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraficosView : ContentPage
    {
        //public static UsuarioModel Usuario { get; set; } = new UsuarioModel().PreencheDados();



        //List<Entry> entradas = new List<Entry>
        //{
        //    new Entry(Usuario.InfoDispositivo[0].DadosTeste[0].UsoCpu)
        //    {
        //        Color=SKColor.Parse("#FF1943"),
        //        Label = Usuario.InfoDispositivo[0].Fabricante,
        //        ValueLabel = Usuario.InfoDispositivo[0].DadosTeste[0].UsoCpu.ToString()
        //    },
        //      new Entry(Usuario.InfoDispositivo[1].DadosTeste[0].UsoCpu)
        //    {
        //        Color=SKColor.Parse("#000"),
        //        Label = Usuario.InfoDispositivo[1].Fabricante,
        //        ValueLabel = Usuario.InfoDispositivo[1].DadosTeste[0].UsoCpu.ToString()
        //    },

        // };

        public GraficosView()
        {
            InitializeComponent();


            //Grafico1.Chart = new BarChart()
            //{
            //    Entries = entradas,
            //    LabelOrientation = Orientation.Horizontal,
            //    LabelTextSize = 36,
            //    ValueLabelOrientation = Orientation.Horizontal,


            //};
            //SKBitmap img = new SKBitmap();
            //SKCanvas Surface = new SKCanvas(img) { };
            //Grafico1.Chart.Draw(Surface, 200, 400);
        }
    }
}