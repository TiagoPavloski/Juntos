using System;
using System.Collections.Generic;
using System.Text;

namespace Juntos.Teste.Domain.Commands
{
    public class UsuarioAlteraSenhaCommand
    {
        public string UserName { get; set; }
        public string SenhaAntiga { get; set; }
        public string SenhaNova { get; set; }
    }
}
