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
        #region[Usuario]
        public async Task<string> SalvarUsuario(UsuarioModel usuario)
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
                           .Where(w => w.Object.Seq == seq || w.Object.Email == email)
                           .FirstOrDefault();

            return conteudo.Object;
        }
        #endregion

        public async Task<string> SalvarDispositivo(InfoDispositivoModel dispositivoModel)
        {
            try
            {
                var seq = await FbClient.Child("Usuarios")
                    .PostAsync(dispositivoModel);
                return seq.Key;
            }
            catch
            {
                throw new Exception("Falha ao gravar o usuario");
            }

        }
    }
}
