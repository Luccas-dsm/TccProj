using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TccProj.Models;
using ZXing.QrCode.Internal;

namespace TccProj.Views.Charts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraficoComparativoView : ContentPage
	{
        public List<TramentoDeDadosModel> DadosTratadosQrCode { get; set; }
        public List<TramentoDeDadosModel> DadosTratadosNfc { get; set; }

        public GraficoComparativoView(List<TramentoDeDadosModel> dadosTratadosQrCode, List<TramentoDeDadosModel> dadosTratadosNfc)
        {
            this.DadosTratadosQrCode = dadosTratadosQrCode;
            this.DadosTratadosNfc = dadosTratadosNfc;
            InitializeComponent();
            Navigation.RemovePage(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

    

            var plotView = new PlotView
            {
                Model = GraficoBarras(),
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
            };

            flexGrafico.Children.Add(plotView);
        }
        private PlotModel GraficoBarras() //barra
        {
            var model = new PlotModel
            {
                Title = $"QrCode X Nfc",
                Legends =
                {
                    new Legend
                    {
                        LegendPlacement = LegendPlacement.Outside,
                        LegendPosition = LegendPosition.BottomCenter,
                        LegendOrientation = LegendOrientation.Horizontal,
                        LegendBorderThickness = 0,
                    }
                },
                DefaultFontSize = 14,
            };
            var qrCode = new BarSeries() { Title = "QrCode" }; 
            var nfc = new BarSeries() { Title = "NFC" }; 
            var categoria = new CategoryAxis { Position = AxisPosition.Left };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0
            };
      
            DadosTratadosQrCode.ForEach(f =>
            {
                qrCode.Items.Add(new BarItem { Value = f.Media});
            });

            DadosTratadosNfc.ForEach(f =>
            {
                nfc.Items.Add(new BarItem { Value = f.Media });
            });

            model.Series.Add(qrCode);
            model.Series.Add(nfc);
            categoria.Labels.Add("Escanner");
            categoria.Labels.Add("Gravação");
            model.Axes.Add(categoria);
            model.Axes.Add(valueAxis);

            return model;
        }

    }
}