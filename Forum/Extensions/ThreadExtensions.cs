using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Forum.Models;
using Forum.Dtos.Thread;

namespace Forum.Extensions
{
    public static class ThreadExtensions
    {
        public static void CreateThread(this IDbConnection cnn, CreateThread ct)
        {
            cnn.Execute(
                @"INSERT INTO Thread (Forum, Title, IsClosed, User, Date, Message, Slug, IsDeleted, Likes, Dislikes)
                VALUES (@Forum, @Title, @IsClosed, @User, @Date, @Message, @Slug, @IsDeleted, @Likes, @Dislikes)", ct);
        }

        public static ThreadModel<object, object> ReadThread(this IDbConnection cnn, int? id)
        {
            var thread = cnn.Query<ThreadModel<object, object>>(
                @"SELECT Id, Forum, Title, IsClosed, User, Date, Message, Slug, IsDeleted, Likes, Dislikes
                FROM Thread WHERE Id = @Id", new { Id = id }).FirstOrDefault();

            if (thread != null)
            {
                thread.Posts = cnn.CountThreadPosts(thread.Id);
            }

            return thread;
        }

        private static int CountThreadPosts(this IDbConnection cnn, int thread)
        {
            return cnn.ExecuteScalar<int>(
                @"SELECT COUNT(*) FROM Post WHERE Thread=@Id AND IsDeleted=false", new { Id = thread });
        }

        public static List<ThreadModel<object, object>> ReadAllThreads(this IDbConnection cnn,
            string forum, string user, DateTime? since, string order, int? limit)
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
                thread.Posts = cnn.CountThreadPosts(thread.Id);
            }

            return threads;
        }

        public static void LikeThread(this IDbConnection cnn, VoteThread vt)
        {
            cnn.Execute(@"UPDATE Thread SET Likes=Likes+1 WHERE Id=@Thread AND IsDeleted=false AND IsClosed=false", vt);
        }

        public static void DislikeThread(this IDbConnection cnn, VoteThread vt)
        {
            cnn.Execute(@"UPDATE Thread SET Dislikes=Dislikes+1
                WHERE Id=@Thread AND IsDeleted=false AND IsClosed=false", vt);
        }

        public static void UpdateThread(this IDbConnection cnn, UpdateThread ut)
        {
            cnn.Execute(@"UPDATE Thread SET Message=@Message, Slug=@Slug
                WHERE Id=@Thread AND IsDeleted=false AND IsClosed=false", ut);
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
            cnn.Execute(@"UPDATE Thread SET IsDeleted=false WHERE Id=@Thread AND IsDeleted=true", rt);
        }

        public static void RestorePosts(this IDbConnection cnn, RestoreThread rt)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=false WHERE Thread=@Thread AND IsDeleted=true", rt);
        }

        public static void RemoveThread(this IDbConnection cnn, RemoveThread rt)
        {
            cnn.Execute(@"UPDATE Thread SET IsDeleted=true WHERE Id=@Thread AND IsDeleted=false", rt);
        }

        public static void RemovePosts(this IDbConnection cnn, RemoveThread rt)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=true WHERE Thread=@Thread AND IsDeleted=false", rt);
        }

        public static void OpenThread(this IDbConnection cnn, OpenThread ot)
        {
            cnn.Execute(@"UPDATE Thread SET IsClosed=false WHERE Id=@Thread AND IsClosed=true", ot);
        }

        public static void CloseThread(this IDbConnection cnn, CloseThread ct)
        {
            cnn.Execute(@"UPDATE Thread SET IsClosed=true WHERE Id=@Thread AND IsClosed=false", ct);
        }
        
        public static int Count()
        {
            return ConnectionProvider.DbConnection.ExecuteScalar<int>(@"select count(*) from Thread");
        }
    }
}