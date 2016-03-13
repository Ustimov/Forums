using System;
using System.Collections.Generic;
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

        public object Post(Follow request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"insert into Follower values(@Follower, @Followee)",
                    new { Follower = request.Follower, Followee = request.Followee });

                return new BaseResponse<UserModel> { Code = StatusCode.Ok, Response = UserCrud.Read(request.Follower) };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Get(ListFollowers request)
        {
            try
            {
                return new BaseResponse<List<UserModel>>
                {
                    Code = StatusCode.Ok,
                    Response = UserCrud.ReadFollowers(request),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Get(ListFollowing request)
        {
            try
            {
                return new BaseResponse<List<UserModel>>
                {
                    Code = StatusCode.Ok,
                    Response = UserCrud.ReadFollowing(request),
                };
            }
            catch (Exception e)
            {
                throw;
            }
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

        public object Post(Unfollow request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"delete from Follower where Follower=@Follower and Followee=@Followee",
                    new { Follower = request.Follower, Followee = request.Followee });

                return new BaseResponse<UserModel> { Code = StatusCode.Ok, Response = UserCrud.Read(request.Follower) };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(UpdateProfile request)
        {
            try
            {
                UserCrud.Update(request);

                return new UpdateProfileResponse { Code = StatusCode.Ok, Response = UserCrud.Read(request.Email) };
            }
            catch(MySqlException e)
            {
                return ErrorResponse.Generate(e);
            }
        }
    }
}
