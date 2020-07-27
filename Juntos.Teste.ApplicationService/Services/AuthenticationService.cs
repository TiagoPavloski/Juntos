using Juntos.Teste.Domain.Contracts.Services;
using Juntos.Teste.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Juntos.Teste.ApplicationService.Services
{
    public class AuthenticationService : IAuthenticateService
    {
        private readonly IUsuarioService _userService;
        private readonly TokenManagement _tokenManagement;

        public AuthenticationService(IUsuarioService service, IOptions<TokenManagement> tokenManagement)
        {
            _userService = service;
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsAuthenticated(TokenRequest request, out string token)
        {
            token = string.Empty;
            if (!_userService.IsValid(request.Username, request.Password))
                return false;

            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                new Claim("Type", "0"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            try
            {
                var jwtToken = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return true;
            }
            catch (Exception)
            {
                return false;

            }

        }
    }
}
