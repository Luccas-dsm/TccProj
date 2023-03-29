using System;
using System.Collections.Generic;

namespace TccProj.Models
{
    public class UsuarioModel
    {
        public string Seq { get; set; }
        public string Nome { get; set; }
        public List<InfoDispositivoModel> InfoDispositivo { get; set; }

        public UsuarioModel PreencheDados()
        {
            var usuario = new UsuarioModel()
            {

                Seq = "1",
                Nome = "Marcelly",
                InfoDispositivo = new List<InfoDispositivoModel>()
                {
                    new InfoDispositivoModel()
                    {
                        Seq = "1",
                        SeqUsuario="1",
                        Fabricante="Xiaomi",
                        SistemaOperacional="Android 11",
                        CPU="SnapDragon",
                        MemoriaRam="4GB",
                        PossuiNFC=true,
                        DadosTeste = new List<DadosModel>()
                        {
                            new DadosModel()
                            {
                                Seq = "1",                                
                                Data= DateTime.Now,
                                Tecnologia="QrCode",
                                ModoOperacao="Leitura",
                                TempoResposta= 11,
                                UsoCpu=50,
                                UsoMemoria=40

                            }
                        }
                    },
                      new InfoDispositivoModel()
                    {
                        Seq = "2",
                        SeqUsuario="1",
                        Fabricante="Samsung",
                        SistemaOperacional="Android 11",
                        CPU="SnapDragon",
                        MemoriaRam="4GB",
                        PossuiNFC=true,
                        DadosTeste = new List<DadosModel>()
                        {
                            new DadosModel()
                            {
                                Seq = "1",                                
                                Data= DateTime.Now,
                                Tecnologia="QrCode",
                                ModoOperacao="Leitura",
                                TempoResposta= 11,
                                UsoCpu=100,
                                UsoMemoria=40

                            }
                        }
                    }
                },
            };
            return usuario;
        }
    }
}
