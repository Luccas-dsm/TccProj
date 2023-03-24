﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TccProj.Views.QrCode;
using TccProj.Views.NFC;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TccProj.Views.Info;

namespace TccProj
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
        private async void QrBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodeView());

        }
        private async void NfcBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NfcView());
        }

        private async void InfoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoView());
        }
    }
}
