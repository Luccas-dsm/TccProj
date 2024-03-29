﻿
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot;
using System.Collections.Generic;
using TccProj.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OxyPlot.Xamarin.Forms;
using System.Linq;

namespace TccProj.Views.Charts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraficosView : ContentPage
    {
        public List<TramentoDeDadosModel> DadosTratados { get; set; }

        public GraficosView(List<TramentoDeDadosModel> dadosTratados)
        {
            this.DadosTratados = dadosTratados;
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
                Title = $"{DadosTratados.FirstOrDefault().Tecnologia}",
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

            var categoria = new CategoryAxis { Position = AxisPosition.Left };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0
            };

            DadosTratados.ForEach(f =>
            {
                var bar = new BarSeries { Title = f.Tipo };
                bar.Items.Add(new BarItem { Value = f.Media });

                model.Series.Add(bar);

            });
            categoria.Labels.Add("Tempo de resposta");
            model.Axes.Add(categoria);
            model.Axes.Add(valueAxis);

            return model;
        }
 
    }
}