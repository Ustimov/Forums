using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Forum.Dtos.Post;
using Forum.Models;

namespace Forum.Extensions
{
    public static class PostExtensions
    {
        public static void CreatePost(this IDbConnection cnn, CreatePost cp)
        {
            cnn.Execute(@"INSERT INTO Post (Parent, IsApproved, IsHighlighted, IsEdited,
                IsSpam, IsDeleted, Date, Thread, Message, User, Forum, Likes, Dislikes)
                VALUES (@Parent, @IsApproved, @IsHighlighted, @IsEdited, @IsSpam, @IsDeleted,
                @Date, @Thread, @Message, @User, @Forum, @Likes, @Dislikes)", cp);
        }

        public static PostModel<object, object, object, object> ReadPost(this IDbConnection cnn, int id)
        {
            return cnn.Query<PostModel<object, object, object, object>>(
                @"SELECT Id, Parent, IsApproved, IsHighlighted, IsEdited, IsSpam, IsDeleted,
                Date, Thread, Message, User, Forum, Likes, Dislikes FROM Post WHERE Id=@Id",
                new { Id = id }).FirstOrDefault();
        }

        public static List<PostModel<object, object, object, object>> ReadAllPosts(this IDbConnection cnn,
            string forum, int? thread, DateTime? since, string order, int? limit, bool isDeleted=false)
        {
            var sql = "select * from Post where " + (isDeleted == false ? "IsDeleted=false and " : string.Empty) +
                (thread == null ? "Forum=@Forum" : "Thread=@Thread") +
                (since == null ? string.Empty : " and Date >= @Since") +
                (order == null ? string.Empty : " order by Date " + order) +
                (limit == null ? string.Empty : " limit @Limit");

            var posts = cnn.Query<PostModel<object, object, object, object>>(
                sql,
                new
                {
                    Forum = forum,
                    Thread = thread,
                    Since = since,
                    Limit = limit,
                }).AsList();

            return posts;
        }

        public static List<int> ReadParents(string order, int? limit, DateTime? since, int thread)
        {
            return ConnectionProvider.DbConnection.Query<int>(
                @"select Id from Post where IsDeleted=false and Thread=@Thread and char_length(Path)=10" + 
                (since == null ? string.Empty : " and Date >= @Since") + 
                " order by Path " + order +
                (limit == null ? string.Empty : " limit @Limit"),
                new { Limit = limit, Since = since, Thread = thread }).AsList();
        }

        public static List<int> ReadChilds(string root, int? limit, DateTime? since, int thread, string order)
        {
            return ConnectionProvider.DbConnection.Query<int>(
                @"select Id from Post where IsDeleted=false and Thread=@Thread and substring(Path, 1, @Length)=@Root" +
                " and Id!=@Post" + (since == null ? string.Empty : " and Date >= @Since") + 
                " order by char_length(Path) asc, Path " + order + (limit == null ? string.Empty : " limit @Limit"),
                new { Limit = limit, Since = since, Thread = thread, Root = root, Post = root, Length = root.Length})
                .AsList();
        }

        public static List<int> ReadChilds(int root, int? limit, DateTime? since, int thread)
        {
            return ConnectionProvider.DbConnection.Query<int>(
                @"select Id from Post where IsDeleted=false and Thread=@Thread and substring(Path, 1, 10)=@Root" +
                " and Id!=@Post" + (since == null ? string.Empty : " and Date >= @Since") + " order by Path " +
                (limit == null ? string.Empty : " limit @Limit"),
                new { Limit = limit, Since = since, Thread = thread, Root = root.ToString("D10"), Post = root }).AsList();
        }

        public static List<int> ReadParentsAndChilds(string order, int? limit, DateTime? since, int thread)
        {
            return ConnectionProvider.DbConnection.Query<int>(
                @"select Id from Post where IsDeleted=false and Thread=@Thread" +
                (since == null ? string.Empty : " and Date >= @Since") +
                " order by Path " + order +
                (limit == null ? string.Empty : " limit @Limit"),
                new { Limit = limit, Since = since, Thread = thread }).AsList();
        }

        public static void LikePost(this IDbConnection cnn, VotePost vp)
        {
            cnn.Execute(@"UPDATE Post SET Likes=Likes+1 WHERE Id=@Post AND IsDeleted=false", vp);
        }

        public static void DislikePost(this IDbConnection cnn, VotePost vp)
        {
            cnn.Execute(@"UPDATE Post SET Dislikes=Dislikes+1 WHERE Id=@Post AND IsDeleted=false", vp);
        }

        public static void UpdatePost(this IDbConnection cnn, UpdatePost up)
        {
            cnn.Execute(@"UPDATE Post SET Message=@Message WHERE Id=@Post AND IsDeleted=false", up);
        }

        public static void RestorePost(this IDbConnection cnn, RestorePost rp)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=false WHERE Id=@Post AND IsDeleted=true", rp);
        }

        public static void RemovePost(this IDbConnection cnn, RemovePost rp)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=true WHERE Id=@Post AND IsDeleted=false", rp);
        }

        public static string ReadPath(this IDbConnection cnn, int? id)
        {
            return cnn.ExecuteScalar<string>(@"SELECT Path FROM Post WHERE Id=@Id AND IsDeleted=false", new { Id = id });
        }

        public static void UpdatePath(this IDbConnection cnn, int id, string parentPath)
        {
            cnn.Execute(@"UPDATE Post SET Path=@Path WHERE Id=@Id AND IsDeleted=false",
                new
                {
                    Id = id,
                    Path = (parentPath == String.Empty ? String.Empty : parentPath + ".") + id.ToString("D10"),
                });
        }

        public static int CountPosts(this IDbConnection cnn)
        {
            return cnn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Post");
        }
    }
}