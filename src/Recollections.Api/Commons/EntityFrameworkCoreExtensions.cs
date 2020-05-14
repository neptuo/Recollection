﻿using Microsoft.Extensions.Configuration;
using Neptuo.Recollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityFrameworkCoreExtensions
    {
        public static void UseDbServer(this DbContextOptionsBuilder options, IConfiguration configuration, PathResolver pathResolver)
        {
            if (configuration.GetValue("Server", DbServer.Sqlite) == DbServer.SqlServer)
                options.UseSqlServer(pathResolver(configuration.GetValue<string>("ConnectionString")));
            else
                options.UseSqlite(pathResolver(configuration.GetValue<string>("ConnectionString")));
        }
    }
}
