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
                (Parent, IsApproved, IsHighlighted, IsEdited, IsSpam, IsDeleted, Date, Thread, Message, User, Forum)
                values (@Parent, @IsApproved, @IsHighlighted, @IsEdited, @IsSpam, @IsDeleted, @Date, @Thread,
                @Message, @User, @Forum)",
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

        public static PostModel<int, string, string, int> Read(CreatePost request)
        {
            return ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int>>(
                @"select * from Post where 
                Parent=@Parent and IsApproved=@IsApproved and IsHighlighted=@IsHighlighted and IsEdited=@IsEdited
                and IsSpam=@IsSpam and IsDeleted=@IsDeleted and Date=@Date and Thread=@Thread and Message=@Message
                and User=@User and Forum=@Forum",
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
                }).FirstOrDefault();
        }

        public static PostModel<int, string, string, int> Read(int id)
        {
            return ConnectionProvider.DbConnection.Query<PostModel<int, string, string, int>>(
                @"select * from Post where Id=@Id", new { Id = id }).FirstOrDefault();
        }

        public static int Count()
        {
            return ConnectionProvider.DbConnection.Query<int>(@"select count (*) Post").FirstOrDefault();
        }
    }
}