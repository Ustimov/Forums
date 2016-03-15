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

        public static ThreadModel<string, string> Read(CreateThread request)
        {
            return ConnectionProvider.DbConnection.Query<ThreadModel<string, string>>(
                @"select * from Thread where Forum = @Forum and Title = @Title and IsClosed = @IsClosed and
                User = @User and Date = @Date and Message = @Message and Slug = @Slug and IsDeleted = @IsDeleted", 
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
                }).FirstOrDefault();
        }

        public static List<ThreadModel<string, string>> ReadAll(string forum, string user, DateTime? since,
            string order, int? limit)
        {
            return ConnectionProvider.DbConnection.Query<ThreadModel<string, string>>(
                @"select * from Thread where " +
                (user == null ? "Forum=@Forum" : "User=@User") +
                (since == null ? string.Empty : " and Date >= @Since") +
                (order == null ? string.Empty : " order by Date " + order) +
                (limit == null ? string.Empty : " limit @Limit"),
                new
                {
                    Forum = forum,
                    User = user,
                    Since = since,
                    Limit = limit,
                }).AsList();
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