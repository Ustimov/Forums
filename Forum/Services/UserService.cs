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

namespace Forum.Services
{
    public class UserService : Service
    {
        // Create new user
        // {"username": "user1", "about": "hello im user1", "isAnonymous": false, "name": "John", "email": "example@mail.ru"}
        public object Post(Create request)
        {
            /*
            if (string.IsNullOrEmpty(request.About) || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Username))
            {
                return new CreateResponse { Code = StatusCode.IncorrectRequest };
            }
            */
            try
            {
                if (!request.IsAnonymous)
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
                }
                else
                {
                    ConnectionProvider.DbConnection.Execute(
                    @"insert into User
                                    (About, Email, IsAnonymous, Name, Username)
                                    values (@About, @Email, @IsAnonymous, @Name, @Username)",
                    new
                    {
                        About = "Lal",
                        Email = request.Email,
                        // Optional
                        IsAnonymous = request.IsAnonymous,
                        Name = "Lal",
                        Username = "Lal",
                    });
                }

                var user = ConnectionProvider.DbConnection.Query<UserModel>(
                    @"select About, Email, IsAnonymous, Name, Username, Id
                    from User
                    where Email = @Email",
                    new { Email = request.Email }).LastOrDefault();

                user.Subscriptions = new List<int>();
                user.Followers = new List<string>();
                user.Following = new List<string>();

                return new CreateResponse
                {
                    Code = 0,
                    Response = user,
                };

            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    return new BaseResponse<string> { Code = (int)StatusCode.UserAlreadyExists, Response = e.Message };
                }
                else if (e.Number == 1048)
                {
                    return new BaseResponse<string> { Code = (int)StatusCode.IncorrectRequest, Response = e.Message };
                }
                else
                {
                    return new BaseResponse<string> { Code = (int)StatusCode.UndefinedError, Response = e.Message };
                }
            }
        }

        // Get user details
        // user/details/?user=example%40mail.ru:
        public object Get(Details request)
        {
            var user = ConnectionProvider.DbConnection.Query<UserModel>(
                @"select * from User where Email = @Email", new { Email = request.User }).FirstOrDefault();

            if (user == null)
            {
                return new BaseResponse<string> { Code = (int)StatusCode.ObjectNotFound, Response = "User not found" };
            }
            else if (user.IsAnonymous)
            {
                return new CreateResponse
                {
                    Code = (int)StatusCode.Ok,
                    Response = new UserModel
                    {
                        IsAnonymous = true,
                        Email = user.Email,
                        Id = user.Id,
                        Subscriptions = new List<int>(),
                        Followers = new List<string>(),
                        Following = new List<string>(),
                    }
                };
            }

            user.Subscriptions = new List<int>();
            user.Followers = new List<string>();
            user.Following = new List<string>();

            return new DetailsResponse
            {
                Code = 0,
                Response = user,
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
