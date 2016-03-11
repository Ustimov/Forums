using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Base;
using Forum.Dtos.User;
using Forum.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Forum.Helpers;

namespace Forum.Services
{
    public class UserService : Service
    {
        public object Post(Create request)
        {
            try
            {
                UserCrud.Create(request);

                return new CreateResponse
                {
                    Code = StatusCode.Ok,
                    Response = UserCrud.Read(request.Email),
                };
            }
            catch (MySqlException e)
            {
                return ErrorResponse.Generate(e);
            }
        }

        public object Get(Details request)
        {
            var user = UserCrud.Read(request.Email);

            if (user != null)
            {
                return new DetailsResponse { Code = StatusCode.Ok, Response = user };
            }
            else
            {
                return new BaseResponse<string> { Code = StatusCode.ObjectNotFound, Response = "User not found" };
            }
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
            try
            {
                ConnectionProvider.DbConnection.Execute(@"update User
                set About=@About, Name=@Name
                where Email=@Email",
                new { About = request.About, Name = request.Name, Email = request.Email });

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
            catch(MySqlException e)
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
}
