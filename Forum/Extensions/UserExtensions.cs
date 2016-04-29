using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Forum.Models;
using Forum.Dtos.User;
using Forum.Dtos.Forum;

namespace Forum.Extensions
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
                @"SELECT Id, About, Email, IsAnonymous, Name, Username FROM User WHERE Email=@Email",
                new { Email = email }).FirstOrDefault();
            
            if (user == null)
            {
                return user;
            }
            else if (user.IsAnonymous)
            {
                user.About = user.Name = user.Username = null;
            }

            cnn.ReadFollowersAndSubscriptions(user);

            return user;
        }

        private static void ReadFollowersAndSubscriptions(this IDbConnection cnn, UserModel um)
        {
            um.Followers = cnn.ReadFollowerEmails(um.Email);
            um.Following = cnn.ReadFollowingEmails(um.Email);
            um.Subscriptions = cnn.ReadSubscriptions(um.Email);
        }

        public static void UpdateUser(this IDbConnection cnn, UserModel um)
        { 
            cnn.Execute(@"UPDATE User SET About=@About, Name=@Name WHERE Email=@Email", um);
        }

        public static IEnumerable<UserModel> ReadFollowers(this IDbConnection cnn, ListFollowers request)
        {
            var users = cnn.Query<UserModel>(
                @"select * from Follower f left join User u on f.Follower=u.Email
                where f.Followee=@Email" + (request.SinceId == null ? string.Empty : " and u.Id >= @SinceId")
                + " order by u.Name " + request.Order,
                new { Email = request.Email, SinceId = request.SinceId });

            foreach (var user in users)
            {
                cnn.ReadFollowersAndSubscriptions(user);
            }

            return users;
        }

        public static IEnumerable<UserModel> ReadFollowing(this DbConnection cnn, ListFollowing request)
        {
            var users = cnn.Query<UserModel>(
                @"select * from Follower f left join User u on f.Followee=u.Email
                where f.Follower=@Email" + (request.SinceId == null ? string.Empty : " and u.Id >= @SinceId")
                + " order by u.Name " + request.Order,
                new { Email = request.Email, SinceId = request.SinceId });

            foreach (var user in users)
            {
                cnn.ReadFollowersAndSubscriptions(user);
            }

            return users;
        }

        private static IEnumerable<string> ReadFollowerEmails(this IDbConnection cnn, string email)
        {
            return cnn.Query<string>(@"SELECT Follower FROM Follower WHERE Followee=@Email", new { Email = email }); ;
        }

        private static IEnumerable<string> ReadFollowingEmails(this IDbConnection cnn, string email)
        {
            return cnn.Query<string>(@"SELECT Followee FROM Follower WHERE Follower=@Email", new { Email = email });
        }

        private static IEnumerable<int> ReadSubscriptions(this IDbConnection cnn, string email)
        {
            return cnn.Query<int>(@"SELECT Thread FROM Subscribe WHERE User=@Email", new { Email = email });
        }

        public static IEnumerable<UserModel> ReadAllUsers(this IDbConnection cnn, ForumListUsers request)
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
                }).Distinct();

            foreach (var user in users)
            {
                if (user.IsAnonymous)
                {
                    user.About = user.Name = user.Username = null;
                }

                cnn.ReadFollowersAndSubscriptions(user);
            }

            return users;
        }

        public static void Unfollow(this IDbConnection cnn, Unfollow u)
        {
            cnn.Execute(@"DELETE FROM Follower WHERE Follower=@Follower AND Followee=@Followee", u);
        }

        public static IEnumerable<PostModel<int, string, string, int?>> UserListPosts(
            this IDbConnection cnn, UserListPosts request)
        {
            var sql = "select * from Post where User=@User" +
                (request.Since == null ? string.Empty : " and Date >= @Since") +
                (request.Order == null ? string.Empty : " order by Date " + request.Order) +
                (request.Limit == null ? string.Empty : " limit @Limit");

            var posts = cnn.Query<PostModel<int, string, string, int?>>(
                sql,
                new
                {
                    User = request.User,
                    Since = request.Since,
                    Limit = request.Limit,
                });
            
            return posts;
        }

        public static void Follow(this IDbConnection cnn, Follow f)
        {
            cnn.Execute(@"INSERT INTO Follower (Follower, Followee) VALUES (@Follower, @Followee)", f);
        }

        public static int CountUsers(this IDbConnection cnn)
        {
            return cnn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM User");
        }
    }
}