using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Common;
using Dapper;

namespace Forum.Services
{
    public class HelloService : Service
    {
        class Obj
        {
            int id;
            int Value { get; set; }
            int? val;
        }

        public object Any(Hello request)
        {
            var connection = ConnectionProvider.DbConnection;

            var result = connection.Query<Obj>("select * from T");

            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }

    public class CommonService : Service
    {
        // Truncate all tables
        // TODO: Post
        public object Any(Clear request)
        {
            ConnectionProvider.DbConnection.Execute("delete from User");
            return new ClearResponse { Response = "OK", Code = 0 };
        }

        // Show status info: maps table name to number of rows in that table
        // {"code": 0, "response": {"user": 100000, "thread": 1000, "forum": 100, "post": 1000000}}
        public object Get(Status request)
        {
            return new StatusResponse
            {
                Code = 0,
                Response = new StatusResponseModel
                {
                    User = 100000,
                    Thread = 1000,
                    Forum = 100,
                    Post = 1000000,
                },
            };
        }
    }
} 