using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using TccProj.Models;
using TccProj.Services.Interfaces;

namespace TccProj.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IAppService _appService;

        public UsuarioService(IAppService appService)
        {
            this._appService = appService;
        }

        public async Task<string> SalvarUsuario(UsuarioModel usuario)
        {
            try
            {
                var seq = await _appService.Client().Child("Users")
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
            var conteudo = (await _appService.Client().Child("Users").OnceAsync<UsuarioModel>())
                           .Where(w => w.Key == seq || w.Object.Email == email)
                           .FirstOrDefault();

            return conteudo.Object;
        }


    }
}
