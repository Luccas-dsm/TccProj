using TccProj.Models;

namespace TccProj.Data
{
    public class InfoDispositivoData
    {        
        public string SeqUsuario { get; set; }
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public string SistemaOperacional { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public string MemoriaRam { get; set; }
        public bool PossuiNFC { get; set; }
        public string FabricanteCpu { get; set; }

        public InfoDispositivoData(InfoDispositivoModel info) 
        {
            this.SeqUsuario = info.SeqUsuario;
            this.Fabricante = info.Fabricante;
            this.Modelo = info.Modelo;
            this.SistemaOperacional= info.SistemaOperacional;
            this.CPU = info.CPU;
            this.MemoriaRam = info.MemoriaRam;
            this.PossuiNFC = info.PossuiNFC;
            this.GPU = info.GPU;
            this.FabricanteCpu = info.FabricanteCpu;
        }
    }
}
