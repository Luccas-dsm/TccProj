using Plugin.NFC;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TccProj.Controller;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TccProj.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcGravarView : ContentPage
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
        public NfcGravarView(InfoDispositivoModel infoDispositivo)
        {
            InitializeComponent();

            this.Dispositivo = infoDispositivo;
            Dados = AppController.PreencheGravarNfc();
            Dados.SeqInfoDispositivo = Dispositivo.Seq;

            btnGravar.BackgroundColor = Color.FromHex("#F8F8F8");

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
                            Payload = NFCUtils.EncodeToByteArray(txtInput.Text),
                            LanguageCode = "pt"
                        };
                        break;
                    case NFCNdefTypeFormat.Uri:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Uri,
                            Payload = NFCUtils.EncodeToByteArray(txtInput.Text)
                        };
                        break;
                    case NFCNdefTypeFormat.Mime:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Mime,
                            MimeType = MIME_TYPE,
                            Payload = NFCUtils.EncodeToByteArray(txtInput.Text)
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
            CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;

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
                var resultado = new Label();

                resultado.Text = result.Message;
                resultado.FontSize = 20;
                resultado.FontAttributes = FontAttributes.Bold;
                resultado.TextColor = Color.Black;
            }
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // In order to support Mifare Classic 1K tags (read/write), you must set legacy mode to true.
            CrossNFC.Legacy = false;



            NfcIsEnabled = CrossNFC.Current.IsEnabled;
            if (!NfcIsEnabled)
                await DisplayAlert("Atenção", "NFC está desativado", "Ok");



            SubscribeEvents();

            await StartListeningIfNotiOS();

        }
        async void SubscribeEvents()
        {
            if (_eventsAlreadySubscribed)
                return;

            _eventsAlreadySubscribed = true;

            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
            CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;

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


        async Task Publish(NFCNdefTypeFormat? type = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            await StartListeningIfNotiOS();
            try
            {
                _type = NFCNdefTypeFormat.Empty;

                if (type.HasValue) _type = type.Value;
                {
                    stopwatch.Start();

                    double memoryBefore = GC.GetTotalMemory(true);

                    CrossNFC.Current.StartPublishing(!type.HasValue); // metodo sendo calculado

                    double memoryAfter = GC.GetTotalMemory(true);


                    Dados.UsoMemoria = Math.Abs(memoryAfter - memoryBefore);

                    stopwatch.Stop();
                    double ticks = stopwatch.ElapsedTicks;
                    double seconds = stopwatch.Elapsed.TotalSeconds;

                    var frequenciaHz = AppController.ConversaoDeFrequencia(ticks, seconds);
                    Dados.UsoCpu = AppController.TransoformarHzEmGhz(frequenciaHz); // divide por 1 bilhão para converter para GHz

                    Dados.TempoResposta = stopwatch.Elapsed;
                    Dados.Tamanho = Encoding.UTF8.GetByteCount(txtInput.Text);
                    _ = AppService.SalvarTeste(new DadosData(Dados));
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", "Aconteceu uma falha ao gravar :" + ex.Message, "Ok");
            }

        }

        async void Current_OnMessagePublished(ITagInfo tagInfo)
        {
            try
            {
                CrossNFC.Current.StopPublishing();

                if (tagInfo.IsEmpty)
                {
                    await DisplayAlert("Eba!", "A operação de formatação da Tag foi um sucesso!", "Ok");
                }
                else
                {
                    await DisplayAlert("Eba!", "A operação de gravação na Tag foi um sucesso!", "Ok");

                }

                btnGravar.BackgroundColor = Color.FromHex("#F8F8F8");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", "Aconteceu uma falha ao gravar :" + ex.Message, "Ok");
            }
        }

        private async void btnGravar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                btnGravar.BackgroundColor = Color.YellowGreen;
                await DisplayAlert("Info", "Enquanto o botão estiver verde é só aproximar da tag para gravar", "Ok");
                await Publish(NFCNdefTypeFormat.WellKnown);
            }
            else
                await DisplayAlert("Ops!", "O campo deve estar preenchido", "Ok");


        }

        private void btnLimparTxt_Clicked(object sender, EventArgs e)
        {
            txtInput.Text = null;
        }
    }
}