using Dapper;
using System.Data;

namespace Forum.Helpers
{
    public static class CommonExtensions
    {
        public static int LastInsertId(this IDbConnection cnn)
        {
            return cnn.ExecuteScalar<int>(@"SELECT LAST_INSERT_ID()");
        }
    }
}
