using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace Forum.Helpers
{
    public static class PostCrud
    {
        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) Post").FirstOrDefault();
        }
    }
}