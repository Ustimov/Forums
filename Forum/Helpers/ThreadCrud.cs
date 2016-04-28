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
        public static void CreateThread(this IDbConnection cnn, CreateThread request)
        {
            cnn.Execute(
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

        public static ThreadModel<object, object> ReadThread(this IDbConnection cnn, int? id)
        {
            var thread = cnn.Query<ThreadModel<object, object>>(
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

        public static void LikeThread(this IDbConnection cnn, VoteThread vt)
        {
            cnn.Execute(@"UPDATE Thread SET Likes=Likes+1 WHERE Id=@Thread", vt);
        }

        public static void DislikeThread(this IDbConnection cnn, VoteThread vt)
        {
            cnn.Execute(@"UPDATE Thread SET Dislikes=Dislikes+1 WHERE Id=@Thread", vt);
        }

        public static void UpdateThread(this IDbConnection cnn, UpdateThread ut)
        {
            cnn.Execute(@"UPDATE Thread SET Message=@Message, Slug=@Slug WHERE Id=@Id", ut);
        }

        public static void Unsubscribe(this IDbConnection cnn, Unsubscribe u)
        {
            cnn.Execute(@"DELETE FROM Subscribe WHERE User=@User AND Thread=@Thread", u);
        }

        public static void Subscribe(this IDbConnection cnn, Subscribe s)
        {
            cnn.Execute(@"INSERT INTO Subscribe (User, Thread) VALUE (@User, @Thread)", s);
        }

        public static void RestoreThread(this IDbConnection cnn, RestoreThread rt)
        {
            cnn.Execute(@"UPDATE Thread SET IsDeleted=false WHERE Id=@Thread", rt);
        }

        public static void RestorePosts(this IDbConnection cnn, RestoreThread rt)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=false WHERE Thread=@Thread", rt);
        }

        public static void RemoveThread(this IDbConnection cnn, RemoveThread rt)
        {
            cnn.Execute(@"UPDATE Thread SET IsDeleted=true WHERE Id=@Thread", rt);
        }

        public static void RemovePosts(this IDbConnection cnn, RemoveThread rt)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=true WHERE Thread=@Thread", rt);
        }

        public static void OpenThread(this IDbConnection cnn, OpenThread ot)
        {
            cnn.Execute(@"UPDATE Thread SET IsClosed=false WHERE Id=@Thread", ot);
        }

        public static void CloseThread(this IDbConnection cnn, CloseThread ct)
        {
            cnn.Execute(@"UPDATE Thread SET IsClosed=true WHERE Id=@Thread", ct);
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