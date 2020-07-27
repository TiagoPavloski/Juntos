using Juntos.Teste.Domain.Contracts.Services;
using Juntos.Teste.Domain.Models;
using Juntos.Teste.Domain.Contracts.Repository;
using Juntos.Teste.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Juntos.Teste.Domain.Commands;

namespace Juntos.Teste.ApplicationService.Services
{
    public class UsuarioService : IUsuarioService
    {
        IDapperRepository dapperRepository;
        public UsuarioService(IDapperRepository _dapperRepository)
        {
            dapperRepository = _dapperRepository;
        }

        public bool IsValid(string userName, string password)
        {
            var user = Find(userName);
            if (user == null)
                return false;

            return Criptografia.Compara(password, user.Password);
        }

        private Usuario Find(string userName)
        {
            try
            {
                return dapperRepository.GetDapperResult<Usuario>("SELECT Username, Password " +
                    "FROM dbo.Usuario " +
                    "WHERE Username = @Username", new { Username = userName }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Create(Usuario user)
        {
            try
            {
                user.Password = Criptografia.Codifica(user.Password);
                return dapperRepository.Execute(" Insert into Usuario (UserName, Password, DateBirth) values ( @UserName, @Password, @DateBirth)", user);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                return dapperRepository.Execute("Delete from usuario where id = @id", new { id = id });
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                return dapperRepository.GetDapperResult<Usuario>("Select * from usuario with(nolock)").ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Usuario GetById(int id)
        {
            try
            {
                return dapperRepository.GetDapperResult<Usuario>("Select * from usuario with(nolock) where id = @id", new { id = id }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Update(Usuario user)
        {
            try
            {
                return dapperRepository.Execute("Update usuario set username = @username, dateBirth = @dateBirth where id = @id", user);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AlteraSenha(UsuarioAlteraSenhaCommand cmd)
        {
            try
            {
                if (IsValid(cmd.UserName, cmd.SenhaAntiga))
                {
                    cmd.SenhaNova = Criptografia.Codifica(cmd.SenhaNova);
                    return dapperRepository.Execute("Update usuario set password = @password where username = @username", new { password = cmd.SenhaNova, username = cmd.UserName });
                }
                return false;

            }
            catch (Exception)
            {

                return false;

            }
        }
    }
}
