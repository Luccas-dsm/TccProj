using System;
using TccProj.Models;

namespace TccProj.Data
{
    public class DadosData
    {        
        public string SeqInfoDispositivo { get; set; }
        public string Tecnologia { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan TempoResposta { get; set; }
        public double UsoMemoria { get; set; }
        public double UsoCpu { get; set; }
        public string ModoOperacao { get; set; } //Leitura/Gravação

        public DadosData(DadosModel dados)
        {
            this.SeqInfoDispositivo = dados.SeqInfoDispositivo;
            this.Tecnologia =   dados.Tecnologia;
            this.Data = dados.Data;
            this.TempoResposta = dados.TempoResposta;
            this.UsoMemoria= dados.UsoMemoria;
            this.UsoCpu = dados.UsoCpu;
            this.ModoOperacao = dados.ModoOperacao;
        }
    }
}
