using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juntos.Teste.Domain;
using Juntos.Teste.Domain.Commands;
using Juntos.Teste.Domain.Contracts.Services;
using Juntos.Teste.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Juntos.Teste.Api.Controllers
{
    [BearerAuthorize]
    [Route("Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService _usuarioService)
        {
            usuarioService = _usuarioService;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Usuario usuario)
        {
            try
            {
                if (usuarioService.Create(usuario))
                    return Ok(true);

                return BadRequest(false);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = usuarioService.GetById(id);
                if(result != null)
                    return Ok(JsonConvert.SerializeObject(result));

                return BadRequest("");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = usuarioService.GetAll();
                if (result != null)
                    return Ok(JsonConvert.SerializeObject(result));

                return BadRequest("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (usuarioService.Delete(id))
                    return Ok(true);

                return BadRequest(false);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AlterarSenha")]
        public IActionResult AlterarSenha(UsuarioAlteraSenhaCommand cmd)
        {
            try
            {
                if (usuarioService.AlteraSenha(cmd))
                    return Ok(true);

                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
