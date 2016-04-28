using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Base;
using Forum.Dtos.User;
using Forum.Models;
using Forum.Helpers;

namespace Forum.Services
{
    public class UserService : Service
    {
        public object Post(CreateUser cu)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.CreateUser(cu);
                cu.Id = cnn.LastInsertId();

                return new CreateUserResponse { Code = StatusCode.Ok, Response = cu };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(UserDetails ud)
        {
            try
            {
                var user = ConnectionProvider.DbConnection.ReadUser(ud.Email);

                if (user == null)
                {
                    return new ErrorResponse { Code = StatusCode.ObjectNotFound, Response = "User Not Found" };
                }

                return new UserDetailsResponse { Code = StatusCode.Ok, Response = user };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }

        }

        public object Post(Follow f)
        {
            try
            {
                return new FollowResponse
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadUser(f.Follower),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ListFollowers lf)
        {
            try
            {
                return new BaseResponse<List<UserModel>>
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadFollowers(lf),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ListFollowing lf)
        {
            try
            {
                return new ListFollowingResponse
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadFollowing(lf),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(UserListPosts ulp)
        {
            try
            {
                return new UserListPostsResponse
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.UserListPosts(ulp),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(Unfollow u)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.Unfollow(u);

                return new UnfollowResponse { Code = StatusCode.Ok, Response = cnn.ReadUser(u.Follower) };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(UpdateProfile up)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.UpdateUser(up);

                return new UpdateProfileResponse { Code = StatusCode.Ok, Response = cnn.ReadUser(up.Email) };
            }
            catch(Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
}
