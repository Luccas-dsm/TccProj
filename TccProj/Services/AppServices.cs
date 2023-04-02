using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TccProj.Data;
using TccProj.Models;

namespace TccProj.Services
{
    public class AppServices 
    {
        private readonly static string SenhaFireBase = "vCqChR1tpxjZVTpNtKTJbJHx1AawYC41gyjJkSWm";
        private readonly FirebaseClient FbClient = new FirebaseClient("https://tccproj-d5cc4-default-rtdb.firebaseio.com/",
                                  new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(SenhaFireBase) });



        #region [Usuarios]
        public async Task<string> SalvarUsuario(UsuarioData usuario)
        {
            try
            {
                var seq = await FbClient.Child("Users")
                    .PostAsync(usuario);

                return seq.Key;
            }
            catch
            {
                throw new Exception("Falha ao gravar o usuario");
            }
        }

        public async Task<UsuarioModel> BuscarUsuario(string seq, string email)
        {
            var conteudo = (await FbClient.Child("Users").OnceAsync<UsuarioModel>())
                           .Where(w => w.Key == seq || w.Object.Email == email)
                           .FirstOrDefault();

            return conteudo.Object;
        }
        #endregion


        #region[Dispositivos]
        public async Task<string> SalvarDispositivo(InfoDispositivoData dispositivo)
        {
            try
            {
                var seq = await FbClient.Child("Dispositivo")
                    .PostAsync(dispositivo);

                return seq.Key;
            }
            catch
            {
                throw new Exception("Falha ao gravar o dispositivo");
            }
        }

        public async Task<InfoDispositivoModel> BuscarDispositivo(string seq)
        {
            var conteudo = (await FbClient.Child("Dispositivo").OnceAsync<InfoDispositivoModel>())
                           .Where(w => w.Key == seq)
                           .FirstOrDefault();
            conteudo.Object.DadosTeste = await BuscarTestePeloDispositivo(conteudo.Key);
            return conteudo.Object;
        }
        public async Task<InfoDispositivoModel> BuscarDispositivoPeloUsuario(string seqUsuario)
        {
            var conteudo = (await FbClient.Child("Dispositivo").OnceAsync<InfoDispositivoModel>())
                           .Where(w => w.Object.SeqUsuario == seqUsuario)
                           .FirstOrDefault();

            return conteudo.Object;
        }
        #endregion

        #region[Dados dos Testes]
        public async Task<string> SalvarTeste(DadosModel dados)
        {
            try
            {
                var seq = await FbClient.Child("Testes")
                    .PostAsync(dados);

                return seq.Key;
            }
            catch
            {
                throw new Exception("Falha ao gravar o usuario");
            }
        }

        public async Task<List<DadosModel>> BuscarTestePeloDispositivo(string seqInfoDispositivo)
        {
            var conteudo = (await FbClient.Child("Testes").OnceAsync<DadosModel>())
                           .Where(w => w.Object.SeqInfoDispositivo == seqInfoDispositivo)
                           .Select(s=> s.Object).ToList();

            return conteudo;
        }
        public async Task<DadosModel> BuscarTeste(string seq)
        {
            var conteudo = (await FbClient.Child("Testes").OnceAsync<DadosModel>())
                           .Where(w => w.Key == seq)
                           .FirstOrDefault();

            return conteudo.Object;
        }
        #endregion
    }
}
