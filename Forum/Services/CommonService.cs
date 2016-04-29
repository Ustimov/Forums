using System;
using Dapper;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Common;
using Forum.Extensions;
using Forum.Dtos.Base;

namespace Forum.Services
{
    public class CommonService : Service
    {
        public object Post(Clear request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"SET FOREIGN_KEY_CHECKS = 0;
                    TRUNCATE Subscribe;
                    TRUNCATE Post;
                    TRUNCATE Thread;
                    TRUNCATE Forum;
                    TRUNCATE Follower;
                    TRUNCATE User;
                    SET FOREIGN_KEY_CHECKS = 1;");

                return new ClearResponse { Code = StatusCode.Ok, Response = "Ok" };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(Status s)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;

                return new StatusResponse
                {
                    Code = StatusCode.Ok,
                    Response = new StatusResponseModel
                    {
                        User = cnn.CountUsers(),
                        Thread = cnn.CountThreads(),
                        Forum = cnn.CountForums(),
                        Post = cnn.CountPosts(),
                    },
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
}
