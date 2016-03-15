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
            throw new NotImplementedException();

            return new ListPostsResponse
            {
                Code = 0,
                
            };
        }

        public object Get(ListThreads request)
        {
            try
            {
                var threads = ThreadCrud.ReadAll(request.Forum, null, request.Since, request.Order, request.Limit);

                if (request.Related == null)
                {
                    return new BaseResponse<List<ThreadModel<string, string>>> { Code = StatusCode.Ok, Response = threads };
                }
                else if (request.Related.Count == 2 && request.Related.Contains("user") && request.Related.Contains("forum"))
                {
                    var threadWithUserAndForum = new List<ThreadModel<ForumModel<string>, UserModel>>();
                    foreach (var thread in threads)
                    {
                        threadWithUserAndForum.Add(new ThreadModel<ForumModel<string>, UserModel>(thread)
                        {
                            User = UserCrud.Read(thread.User),
                            Forum = ForumCrud.Read(thread.Forum),
                        });
                    }

                    return new BaseResponse<List<ThreadModel<ForumModel<string>, UserModel>>>
                    {
                        Code = StatusCode.Ok,
                        Response = threadWithUserAndForum,
                    };
                }
                else if (request.Related.Count == 1)
                {
                    if (request.Related.Contains("user"))
                    {
                        var threadWithUser = new List<ThreadModel<string, UserModel>>();
                        
                        foreach (var thread in threads)
                        {
                            threadWithUser.Add(new ThreadModel<string, UserModel>(thread)
                            {
                                User = UserCrud.Read(thread.User),
                                Forum = thread.Forum,
                            });
                        }

                        return new BaseResponse<List<ThreadModel<string, UserModel>>>
                        {
                            Code = StatusCode.Ok,
                            Response = threadWithUser,
                        };
                    }
                    else if (request.Related.Contains("forum"))
                    {
                        var threadWithForum = new List<ThreadModel<ForumModel<string>, string>>();

                        foreach (var thread in threads)
                        {
                            threadWithForum.Add(new ThreadModel<ForumModel<string>, string>(thread)
                            {
                                User = thread.User,
                                Forum = ForumCrud.Read(thread.Forum),
                            });
                        }

                        return new BaseResponse<List<ThreadModel<ForumModel<string>, string>>>
                        {
                            Code = StatusCode.Ok,
                            Response = threadWithForum,
                        };
                    }
                }

                return new BaseResponse<string> { Code = StatusCode.UndefinedError, Response = "Undefined error" };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Get(ListUsers request)
        {
            try
            {
                return new BaseResponse<List<UserModel>> { Code = StatusCode.Ok, Response = UserCrud.ReadAll(request) };
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
