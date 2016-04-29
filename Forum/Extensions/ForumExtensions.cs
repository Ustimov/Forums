using System.Data;
using System.Linq;
using Dapper;
using Forum.Models;
using Forum.Dtos.Forum;

namespace Forum.Extensions
{
    public static class ForumExtensions
    {
        public static void CreateForum(this IDbConnection cnn, CreateForum cf)
        {
            cnn.Execute(@"INSERT INTO Forum (Name, ShortName, User) VALUES (@Name, @ShortName, @User)", cf);
        }

        public static ForumModel<object> ReadForum(this IDbConnection cnn, string shortName)
        {
            return cnn.Query<ForumModel<object>>(
                @"SELECT Id, Name, ShortName, User FROM Forum WHERE ShortName=@ShortName",
                new { ShortName = shortName }).FirstOrDefault();
        }

        public static int CountForums(this IDbConnection cnn)
        {
            return cnn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Forum");
        }
    }
}