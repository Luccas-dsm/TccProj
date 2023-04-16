using Plugin.NFC;
using System;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TccProj.Controller;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.QrCode.Internal;
using static Android.Icu.Text.TimeZoneFormat;
using static System.Net.Mime.MediaTypeNames;

namespace TccProj.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcScanView : ContentPage
    {
        AppController AppController = new AppController();
        NFCNdefTypeFormat _type;
        public const string MIME_TYPE = "application/com.companyname.nfcsample";
        bool _makeReadOnly = false;

        private bool _deviceIsListening;

        private bool _nfcIsEnabled;
        public bool NfcIsDisabled => !NfcIsEnabled;
        public bool NfcIsEnabled
        {
            get => _nfcIsEnabled;
            set
            {
                _nfcIsEnabled = value;
                OnPropertyChanged(nameof(NfcIsEnabled));
                OnPropertyChanged(nameof(NfcIsDisabled));
            }
        }
        public NfcScanView()
        {
            InitializeComponent();
  

            LerTag();


        }

        async Task LerTag()
        {
            try
            {
                CrossNFC.Current.StartListening();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Opa!", "Apareceu o seguinte erro na leitura: " + ex, "Ok");
            }
        }


        async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
        {
            if (!CrossNFC.Current.IsWritingTagSupported)
            {
                await DisplayAlert("Opa!", "Este tipo de tag não aceita ser editada ou escrita", "Ok");
                return;
            }

            try
            {
                NFCNdefRecord record = null;
                switch (_type)
                {
                    case NFCNdefTypeFormat.WellKnown:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.WellKnown,
                            MimeType = MIME_TYPE,
                            Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!"),
                            LanguageCode = "en"
                        };
                        break;
                    case NFCNdefTypeFormat.Uri:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Uri,
                            Payload = NFCUtils.EncodeToByteArray("https://github.com/franckbour/Plugin.NFC")
                        };
                        break;
                    case NFCNdefTypeFormat.Mime:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Mime,
                            MimeType = MIME_TYPE,
                            Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!")
                        };
                        break;
                    default:
                        break;
                }

                if (!format && record == null)
                    throw new Exception("Record can't be null.");

                tagInfo.Records = new[] { record };

                if (format)
                    CrossNFC.Current.ClearMessage(tagInfo);
                else
                {
                    CrossNFC.Current.PublishMessage(tagInfo, _makeReadOnly);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", "Encontramos um erro: " + ex.Message, "Ok");
            }
        }
        void UnsubscribeEvents()
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
            //CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;
            //CrossNFC.Current.OnNfcStatusChanged -= Current_OnNfcStatusChanged;
            // CrossNFC.Current.OnTagListeningStatusChanged -= Current_OnTagListeningStatusChanged;

        }
        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            if (tagInfo == null)
            {
                await DisplayAlert("Ops!", "Nenhuma tag encontrada", "Ok");
                return;
            }

            // Customized serial number
            var identifier = tagInfo.Identifier;
            var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
            var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

            if (!tagInfo.IsSupported)
            {
                await DisplayAlert("Ops!", "Tipo de Tag não suportada" + title, "Ok");
            }
            else if (tagInfo.IsEmpty)
            {
                await DisplayAlert("", "Tag Vazia", "Ok");
            }
            else
            {
                var result = tagInfo.Records[0];             
                
                resultado.Text = result.Message;
                resultado.FontSize = 20;
                resultado.FontAttributes = FontAttributes.Bold;
                resultado.TextColor = Color.Black;

                stackResultado.Padding = 20;
                stackResultado.Children.Add(resultado);
            }
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // In order to support Mifare Classic 1K tags (read/write), you must set legacy mode to true.
            CrossNFC.Legacy = false;

            if (CrossNFC.IsSupported)
            {
                if (!CrossNFC.Current.IsAvailable)
                {
                    await DisplayAlert("Ops!", "O seu dispositivo não possui a tecnologia NFC", "OK");
                    //       await Navigation.PopModalAsync();
                }
                else
                {

                    NfcIsEnabled = CrossNFC.Current.IsEnabled;
                    if (!NfcIsEnabled)
                        await DisplayAlert("Atenção", "NFC está desativado", "Ok");
                }


                SubscribeEvents();

                await StartListeningIfNotiOS();
            }
        }
        void SubscribeEvents()
        {
            if (_eventsAlreadySubscribed)
                return;

            _eventsAlreadySubscribed = true;

            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            //CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
            //CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
            //CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;
        }

        protected override bool OnBackButtonPressed()
        {
            UnsubscribeEvents();
            CrossNFC.Current.StopListening();
            return base.OnBackButtonPressed();
        }
        bool _eventsAlreadySubscribed = false;
        async Task StartListeningIfNotiOS()
        {
            await LerTag();
        }

        private void btnOpenLink_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(resultado.Text))
            {
                if (AppController.HasLink(resultado.Text))
                {
                    string link = AppController.ExtractLink(resultado.Text);
                    Device.OpenUri(new Uri(link));
                }
            }
        }
    }
}