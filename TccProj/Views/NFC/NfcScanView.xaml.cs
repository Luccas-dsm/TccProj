using Plugin.NFC;
using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
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
        AppServices AppService = new AppServices();
        DadosModel Dados;
        InfoDispositivoModel Dispositivo;
        ITagInfo TagInfo;
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
        public NfcScanView(InfoDispositivoModel infoDispositivo)
        {
            this.Dispositivo = infoDispositivo;
            InitializeComponent();
            Dados = AppController.PreencheEscanearNfc();
            Dados.SeqInfoDispositivo = Dispositivo.Seq;

        }

        async Task LerTag()
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                if (TagInfo != null)
                {
                    stopwatch.Start();
                    double memoryBefore = GC.GetTotalMemory(true);

                    CrossNFC.Current.StartListening();

                    double memoryAfter = GC.GetTotalMemory(true);

                    Dados.UsoMemoria = Math.Abs(memoryAfter - memoryBefore);

                    stopwatch.Stop();
                    double ticks = stopwatch.ElapsedTicks;
                    double seconds = stopwatch.Elapsed.TotalSeconds;

                    var frequenciaHz = AppController.ConversaoDeFrequencia(ticks, seconds);
                    Dados.UsoCpu = AppController.TransoformarHzEmGhz(frequenciaHz); // divide por 1 bilhão para converter para GHz

                    Dados.TempoResposta = stopwatch.Elapsed;
                    Dados.Tamanho = Encoding.UTF8.GetByteCount(TagInfo.Records[0].Message);

                    _ = AppService.SalvarTeste(new DadosData(Dados));

                }
                else
                {
                    CrossNFC.Current.StartListening();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Opa!", "Apareceu o seguinte erro na leitura: " + ex, "Ok");
            }
        }



        void UnsubscribeEvents()
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;

        }
        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {

            if (tagInfo == null)
            {
                await DisplayAlert("Ops!", "Nenhuma tag encontrada", "Ok");
                return;
            }
            TagInfo = tagInfo;
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