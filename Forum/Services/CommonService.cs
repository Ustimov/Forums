using Dapper;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Common;
using Forum.Helpers;
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

            return new BaseResponse<string> { Code = StatusCode.Ok, Response = "Ok" };
        }

        public object Get(Status request)
        {
            try
            {
                return new BaseResponse<StatusResponseModel>
                {
                    Code = StatusCode.Ok,
                    Response = new StatusResponseModel
                    {
                        User = UserCrud.Count(),
                        Thread = ThreadCrud.Count(),
                        Forum = ForumCrud.Count(),
                        Post = PostCrud.Count(),
                    },
                };
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
} 