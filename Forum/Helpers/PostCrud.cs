﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Dtos.Post;
using Forum.Models;
using System.Data;

namespace Forum.Helpers
{
    public static class PostCrud
    {
        public static void CreatePost(this IDbConnection cnn, CreatePost request)
        {
            cnn.Execute(
                @"insert into Post 
                (Parent, IsApproved, IsHighlighted, IsEdited, IsSpam, IsDeleted, Date, Thread, Message, User, Forum,
                Likes, Dislikes)
                values (@Parent, @IsApproved, @IsHighlighted, @IsEdited, @IsSpam, @IsDeleted, @Date, @Thread,
                @Message, @User, @Forum, 0, 0)",
                new
                {
                    Parent = request.Parent,
                    IsApproved = request.IsApproved,
                    IsHighlighted = request.IsHighlighted,
                    IsEdited = request.IsEdited,
                    IsSpam = request.IsSpam,
                    IsDeleted = request.IsDeleted,
                    Date = request.Date,
                    Thread = request.Thread,
                    Message = request.Message,
                    User = request.User,
                    Forum = request.Forum,
                });
        }

        public static PostModel<int, string, string, int?> Read(CreatePost request)
        {
            return ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int?>>(
                @"select * from Post where 
                IsApproved=@IsApproved and IsHighlighted=@IsHighlighted and IsEdited=@IsEdited
                and IsSpam=@IsSpam and IsDeleted=@IsDeleted and Date=@Date and Thread=@Thread and Message=@Message
                and User=@User and Forum=@Forum",
                new
                {
                    IsApproved = request.IsApproved,
                    IsHighlighted = request.IsHighlighted,
                    IsEdited = request.IsEdited,
                    IsSpam = request.IsSpam,
                    IsDeleted = request.IsDeleted,
                    Date = request.Date,
                    Thread = request.Thread,
                    Message = request.Message,
                    User = request.User,
                    Forum = request.Forum,
                }).FirstOrDefault();
        }

        public static PostModel<object, object, object, object> ReadPost(this IDbConnection cnn, int id)
        {
            return cnn.Query<PostModel<object, object, object, object>>(
                @"select * from Post where Id=@Id", new { Id = id }).FirstOrDefault();
        }

        public static List<PostModel<object, object, object, object>> ReadAllPosts(this IDbConnection cnn, string forum, int? thread, DateTime? since,
            string order, int? limit, bool isDeleted=false)
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

        public static string ReadPath(int id)
        {
            return ConnectionProvider.DbConnection.ExecuteScalar<string>(
                @"select Path from Post where Id=@Post", new { Post = id });
        }

        public static void LikePost(this IDbConnection cnn, VotePost vp)
        {
            cnn.Execute(@"UPDATE Post SET Likes=Likes+1 WHERE Id=@Post", vp);
        }

        public static void DislikePost(this IDbConnection cnn, VotePost vp)
        {
            cnn.Execute(@"UPDATE Post SET Dislikes=Dislikes+1 WHERE Id=@Id", vp);
        }

        public static void UpdatePost(this IDbConnection cnn, UpdatePost up)
        {
            cnn.Execute(@"UPDATE Post SET Message=@Message WHERE Id=@Post", up);
        }

        public static void RestorePost(this IDbConnection cnn, RestorePost rp)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=false WHERE Id=@Post", rp);
        }

        public static void RemovePost(this IDbConnection cnn, RemovePost rp)
        {
            cnn.Execute(@"UPDATE Post SET IsDeleted=true WHERE Id=@Post", rp);
        }

        public static int Count()
        {
            return ConnectionProvider.DbConnection.ExecuteScalar<int>(@"select count(*) from Post");
        }
    }
}