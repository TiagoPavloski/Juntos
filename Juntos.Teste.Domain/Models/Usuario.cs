using System;

namespace Juntos.Teste.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateBirth { get; set; }
    }
}
