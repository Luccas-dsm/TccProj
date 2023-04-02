using System;

namespace TccProj.Data
{
    public class DadosData
    {        
        public string SeqInfoDispositivo { get; set; }
        public string Tecnologia { get; set; }
        public DateTime Data { get; set; }
        public long TempoResposta { get; set; }
        public long UsoMemoria { get; set; }
        public long UsoCpu { get; set; }
        public string ModoOperacao { get; set; } //Leitura/Gravação
    }
}
