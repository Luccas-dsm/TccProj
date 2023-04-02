using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TccProj.Models;

namespace TccProj.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<string> SalvarUsuario(UsuarioModel usuario);
        Task<UsuarioModel> BuscarUsuario(string seq, string email);
    }
}
