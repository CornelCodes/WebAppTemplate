using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CredentialLoader
    {
        public static string LoadConnectionString()
        {
            return "Host=localhost;port=2002;Database=templatedev;Username=webuser;Password=password";
        }
    }
}
