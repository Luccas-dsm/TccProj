using System.Collections.Generic;
using System.Management;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TccProj.Models
{
    public class MockDados
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Conteudo { get; set; }

        public List<MockDados> PreencheDados()
        {
            var lista = new List<MockDados>()
            {
                new MockDados(){ Titulo= "CPU",   Conteudo =""},
                new MockDados(){ Titulo="RAM",    Conteudo = "XX"},
                new MockDados(){ Titulo="SO",     Conteudo = DeviceInfo.Platform.ToString() +" "+ DeviceInfo.Version },
                new MockDados(){ Titulo="QRCode", Conteudo ="50%" ,         Descricao="Leitura"  },
                new MockDados(){ Titulo="QRCode", Conteudo ="40%" ,         Descricao="Gravação"  },
                new MockDados(){ Titulo="QRCode", Conteudo ="30%" ,         Descricao="Resposta"  },
                new MockDados(){ Titulo="NFC",    Conteudo ="80%",          Descricao="Leitura"  },
                new MockDados(){ Titulo="NFC",    Conteudo ="90%",          Descricao="Gravação"  },
                new MockDados(){ Titulo="NFC",    Conteudo ="50%",          Descricao="Resposta"  },
                new MockDados(){ Titulo="Beacon", Conteudo ="90%" ,         Descricao="Leitura"  },
                new MockDados(){ Titulo="Beacon", Conteudo ="30%" ,         Descricao="Gravação"  },
                new MockDados(){ Titulo="Beacon", Conteudo ="20%" ,         Descricao="Resposta"  },
            };
  
            return lista;
        }


        public string modelo()
        {


            string cpuModel = "";

            using (ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject mo in mos.Get())
                {
                    cpuModel = mo["Name"].ToString();
                    break;
                }
            }
            return cpuModel;
        }
    }
}
