using Juntos.Teste.Domain.Commands;
using Juntos.Teste.Domain.Models;
using System.Collections.Generic;


namespace Juntos.Teste.Domain.Contracts.Services
{
    public interface IUsuarioService
    {
        bool IsValid(string userName, string password);
        bool Create(Usuario cmd); 
        bool Update(Usuario cmd);
        bool Delete(int id);

        Usuario GetById(int id);
        List<Usuario> GetAll();
        bool AlteraSenha(UsuarioAlteraSenhaCommand cmd);
    }
}
