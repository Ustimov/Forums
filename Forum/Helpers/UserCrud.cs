using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Dtos.User;
using Forum.Dtos.Forum;
using Forum.Dtos.Base;

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
            
            if (user == null)
            {
                return user;
            }
            else if (user.IsAnonymous)
            {
                user.About = user.Name = user.Username = null;
            }

            // TODO: To additional method
            user.Followers = ReadFollowerEmails(user.Email);
            user.Following = ReadFollowingEmails(user.Email);
            user.Subscriptions = ReadSubscriptions(user.Email);

            return user;
        }

        public static void Update(UserModel user)
        { 
            ConnectionProvider.DbConnection.Execute(
                @"update User set About=@About, Name=@Name where Email=@Email",
                new { About = user.About, Name = user.Name, Email = user.Email });
        }

        public static List<UserModel> ReadFollowers(ListFollowers request)
        {
            var users = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from Follower f left join User u on f.Follower=u.Email
                where f.Followee=@Email and u.Id >= @SinceId order by @Order",
                new { Email = request.Email, SinceId = request.SinceId, Order = request.Order });

            foreach (var user in users)
            {
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
                user.Subscriptions = ReadSubscriptions(user.Email);
            }

            return users.ToList();
        }

        public static List<UserModel> ReadFollowing(ListFollowing request)
        {
            var users = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from Follower f left join User u on f.Followee=u.Email
                where f.Follower=@Email and u.Id >= @SinceId order by @Order",
                new { Email = request.Email, SinceId = request.SinceId, Order = request.Order });

            foreach (var user in users)
            {
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
            }

            return users.ToList();
        }

        private static List<string> ReadFollowerEmails(string email)
        {
            var emails = ConnectionProvider.DbConnection.Query<string>(
                @"select Follower from Follower where Followee=@Email", new { Email = email });

            return emails.ToList();
        }

        private static List<string> ReadFollowingEmails(string email)
        {
            var emails = ConnectionProvider.DbConnection.Query<string>(
                @"select Followee from Follower where Follower=@Email", new { Email = email });

            return emails.ToList();
        }

        private static List<int> ReadSubscriptions(string email)
        {
            return ConnectionProvider.DbConnection.Query<int>(
                @"select Thread from Subscribe where User=@Email", new { Email = email }).
                ToList();
        }

        public static List<UserModel> ReadAll(ListUsers request)
        {
            throw new NotImplementedException();
            // Get user with posts on this forum
            var users = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from User where Forum=@Forum" +
                (request.SinceId == null ? string.Empty : " and Id >= @SinceId") +
                (request.Order == null ? string.Empty : " order by Name " + request.Order) +
                (request.Limit == null ? string.Empty : " limit @Limit"),
                new
                {
                    Forum = request.Forum,
                    SinceId = request.SinceId,
                    Limit = request.Limit,
                }).AsList();

            foreach (var user in users)
            {
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
                user.Subscriptions = ReadSubscriptions(user.Email);
            }

            return users;
        }

        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) User").FirstOrDefault();
        }
    }
}