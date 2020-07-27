using Dapper;
using Juntos.Teste.Domain.Contracts.Repository;
using Juntos.Teste.Domain.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Juntos.Teste.Domain.Repository
{
    public class DapperRepository : IDapperRepository
    {
        private readonly ConnString connString;

        public DapperRepository(IOptions<ConnString> _connString)
        {
            connString = _connString.Value;
        }
        public bool Execute(string query, object obj)
        {
            SqlConnection conn = new SqlConnection(connString.ConnPrincipal);

            conn.Open();
            var result = conn.Execute(query, obj);
            conn.Close();

            return result == 1;
        }

        public IEnumerable<T> GetDapperResult<T>(string query)
        {
            SqlConnection conn = new SqlConnection(connString.ConnPrincipal);

            conn.Open();
            var result = conn.Query<T>(query, commandTimeout: 120);
            conn.Close();

            return result;
        }

        public IEnumerable<T> GetDapperResult<T>(string query, object obj)
        {
            SqlConnection conn = new SqlConnection(connString.ConnPrincipal);

            conn.Open();
            var result = conn.Query<T>(query, param: obj, commandTimeout: 120);
            conn.Close();

            return result;
        }
    }
}
