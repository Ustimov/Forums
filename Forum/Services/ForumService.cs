using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Forum;
using Forum.Helpers;
using Forum.Dtos.Base;

namespace Forum.Services
{
    public class ForumService : Service
    {
        public object Post(CreateForum request)
        {
            try
            {
                ForumCrud.Create(request);

                return new BaseResponse<ForumModel<string>>
                {
                    Code = StatusCode.Ok,
                    Response = ForumCrud.Read(request.ShortName)
                };
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public object Get(ForumDetails request)
        {
            try
            {
                var forum = ForumCrud.Read(request.Forum);

                if (forum == null)
                {
                    return new BaseResponse<string> { Code = StatusCode.ObjectNotFound, Response = "Forum not found" };
                }

                if (request.Related == null)
                {
                    return new BaseResponse<ForumModel<string>> { Code = StatusCode.Ok, Response = forum };
                }
                else if (request.Related.Count == 1 && request.Related.Contains("user"))
                {
                    return new BaseResponse<ForumModel<UserModel>>
                    {
                        Code = StatusCode.Ok,
                        Response = new ForumModel<UserModel>(forum)
                        {
                            User = UserCrud.Read(forum.User),
                        },
                    };
                }

                return new BaseResponse<string>
                {
                    Code = StatusCode.IncorrectRequest,
                    Response = "Incorrect request",
                };
            }
            catch(Exception e)
            {
                throw;
            }
        }

        // Get posts from this forum
        // http://some.host.ru/db/api/forum/listPosts/?related=thread&related=forum&since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListPosts request)
        {
            return new ListPostsResponse
            {
                Code = 0,
                
            };
        }

        // Get posts from this forum
        // http://some.host.ru/db/api/forum/listPosts/?related=thread&related=forum&since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListThreads request)
        {
            return new ListThreadsResponse
            {
                Code = 0,
                Response = new List<ThreadModel<ForumModel<string>, string>>
                {
                    new ThreadModel<ForumModel<string>, string>
                    {
                        Date = DateTime.Parse("2014-01-01 00:00:01"),
                        Dislikes = 0,
                        Forum = new ForumModel<string>
                        {
                            Id = 2,
                            Name = "Forum I",
                            ShortName = "forum1",
                            User = "example3@mail.ru",
                        },
                        Id = 1,
                        IsClosed = true,
                        IsDeleted = true,
                        Likes = 0,
                        Message = "hey hey hey hey!",
                        Points = 0,
                        Posts = 0,
                        Slug = "Threadwithsufficientlylargetitle",
                        Title = "Thread With Sufficiently Large Title",
                        User = "example3@mail.ru",
                    }
                }
            };
        }

        public object Get(ListUsers request)
        {
            return new ListUsersResponse
            {
                Code = 0,
                Response = new List<UserModel>
                {
                    new UserModel
                    {
                        About = null,
                        Email = "richard.nixon@example.com",
                        Followers = new List<string>(),
                        Following = new List<string>(),
                        Id = 2,
                        IsAnonymous = true,
                        Name = null,
                        Subscriptions = new List<int>(),
                        Username = null,
                    }
                }
            };
        }
    }
}
