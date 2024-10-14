using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace northwind_net8_api01.MsSQL
{
    public class MSSQLConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public MSSQLConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection CreateConn()
        {
            //return _connection;
            return new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        }
    }
}
