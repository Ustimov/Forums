using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Models;
using Forum.Dtos.Forum;

namespace Forum.Helpers
{
    public static class ForumCrud
    {
        public static void Create(CreateForum forum)
        {
            ConnectionProvider.DbConnection.Execute(
                @"insert into Forum (Name, ShortName, User) values (@Name, @ShortName, @User)",
                new
                {
                    Name = forum.Name,
                    ShortName = forum.ShortName,
                    User = forum.User,
                });
        }

        // TODO: related
        public static ForumModel<string> Read(ForumDetails request)
        {
            return ConnectionProvider.DbConnection.Query<ForumModel<string>>(
                @"select * from Forum where ShortName = @ShortName",
                new { ShortName = request.Forum }).FirstOrDefault();
        }

        public static ForumModel<string> Read(string shortName)
        {
            return ConnectionProvider.DbConnection.Query<ForumModel<string>>(
                @"select * from Forum where ShortName = @ShortName",
                new { ShortName = shortName }).FirstOrDefault();
        }

        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) Forum").FirstOrDefault();
        }
    }
}