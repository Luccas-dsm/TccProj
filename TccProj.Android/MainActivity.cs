using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Plugin.NFC;
using TccProj.Views.NFC;
using Xamarin.Forms;

namespace TccProj.Droid
{
    [Activity(Label = "TccProj", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = NfcView.MIME_TYPE)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossNFC.Init(this);
            //ZXing Inicialização
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            LoadApplication(new App());

            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.Gray },
                        new Setter { Property = Button.TextColorProperty, Value = Color.White },
                        new Setter { Property = Button.CornerRadiusProperty, Value = 5 },
                }
            };
            Xamarin.Forms.Application.Current.Resources.Add(buttonStyle);

            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;

        }
         
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
            CrossNFC.OnResume();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            CrossNFC.OnNewIntent(intent);
        }
    }
}