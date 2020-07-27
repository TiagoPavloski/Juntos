using Juntos.Teste.Api.Controllers;
using Juntos.Teste.ApplicationService.Services;
using Juntos.Teste.Domain.Commands;
using Juntos.Teste.Domain.Contracts.Services;
using Juntos.Teste.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Juntos.Teste.Test
{
    [TestClass]
    public class UsuarioTest
    {
        UsuarioController controller;
        IUsuarioService usuarioService;
        public UsuarioTest()
        {
            usuarioService = new UsuarioServiceFake();
            controller = new UsuarioController(usuarioService);
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void GetById(int id)
        {
            var result = usuarioService.GetById(id);
            Assert.IsTrue(result != null && result.Id == id);
        }

        [TestMethod]
        [DynamicData(nameof(usuarioCreate))]
        public void Create(Usuario usuario)
        {
            Assert.IsTrue(usuarioService.Create(usuario));
        }

        [TestMethod]
        [DynamicData(nameof(usuarioUpdate))]
        public void Update(Usuario usuario)
        {
            Assert.IsTrue(usuarioService.Update(usuario));
        }

        [TestMethod]
        [DynamicData(nameof(usuarioChangePassword))]
        public void ChangePassword(UsuarioAlteraSenhaCommand cmd)
        {
            Assert.IsTrue(usuarioService.AlteraSenha(cmd));
        }

        [TestMethod]
        public void GetAll()
        {
            Assert.IsTrue(usuarioService.GetAll().Count == 4);
        }




        static IEnumerable<object[]> usuarioCreate
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new Usuario { UserName = "Jorge", Password = "passjor", DateBirth = DateTime.Now.AddYears(-18).AddDays(12) },
                    }
                };
            }
        }

        static IEnumerable<object[]> usuarioUpdate
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new Usuario { Id = 3, UserName = "Robson Silva", DateBirth = DateTime.Now.AddYears(-19).AddDays(12) },
                    }
                };
            }
        }

        static IEnumerable<object[]> usuarioChangePassword
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new UsuarioAlteraSenhaCommand { UserName = "Robson", SenhaAntiga = "robs0n", SenhaNova = "robsilva" },
                    }
                };
            }
        }

    }
}
