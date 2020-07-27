using CryptSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Juntos.Teste.Domain.Utils
{
    public static class Criptografia
    {
        public static string Codifica(string senha)
        {
            return Crypter.MD5.Crypt(senha);
        }

        public static bool Compara(string senha, string hash)
        {
            return Crypter.CheckPassword(senha, hash);
        }
    }
}
