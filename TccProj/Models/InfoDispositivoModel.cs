using System.Collections.Generic;

namespace TccProj.Models
{
    public class InfoDispositivoModel
    {
        public string Seq { get; set; }
        public string SeqUsuario { get; set; }
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public string SistemaOperacional { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public string MemoriaRam { get; set; }
        public bool PossuiNFC { get; set; }
        public List<DadosModel> DadosTeste { get; set; }
    }
}
