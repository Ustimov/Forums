using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Forum.Dtos.Post;
using Forum.Models;

namespace Forum.Helpers
{
    public static class PostCrud
    {
        public static void Create(CreatePost request)
        {
            ConnectionProvider.DbConnection.Execute(
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

        public static PostModel<int, string, string, int?> Read(int id)
        {
            return ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int?>>(
                @"select * from Post where Id=@Id", new { Id = id }).FirstOrDefault();
        }

        public static List<PostModel<int, string, string, int?>> ReadAll(string forum, int? thread, DateTime? since,
            string order, int? limit, bool isDeleted=false)
        {
            var sql = "select * from Post where " + (isDeleted == false ? "IsDeleted=false and " : string.Empty) +
                (thread == null ? "Forum=@Forum" : "Thread=@Thread") +
                (since == null ? string.Empty : " and Date >= @Since") +
                (order == null ? string.Empty : " order by Date " + order) +
                (limit == null ? string.Empty : " limit @Limit");

            var posts = ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int?>>(
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

        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) Post").FirstOrDefault();
        }
    }
}