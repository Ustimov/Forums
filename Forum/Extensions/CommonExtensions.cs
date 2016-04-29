using System.Data;
using Dapper;

namespace Forum.Extensions
{
    public static class CommonExtensions
    {
        public static int LastInsertId(this IDbConnection cnn)
        {
            return cnn.ExecuteScalar<int>(@"SELECT LAST_INSERT_ID()");
        }
    }
}
