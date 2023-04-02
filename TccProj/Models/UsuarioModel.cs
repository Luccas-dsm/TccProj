using System;
using System.Collections.Generic;

namespace TccProj.Models
{
    public class UsuarioModel
    {
        public string Seq { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<InfoDispositivoModel> InfoDispositivos { get; set; }

    }
}
