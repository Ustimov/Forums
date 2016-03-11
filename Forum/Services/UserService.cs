using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.User;
using Forum.Models;
using Dapper;

namespace Forum.Services
{
    public class UserService : Service
    {
        // Create new user
        // {"username": "user1", "about": "hello im user1", "isAnonymous": false, "name": "John", "email": "example@mail.ru"}
        public object Post(Create request)
        {
            ConnectionProvider.DbConnection.Execute(
                @"insert into User
                (About, Email, IsAnonymous, Name, Username)
                values (@About, @Email, @IsAnonymous, @Name, @Username)",
                new
                {
                    About = request.About,
                    Email = request.Email,
                    // Optional
                    IsAnonymous = request.IsAnonymous,
                    Name = request.Name,
                    Username = request.Username,
                });

            var user = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select About, Email, IsAnonymous, Name, Username, Id
                from User
                where Email = @Email",
                new { Email = request.Email });

            return new CreateResponse
            {
                Code = 0,
                Response = user.FirstOrDefault(),
            };
        }

        // Get user details
        // user/details/?user=example%40mail.ru:
        public object Get(Details request)
        {
            var user = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from User where Email = @Email", new { Email = request.User });

            return new DetailsResponse
            {
                Code = 0,
                Response = user.FirstOrDefault(),
                /*new UserModel
                {
                    About = "hello im user1",
                    Email = "example@mail.ru",
                    Id = 1,
                    IsAnonymous = false,
                    Username = "user1",
                    Name = "John",
                    Subscriptions = new List<int> { 4 },
                    Followers = new List<string> { "example3@mail.ru" },
                    Following = new List<string> { "example3@mail.ru" }
                },
                */
            };
        }

        // Mark one user as folowing other user
        // {"follower": "example@mail.ru", "followee": "example3@mail.ru"}
        public object Post(Follow request)
        {
            return new FollowResponse
            {
                Code = 0,
                Response = new UserModel
                {
                    About = "hello im user1",
                    Email = "example@mail.ru",
                    Id = 1,
                    IsAnonymous = false,
                    Username = "user1",
                    Name = "John",
                    Subscriptions = new List<int> { 4 },
                    Followers = new List<string> { "example3@mail.ru" },
                    Following = new List<string> { "example3@mail.ru" }
                },
            };
        }

        // Get followers of this user
        // user/listFollowers/?user=example%40mail.ru&order=asc
        public object Get(ListFollowers request)
        {
            return new ListFollowersResponse
            {
                Code = 0,
                Response = new List<UserModel>
                {
                    new UserModel
                    {
                        About = "hello im user1",
                        Email = "example@mail.ru",
                        Id = 1,
                        IsAnonymous = false,
                        Username = "user1",
                        Name = "John",
                        Subscriptions = new List<int> { 4 },
                        Followers = new List<string> { "example3@mail.ru" },
                        Following = new List<string> { "example3@mail.ru" }
                    },
                },
            };
        }

        // Get followees of this user
        // user/listFollowing/?limit=3&user=example3%40mail.ru&since_id=1&order=desc
        public object Get(ListFollowing request)
        {
            return new ListFollowingResponse
            {
                Code = 0,
                Response = new List<UserModel>
                {
                    new UserModel
                    {
                        About = "hello im user1",
                        Email = "example@mail.ru",
                        Id = 1,
                        IsAnonymous = false,
                        Username = "user1",
                        Name = "John",
                        Subscriptions = new List<int> { 4 },
                        Followers = new List<string> { "example3@mail.ru" },
                        Following = new List<string> { "example3@mail.ru" }
                    },
                }
            };
        }

        // Get posts from this user
        // user/listPosts/?since=2014-01-02+00%3A00%3A00&limit=2&user=example%40mail.ru&order=asc:
        public object Get(ListPosts request)
        {
            return new ListPostsResponse
            {
                Code = 0,
                Response = new List<PostModel<int, string>>
                {
                    new PostModel<int, string>
                    {
                        Date = new DateTime(),
                        Dislikes = 0,
                        Forum = "forum1",
                        Id = 5,
                        IsApproved = false,
                        IsDeleted = true,
                        IsEdited = false,
                        IsHighlighted = false,
                        IsSpam = false,
                        Likes = 0,
                        Message = "my message 1",
                        Points = 0,
                        Thread = 4,
                        User = "richard.nixon@example.com",
                    }
                },
            };
        }

        // Mark one user as not folowing other user anymore
        // {"follower": "example@mail.ru", "followee": "example3@mail.ru"}
        public object Post(Unfollow request)
        {
            return new UnfollowResponse
            {
                Code = 0,
                Response = new UserModel
                {
                    About = "hello im user1",
                    Email = "example@mail.ru",
                    Id = 1,
                    IsAnonymous = false,
                    Username = "user1",
                    Name = "John",
                    Subscriptions = new List<int> { 4 },
                    Followers = new List<string> { "example3@mail.ru" },
                    Following = new List<string> { "example3@mail.ru" }
                },
            };
        }

        // Update profile
        // {"about": "Wowowowow!!!", "user": "example3@mail.ru", "name": "NewName2"}
        public object Post(UpdateProfile request)
        {
            return new UpdateProfileResponse
            {
                Code = 0,
                Response = new UserModel
                {
                    About = "hello im user1",
                    Email = "example@mail.ru",
                    Id = 1,
                    IsAnonymous = false,
                    Username = "user1",
                    Name = "John",
                    Subscriptions = new List<int> { 4 },
                    Followers = new List<string> { "example3@mail.ru" },
                    Following = new List<string> { "example3@mail.ru" }
                },
            };
        }
    }
}
