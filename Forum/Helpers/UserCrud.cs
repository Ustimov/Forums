using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace Forum.Helpers
{
    public static class UserCrud
    {
        public static void Create(UserModel user)
        {
            ConnectionProvider.DbConnection.Execute(
                @"insert into User (About, Email, IsAnonymous, Name, Username)
                values (@About, @Email, @IsAnonymous, @Name, @Username)",
                new
                {
                    About = user.About ?? "",
                    Email = user.Email,
                    IsAnonymous = user.IsAnonymous,
                    Name = user.Name ?? "",
                    Username = user.Username ?? "",
                });
        }

        public static UserModel Read(string email)
        {
            var user = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from User where Email = @Email",
                new { Email = email })
                .FirstOrDefault();
            
            if (user != null && user.IsAnonymous)
            {
                user.About = user.Name = user.Username = null;
            }

            return user;
        }

        public static void Update(UserModel user)
        { 
            ConnectionProvider.DbConnection.Execute(
                @"update User set About=@About, Name=@Name where Email=@Email",
                new { About = user.About, Name = user.Name, Email = user.Email });
        }
    }
}