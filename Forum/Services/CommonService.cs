using Dapper;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Common;
using Forum.Extensions;
using Forum.Dtos.Base;
using System;

namespace Forum.Services
{
    public class CommonService : Service
    {
        public object Post(Clear request)
        {
            ConnectionProvider.DbConnection.Execute("delete from Subscribe");
            ConnectionProvider.DbConnection.Execute("delete from Post");
            ConnectionProvider.DbConnection.Execute("delete from Thread");
            ConnectionProvider.DbConnection.Execute("delete from Forum");
            ConnectionProvider.DbConnection.Execute("delete from Follower");
            ConnectionProvider.DbConnection.Execute("delete from User");

            return new ClearResponse { Code = StatusCode.Ok, Response = "Ok" };
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