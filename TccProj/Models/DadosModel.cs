using System;

namespace TccProj.Models
{
    public class DadosModel
    {
        public string Seq { get; set; }
        public string SeqInfoDispositivo { get; set; }        
        public string Tecnologia { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan TempoResposta { get; set; }
        public double  UsoMemoria { get; set; }
        public double UsoCpu { get; set; }
        public long Tamanho { get; set; }
        public string ModoOperacao { get; set;} //Leitura/Gravação
    }
}
