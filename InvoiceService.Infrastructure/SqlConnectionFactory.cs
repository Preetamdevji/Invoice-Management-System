using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Infrastructure
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionStrong)
        {
            _connectionString = connectionStrong;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
