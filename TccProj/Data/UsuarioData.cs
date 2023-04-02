using TccProj.Models;

namespace TccProj.Data
{
    public class UsuarioData
    {
        public string Email { get; set; }
        public string Senha { get; set; }

        public UsuarioData(UsuarioModel user)
        {
            this.Email = user.Email;
            this.Senha = user.Senha;
        }
    }
}
