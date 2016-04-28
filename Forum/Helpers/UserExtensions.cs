using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Dtos.User;
using Forum.Dtos.Forum;
using Forum.Dtos.Base;
using System.Data;
using System.Data.Common;

namespace Forum.Helpers
{
    public static class UserExtensions
    {
        public static void CreateUser(this IDbConnection cnn, UserModel user)
        {
            cnn.Execute(
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

        public static UserModel ReadUser(this IDbConnection cnn, string email)
        {
            var user = cnn.Query<UserModel>(
                @"select * from User where Email = @Email",
                new { Email = email }).FirstOrDefault();
            
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

        public static void UpdateUser(this IDbConnection cnn, UserModel um)
        { 
            cnn.Execute(@"UPDATE User SET About=@About, Name=@Name WHERE Email=@Email", um);
        }

        public static List<UserModel> ReadFollowers(this IDbConnection cnn, ListFollowers request)
        {
            var users = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from Follower f left join User u on f.Follower=u.Email
                where f.Followee=@Email" + (request.SinceId == null ? string.Empty : " and u.Id >= @SinceId")
                + " order by u.Name " + request.Order,
                new { Email = request.Email, SinceId = request.SinceId });

            foreach (var user in users)
            {
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
                user.Subscriptions = ReadSubscriptions(user.Email);
            }

            return users.ToList();
        }

        public static List<UserModel> ReadFollowing(this DbConnection cnn, ListFollowing request)
        {
            var users = cnn.Query<UserModel>(
                @"select * from Follower f left join User u on f.Followee=u.Email
                where f.Follower=@Email" + (request.SinceId == null ? string.Empty : " and u.Id >= @SinceId")
                + " order by u.Name " + request.Order,
                new { Email = request.Email, SinceId = request.SinceId });

            foreach (var user in users)
            {
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
                user.Subscriptions = ReadSubscriptions(user.Email);
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

        public static List<UserModel> ReadAllUsers(this IDbConnection cnn, ForumListUsers request)
        {
            var users = cnn.Query<UserModel>(
                @"select distinct u.* from Post p left join User u on p.User = u.Email where p.Forum=@Forum" +
                (request.SinceId == null ? string.Empty : " and u.Id >= @SinceId") +
                (request.Order == null ? string.Empty : " order by u.Name " + request.Order) +
                (request.Limit == null ? string.Empty : " limit @Limit"),
                new
                {
                    Forum = request.Forum,
                    SinceId = request.SinceId,
                    Limit = request.Limit,
                }).Distinct().AsList();

            foreach (var user in users)
            {
                if (user.IsAnonymous)
                {
                    user.About = user.Name = user.Username = null;
                }
                user.Followers = ReadFollowerEmails(user.Email);
                user.Following = ReadFollowingEmails(user.Email);
                user.Subscriptions = ReadSubscriptions(user.Email);
            }

            return users;
        }

        public static void Unfollow(this IDbConnection cnn, Unfollow u)
        {
            cnn.Execute(@"DELETE FROM Follower WHERE Follower=@Follower AND Followee=@Followee", u);
        }

        public static IEnumerable<PostModel<int, string, string, int?>> UserListPosts(this IDbConnection cnn, UserListPosts request)
        {
            var sql = "select * from Post where User=@User" +
                (request.Since == null ? string.Empty : " and Date >= @Since") +
                (request.Order == null ? string.Empty : " order by Date " + request.Order) +
                (request.Limit == null ? string.Empty : " limit @Limit");

            var posts = ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int?>>(
                sql,
                new
                {
                    User = request.User,
                    Since = request.Since,
                    Limit = request.Limit,
                });
            
            return posts;
        }

        public static void Follow(this IDbConnection cnn, Follow request)
        {
            cnn.Execute(
                @"insert into Follower (Follower, Followee) values(@Follower, @Followee)",
                new { Follower = request.Follower, Followee = request.Followee });
        }

        public static int Count()
        {
            return ConnectionProvider.DbConnection.ExecuteScalar<int>(@"select count(*) from User");
        }
    }
}