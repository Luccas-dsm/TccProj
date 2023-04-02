
using Android.OS;
using Java.IO;
using Java.Lang;
using System;

namespace TccProj.Controller
{
    public class AppController
    {
        public string GravaDadosLeitura()
        {
            return null;
        }
        public string GravaDadosGravacao()
        {
            return null;
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
        public void GetCPU()
        {
            string deviceModel = Build.Model;   //modelo do dispositivo
            string fabricante = Build.Manufacturer; //fabricante 
            string cpuAbi = Build.CpuAbi;   //arquitetura

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
    }
}
