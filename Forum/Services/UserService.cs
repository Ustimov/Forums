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
        public object Post(CreateUser request)
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

        public object Get(UserDetails request)
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

        public object Get(UserListPosts request)
        {
            try
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
                    }).AsList();

                return new BaseResponse<List<PostModel<int, string, string, int?>>> { Code = StatusCode.Ok, Response = posts };
            }
            catch (Exception e)
            {
                throw;
            }
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
