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

        public object Get(ForumListPosts request)
        {
            try
            {
                var posts = PostCrud.ReadAll(request.Forum, null, request.Since, request.Order, request.Limit);

                if (request.Related == null)
                {
                    return new BaseResponse<List<PostModel<int, string, string, int?>>>
                    {
                        Code = StatusCode.Ok,
                        Response = posts,
                    };
                }
                else if (request.Related.Count == 3 && request.Related.Contains("thread")
                    && request.Related.Contains("forum") && request.Related.Contains("user"))
                {
                    var postWithThreadForumAndUser = new List<PostModel<ThreadModel<string, string>, ForumModel<string>,
                        UserModel, int?>>();

                    foreach (var post in posts)
                    {
                        postWithThreadForumAndUser.Add(new PostModel<ThreadModel<string, string>, ForumModel<string>,
                            UserModel, int?>(post)
                        {
                            Thread = ThreadCrud.Read(post.Thread),
                            Forum = ForumCrud.Read(post.Forum),
                            User = UserCrud.Read(post.User),
                            Parent = post.Parent,
                        });
                    }

                    return new BaseResponse<List<PostModel<ThreadModel<string, string>, ForumModel<string>, UserModel, int?>>>
                    {
                        Code = StatusCode.Ok,
                        Response = postWithThreadForumAndUser,
                    };
                }
                else if (request.Related.Count == 2)
                {
                    if (request.Related.Contains("thread") && request.Related.Contains("forum"))
                    {
                        var postWithThreadAndForum = new List<PostModel<ThreadModel<string, string>, ForumModel<string>,
                        string, int?>>();

                        foreach (var post in posts)
                        {
                            postWithThreadAndForum.Add(new PostModel<ThreadModel<string, string>, ForumModel<string>,
                                string, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = ForumCrud.Read(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<ThreadModel<string, string>, ForumModel<string>, string, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithThreadAndForum,
                        };
                    }
                    else if (request.Related.Contains("thread") && request.Related.Contains("user"))
                    {
                        var postWithThreadAndUser = new List<PostModel<ThreadModel<string, string>, string,
                            UserModel, int?>>();

                        foreach (var post in posts)
                        {
                            postWithThreadAndUser.Add(new PostModel<ThreadModel<string, string>, string, UserModel, int?>
                                (post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = post.Forum,
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<ThreadModel<string, string>, string, UserModel, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithThreadAndUser,
                        };
                    }
                    else if (request.Related.Contains("user") && request.Related.Contains("forum"))
                    {
                        var postWithUserAndForum = new List<PostModel<int, ForumModel<string>,
                            UserModel, int?>>();

                        foreach (var post in posts)
                        {
                            postWithUserAndForum.Add(new PostModel<int, ForumModel<string>, UserModel, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = ForumCrud.Read(post.Forum),
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<int, ForumModel<string>, UserModel, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithUserAndForum,
                        };
                    }
                }
                else if (request.Related.Count == 1)
                {
                    if (request.Related.Contains("user"))
                    {
                        var postWithUser = new List<PostModel<int, string, UserModel, int?>>();

                        foreach (var post in posts)
                        {
                            postWithUser.Add(new PostModel<int, string, UserModel, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = post.Forum,
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<int, string, UserModel, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithUser,
                        };
                    }
                    else if (request.Related.Contains("forum"))
                    {
                        var postWithForum = new List<PostModel<int, ForumModel<string>, string, int?>>();

                        foreach (var post in posts)
                        {
                            postWithForum.Add(new PostModel<int, ForumModel<string>, string, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = ForumCrud.Read(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<int, ForumModel<string>, string, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithForum,
                        };
                    }
                    else if (request.Related.Contains("thread"))
                    {
                        var postWithThread = new List<PostModel<ThreadModel<string, string>, string, string, int?>>();

                        foreach (var post in posts)
                        {
                            postWithThread.Add(new PostModel<ThreadModel<string, string>, string, string, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = post.Forum,
                                User = post.User,
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<ThreadModel<string, string>, string, string, int?>>>
                        {
                            Code = StatusCode.Ok,
                            Response = postWithThread,
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

        public object Get(ForumListThreads request)
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

        public object Get(ForumListUsers request)
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
