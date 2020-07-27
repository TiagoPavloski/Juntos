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
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace Juntos.Teste.ApplicationService.Services
{
    public class UsuarioServiceFake : IUsuarioService
    {
        private List<Usuario> _usuarios;
        public UsuarioServiceFake()
        {
            _usuarios = new List<Usuario>()
            {
                new Usuario(){ Id = 1, UserName = "João", DateBirth = DateTime.Now.AddYears(-30), Password = Criptografia.Codifica("senha")},
                new Usuario(){ Id = 2, UserName = "Maria", DateBirth = DateTime.Now.AddYears(-25), Password = Criptografia.Codifica("mary")},
                new Usuario(){ Id = 3, UserName = "Robson", DateBirth = DateTime.Now.AddYears(-10), Password = Criptografia.Codifica("robs0n")},
                new Usuario(){ Id = 4, UserName = "Deyverson", DateBirth = DateTime.Now.AddYears(-80), Password =Criptografia.Codifica( "deyvin")}
            };
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
                return _usuarios.Where(x => x.UserName == userName).FirstOrDefault();
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

                user.Id = GeraId();
                _usuarios.Add(user);
                return true;
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
                var item = _usuarios.Where(x => x.Id == id).FirstOrDefault();
                return _usuarios.Remove(item);
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
                return _usuarios;
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
                return _usuarios.Where(x => x.Id == id).FirstOrDefault();
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
                var item = _usuarios.Where(x => x.Id == user.Id).FirstOrDefault();
                if (item == null)
                    return false;

                item.DateBirth = user.DateBirth;
                item.UserName = user.UserName;

                return true;
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
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;

            }
        }

        private int GeraId()
        {
            Random random = new Random();
            while (true)
            {
                var id = random.Next(1, 500);
                if (_usuarios.Where(x => x.Id == id).Count() == 0)
                    return id;
            }
        }
    }
}
