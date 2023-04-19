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
        public static UsuarioModel Usuario { get; set; }
        public List<TramentoDeDadosModel> DadosTratados { get; set; }




        private List<Entry> Entradas()
        {
            List<Entry> entradas = new List<Entry>();
            DadosTratados.ForEach(f =>
            {
                entradas.Add(new Entry((float)f.Media)
                {
                    Color = SKColor.Parse("#FF1943"),
                    Label = f.Tipo,
                    ValueLabel = f.Media.ToString("N4"),
                });
            });
            return entradas;
        }

        public GraficosView(List<TramentoDeDadosModel> dadosTratados)
        {
            this.DadosTratados = dadosTratados;
            InitializeComponent();

            Grafico1.Chart = new BarChart()
            {
                Entries = Entradas(),
                LabelOrientation = Orientation.Horizontal,
                LabelTextSize = 36,
                ValueLabelOrientation = Orientation.Horizontal,


            };
            SKBitmap img = new SKBitmap();
            SKCanvas Surface = new SKCanvas(img) { };
            Grafico1.Chart.Draw(Surface, 200, 400);
        }
    }
}