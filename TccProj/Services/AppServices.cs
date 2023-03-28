using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using TccProj.Models;

namespace TccProj.Services
{
    public class AppServices
    {
        private static string SenhaFireBase = "vCqChR1tpxjZVTpNtKTJbJHx1AawYC41gyjJkSWm";
        FirebaseClient FbClient = new FirebaseClient("https://tccproj-d5cc4-default-rtdb.firebaseio.com/",
                                  new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(SenhaFireBase) });
        #region[Usuário]
        public async Task<Guid> SalvarUsuario(UsuarioModel usuario)
        {
            try
            {
                var seq = await FbClient.Child("Usuarios")
                    .PostAsync(usuario);
                return seq.Object.Seq;
            }
            catch
            {
                throw new Exception("Falha ao gravar o usuario");
            }
        }

        public async Task<UsuarioModel> BuscarUsuario(Guid? seq, string nome)
        {
            var conteudo = (await FbClient.Child("Usuarios").OnceAsync<UsuarioModel>())
                           .Where(w => w.Object.Seq == seq || w.Object.Email == nome)
                           .FirstOrDefault();

            return conteudo.Object;
        }


        public async Task<string> UpdateUsuario(string nome)
        {
            var conteudo = (await FbClient.Child("Usuarios").OnceAsync<UsuarioModel>())
                           .Where(w => w.Object.Email == nome)
                           .FirstOrDefault();

            conteudo.Object.InfoDispositivo.ForEach(f => { if (f.Seq == "0") f.CPU = "Enyos"; });

            await FbClient.Child("Usuarios")
                    .PutAsync(conteudo);

            return conteudo.Object.Email;
        }

        public async Task<bool> ValidaUsuario(string email, string senha)
        {
            var conteudo = (await FbClient.Child("Usuarios").OnceAsync<UsuarioModel>())
               .Where(w => w.Object.Email.Equals(email) && w.Object.Senha.Equals(senha))
               .FirstOrDefault();

            return conteudo != null;
        }
        public async Task<bool> ValidaEmailUsuario(string email)
        {
            var conteudo = (await FbClient.Child("Usuarios").OnceAsync<UsuarioModel>())
               .Where(w => w.Object.Email.Equals(email))
               .FirstOrDefault();

            return conteudo != null;
        }
        #endregion

        public async Task<string> SalvarDispositivo(InfoDispositivoModel dispositivoModel)
        {
            try
            {
                var seq = await FbClient.Child("Dispositivo")
                    .PostAsync(dispositivoModel);
                return seq.Key;
            }
            catch
            {
                throw new Exception("Falha ao gravar o dispositivo");
            }

        }
    }
}
