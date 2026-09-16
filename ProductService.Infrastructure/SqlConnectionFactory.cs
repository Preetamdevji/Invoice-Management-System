using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Infrastructure
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public  SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
