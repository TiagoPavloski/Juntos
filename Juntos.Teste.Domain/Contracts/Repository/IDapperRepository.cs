using System;
using System.Collections.Generic;
using System.Text;

namespace Juntos.Teste.Domain.Contracts.Repository
{
    public interface IDapperRepository
    {
        IEnumerable<T> GetDapperResult<T>(string query);
        IEnumerable<T> GetDapperResult<T>(string query, object obj);
        bool Execute(string query, object obj);
    }
}
