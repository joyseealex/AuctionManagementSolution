using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionManagement.DataAccess
{
    public class BaseDataAccess
    {
        public IConfiguration Configuration { get; }
        public string ConnectionString = String.Empty;

        public BaseDataAccess(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DevConnection");
        }

        
    }
}
