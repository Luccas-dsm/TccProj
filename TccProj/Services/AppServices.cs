using Firebase.Database;
using Firebase.Database.Query;
using Java.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TccProj.Controller;
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

        public async Task<string> UpdateUsuario(string nome)
        {
            var conteudo = (await FbClient.Child("Users").OnceAsync<UsuarioModel>())
                           .Where(w => w.Object.Email == nome)
                           .FirstOrDefault();

            //conteudo.Object.ForEach(f => { if (f.Seq == "0") f.CPU = "Enyos"; });

            await FbClient.Child("Users")
                    .PutAsync(conteudo);

            return conteudo.Object.Email;
        }

        public async Task<string> ValidaUsuario(string email, string senha)
        {
            var conteudo = (await FbClient.Child("Users").OnceAsync<UsuarioModel>())
               .Where(w => w.Object.Email.Equals(email) && w.Object.Senha.Equals(senha))
               .FirstOrDefault();

            string key;
            if(conteudo!=null)
                key = conteudo.Key;
            else key = "";

            return key;
        }
        public async Task<bool> ValidaEmailUsuario(string email)
        {
            
            var conteudo = (await FbClient.Child("Users").OnceAsync<UsuarioModel>())
               .Where(w => w.Object.Email.Equals(email))
               .FirstOrDefault();

            return conteudo != null;
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
        public async Task<InfoDispositivoModel> BuscarDispositivoPeloUsuarioEModelo(string seqUsuario, string modelo)
        {
            var conteudo = (await FbClient.Child("Dispositivo").OnceAsync<InfoDispositivoModel>())
                           .Where(w => w.Object.SeqUsuario == seqUsuario && w.Object.Modelo == modelo)
                           .FirstOrDefault();
            conteudo.Object.Seq = conteudo.Key;

            return conteudo.Object;
        }
        public async Task<bool> ValidaDispositivoPeloUsuarioEModelo(string seqUsuario, string modelo)
        {
            var conteudo = (await FbClient.Child("Dispositivo").OnceAsync<InfoDispositivoModel>())
                           .Where(w => w.Object.SeqUsuario == seqUsuario && w.Object.Modelo == modelo)
                           .FirstOrDefault();


            return conteudo == null ? true : false;
        }
        #endregion

        #region[Dados dos Testes]
        public async Task<string> SalvarTeste(DadosData dados)
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
            var conteudo = FbClient.Child("Testes").OnceAsync<DadosModel>().Result
                         .Where(w => w.Object.SeqInfoDispositivo == seqInfoDispositivo).ToList();

 

     
            return conteudo.Select(s=> s.Object).ToList();
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
