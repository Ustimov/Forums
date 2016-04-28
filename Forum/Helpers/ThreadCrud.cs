using Forum.Dtos.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Models;
using System.Data;

namespace Forum.Helpers
{
    public static class ThreadCrud
    {
        public static void Create(CreateThread request)
        {
            ConnectionProvider.DbConnection.Execute(
                @"insert into Thread (Forum, Title, IsClosed, User, Date, Message, Slug, IsDeleted, Likes, Dislikes)
                values (@Forum, @Title, @IsClosed, @User, @Date, @Message, @Slug, @IsDeleted, 0, 0)",
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

        public static ThreadModel<string, string> ReadThread(this IDbConnection cnn, int? id)
        {
            var thread = cnn.Query<ThreadModel<string, string>>(
                @"select * from Thread where Id = @Id", new { Id = id }).FirstOrDefault();

            if (thread != null)
            {
                thread.Posts = CountPosts(thread.Id);
            }

            return thread;
        }

        private static int CountPosts(int id)
        {
            return ConnectionProvider.DbConnection.ExecuteScalar<int>(
                @"select count(*) from Post where Thread=@Id and IsDeleted=false", new { Id = id });
        }

        public static ThreadModel<string, string> Read(CreateThread request)
        {
            var thread = ConnectionProvider.DbConnection.Query<ThreadModel<string, string>>(
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

            if (thread != null)
            {
                thread.Posts = CountPosts(thread.Id);
            }

            return thread;
        }

        public static List<ThreadModel<object, object>> ReadAllThreads(this IDbConnection cnn, string forum, string user, DateTime? since,
            string order, int? limit)
        {
            var threads = cnn.Query<ThreadModel<object, object>>(
                @"select * from Thread where IsDeleted=false and " +
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

            foreach (var thread in threads)
            {
                thread.Posts = CountPosts(thread.Id);
            }

            return threads;
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
            return ConnectionProvider.DbConnection.ExecuteScalar<int>(@"select count(*) from Thread");
        }
    }
}