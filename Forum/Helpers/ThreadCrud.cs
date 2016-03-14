using Forum.Dtos.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Models;

namespace Forum.Helpers
{
    public class ThreadCrud
    {
        public static void Create(CreateThread request)
        {
            ConnectionProvider.DbConnection.Execute(
                @"insert into Thread (Forum, Title, IsClosed, User, Date, Message, Slug, IsDeleted)
                values (@Forum, @Title, @IsClosed, @User, @Date, @Message, @Slug, @IsDeleted)",
                new
                {
                    Forum = request.Forum,
                    Title = request.Title,
                    IsClosed = request.IsClosed,
                    User = request.User,
                    Date = request.Date,
                    Message = request.Message,
                    Slug = request.Slug,
                    IsDeleted = request.IsDeleted,
                });
        }

        public static ThreadModel<string, string> Read(int id)
        {
            return ConnectionProvider.DbConnection.Query<ThreadModel<string, string>>(
                @"select * from Thread where Id = @Id", new { Id = id }).FirstOrDefault();
        }

        /*
        public static ThreadModel<ForumModel, string> ReadWithForum(int id)
        {
            var thread = Read(id);

            ThreadModel<ForumModel, string> threadWithForum = new ThreadModel<ForumModel, string>(thread);
            threadWithForum.Forum = ForumCrud.Read(thread.Forum);

            return threadWithForum;

        }
        
        public static ThreadModel<string, UserModel> ReadWithUser(int id)
        {
            var thread = Read(id);

            ThreadModel<string, UserModel> threadWithUser = new ThreadModel<string, UserModel>(thread);
            threadWithUser.User = UserCrud.Read(thread.User);

            return threadWithUser;
        }

        public static ThreadModel<ForumModel, UserModel> ReadWith
        */
        
        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) Thread").FirstOrDefault();
        }
    }
}