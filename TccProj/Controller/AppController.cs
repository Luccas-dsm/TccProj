﻿using Android.Opengl;
using Android.OS;
using Java.IO;
using Java.Lang;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TccProj.Data;
using TccProj.Models;
using TccProj.Services;
using Xamarin.Essentials;

namespace TccProj.Controller
{
    public class AppController
    {

        public AppServices AppService = new AppServices();


        public async Task<InfoDispositivoModel> InformacoesDispositivo(string seqUsuario)
        {
            if (await AppService.ValidaDispositivoPeloUsuarioEModelo(seqUsuario, Build.Model))
            {
                var dispositivo = new InfoDispositivoModel()
                {
                    CPU = Build.CpuAbi,
                    GPU = GetGpuModel(),
                    Fabricante = Build.Manufacturer,
                    MemoriaRam = GetTotalMemory() + " Mb",
                    Modelo = Build.Model,
                    PossuiNFC = PossuiNfc(),
                    SeqUsuario = seqUsuario,
                    SistemaOperacional = DeviceInfo.Platform + " " + Build.VERSION.Release,
                    FabricanteCpu = Build.Hardware
                };
                dispositivo.Seq = await AppService.SalvarDispositivo(new InfoDispositivoData(dispositivo));
                return dispositivo;
            }
            else
            {
                return await AppService.BuscarDispositivoPeloUsuarioEModelo(seqUsuario, Build.Model);

            }
        }
        private bool PossuiNfc() => CrossNFC.Current.IsAvailable;
                
                  
    

        private string GetGpuModel()
        {
            EGLDisplay display = EGL14.EglGetDisplay(EGL14.EglDefaultDisplay);
            int[] version = new int[2];
            EGL14.EglInitialize(display, version, 0, version, 1);

            int[] configAttributes =
            {
                EGL14.EglRenderableType, EGL14.EglOpenglEs2Bit,
                EGL14.EglRedSize, 8,
                EGL14.EglGreenSize, 8,
                EGL14.EglBlueSize, 8,
                EGL14.EglDepthSize, 0,
                EGL14.EglNone
            };

            EGLConfig[] configs = new EGLConfig[1];
            int[] numConfigs = new int[1];
            EGL14.EglChooseConfig(display, configAttributes, 0, configs, 0, configs.Length, numConfigs, 0);

            EGLSurface surface = EGL14.EglCreatePbufferSurface(display, configs[0], new int[] { EGL14.EglWidth, 1, EGL14.EglHeight, 1, EGL14.EglNone }, 0);
            EGLContext context = EGL14.EglCreateContext(display, configs[0], EGL14.EglNoContext, new int[] { EGL14.EglContextClientVersion, 2, EGL14.EglNone }, 0);
            EGL14.EglMakeCurrent(display, surface, surface, context);

            string renderer = GLES20.GlGetString(GLES20.GlRenderer);
            string vendor = GLES20.GlGetString(GLES20.GlVendor);
            string versionString = GLES20.GlGetString(GLES20.GlVersion);

            EGL14.EglMakeCurrent(display, EGL14.EglNoSurface, EGL14.EglNoSurface, EGL14.EglNoContext);
            EGL14.EglDestroyContext(display, context);
            EGL14.EglDestroySurface(display, surface);

            return $"{vendor} {renderer} {versionString}";
        }

        public void GetWindowsCPU()
        {
            string cpuModel = "";

            try
            {
                var process = Runtime.GetRuntime().Exec("cat /proc/cpuinfo");
                BufferedReader reader = new BufferedReader(new InputStreamReader(process.InputStream));
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("model name"))
                    {
                        string[] parts = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            cpuModel = parts[1].Trim();
                            break;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                // Lidar com a exceção de IO
            }

            // Exibir o modelo da CPU do dispositivo
            var X = cpuModel;
        }

        public double GetTotalMemory()
        {
            //string manufacturer = Build.Manufacturer;
            //string model = Build.Model;
            //string osVersion = Build.VERSION.Release;

            // Obter a quantidade total de memória RAM disponível no dispositivo
            long totalRam = 0;

            try
            {
                var process = Runtime.GetRuntime().Exec("cat /proc/meminfo");
                BufferedReader reader = new BufferedReader(new InputStreamReader(process.InputStream));
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("MemTotal:"))
                    {
                        string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            long value;
                            if (long.TryParse(parts[1], out value))
                            {
                                // Converter em bytes
                                totalRam = value * 1024;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Opa encontramos um erro ao recuperar a memoria ram total.", ex);
            }
            // Converter em megabytes
            double totalRamInMb = totalRam / (1024 * 1024);

            return totalRamInMb;
        }

        public double TransoformarHzEmGhz(double frequencuaHZ) => frequencuaHZ / 1000000000;

        public double ConversaoDeFrequencia(double ticks, double segundos) => ticks / segundos;

        public bool HasLink(string text)
        {
            // Expressão regular que procura por padrões de texto que correspondem a um URL
            Regex urlRegex = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Procura por correspondências na string de texto
            MatchCollection matches = urlRegex.Matches(text);

            // Retorna true se pelo menos uma correspondência foi encontrada
            return (matches.Count > 0);
        }

        public string ExtractLink(string text)
        {
            // Expressão regular que procura por padrões de texto que correspondem a um URL
            Regex urlRegex = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Procura por correspondências na string de texto
            MatchCollection matches = urlRegex.Matches(text);

            // Extrai o primeiro link encontrado
            string link = matches[0].Value;

            // Extrai o texto antes e depois do link
            string beforeLink = text.Substring(0, matches[0].Index);
            string afterLink = text.Substring(matches[0].Index + matches[0].Length);


            if (!link.Contains("https://"))
                link = string.Format("https://" + link);
            // Retorna o texto antes do link e o link
            return link;
        }
        #region[Pre preenchimento dados de testes]
        public DadosModel PreencheEscanearNfc() => new DadosModel()
        {
            Data = DateTime.Now,
            ModoOperacao = "Escanear",
            Tecnologia = "NFC",
        };
        public DadosModel PreencheGravarNfc() => new DadosModel()
        {
            Data = DateTime.Now,
            ModoOperacao = "Gravacao",
            Tecnologia = "NFC",
        };
        public DadosModel PreencheEscanearQrCode() => new DadosModel()
        {
            Data = DateTime.Now,
            ModoOperacao = "Escanear",
            Tecnologia = "QrCode",
        };
        public DadosModel PreencheGravarQrCode() => new DadosModel()
        {
            Data = DateTime.Now,
            ModoOperacao = "Gravacao",
            Tecnologia = "QrCode",
        };
        #endregion
        public async Task<double> MediaTempoResposta(InfoDispositivoModel dispositivo)
        {
            var lista = await AppService.BuscarTestePeloDispositivo(dispositivo.Seq);

            TimeSpan somaTempo = new TimeSpan();
            lista.ForEach(f => somaTempo += f.TempoResposta);

            double mediaTempo = somaTempo.TotalSeconds / lista.Count;

            return mediaTempo;
        }

        public async Task<List<TramentoDeDadosModel>> TratamentoDeDados(InfoDispositivoModel dispositivo)
        {

            var listaTratada = new List<TramentoDeDadosModel>();

            var lista = await AppService.BuscarTestePeloDispositivo(dispositivo.Seq);

            var listaQrCode = lista.Where(w => w.Tecnologia.Equals("QrCode")).ToList();

            var listaNfc = lista.Where(w => w.Tecnologia.Equals("NFC")).ToList();
            TimeSpan tempo = new TimeSpan();

            listaQrCode.ForEach(f => tempo += f.TempoResposta);


            double mediaTempoRespostaQrCode = tempo.TotalMilliseconds / listaQrCode.Count();
            tempo = new TimeSpan();

            listaNfc.ForEach(f => tempo += f.TempoResposta);
            double mediaTempoRespostaNfc = tempo.TotalMilliseconds / listaQrCode.Count();

            try
            {
                double mediaGravacaoQrCode = CalculaMediaModoOperacao(listaQrCode, "Gravacao");
                double mediaLeituraQrCode = CalculaMediaModoOperacao(listaQrCode, "Escanear");
                listaTratada.Add(new TramentoDeDadosModel() { Tecnologia = "QrCode", SeqInfoDispositivo = dispositivo.Seq, Tipo = "Gravacao", Media = mediaGravacaoQrCode });
                listaTratada.Add(new TramentoDeDadosModel() { Tecnologia = "QrCode", SeqInfoDispositivo = dispositivo.Seq, Tipo = "Escanear", Media = mediaLeituraQrCode });
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Erro ao buscar dados dos testes " + e.Message);
            }

            try
            {
                double mediaGravacaoNfc = CalculaMediaModoOperacao(listaNfc, "Gravacao");
                double mediaLeituraNfc = CalculaMediaModoOperacao(listaNfc, "Escanear");
                listaTratada.Add(new TramentoDeDadosModel() { Tecnologia = "NFC", SeqInfoDispositivo = dispositivo.Seq, Tipo = "Gravacao", Media = mediaGravacaoNfc });
                listaTratada.Add(new TramentoDeDadosModel() { Tecnologia = "NFC", SeqInfoDispositivo = dispositivo.Seq, Tipo = "Escanear", Media = mediaLeituraNfc });
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Erro ao buscar dados dos testes " + e.Message);
            }

            return listaTratada;
        }

        private double CalculaMediaModoOperacao(List<DadosModel> lista, string parametro)
        {
            var listaSeparada = lista.Where(w => w.ModoOperacao == parametro).ToList();
            if (listaSeparada.Count() <= 0)
                return 0;

            double tempoTotal = 0;

            listaSeparada.ForEach(f => tempoTotal += f.TempoResposta.TotalSeconds);


            return tempoTotal / listaSeparada.Count();
        }
        public bool ValidaEnderecoEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
