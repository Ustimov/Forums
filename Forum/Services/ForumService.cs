using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Forum;
using Forum.Helpers;
using Forum.Dtos.Base;

namespace Forum.Services
{
    public class ForumService : Service
    {
        public object Post(CreateForum cf)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.CreateForum(cf);
                cf.Id = cnn.LastInsertId();

                return new CreateForumResponse { Code = StatusCode.Ok, Response = cf };
            }
            catch(Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ForumDetails fd)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                var forum = cnn.ReadForum(fd.Forum);

                if (forum == null)
                {
                    return new ErrorResponse { Code = StatusCode.ObjectNotFound, Response = "Object Not Found" };
                }
                else if (fd.Related != null && fd.Related.Contains("user"))
                {
                    forum.User = UserCrud.Read(forum.User as string);
                }

                return new ForumDetailsResponse { Code = StatusCode.Ok, Response = forum };
            }
            catch(Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
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
                    var postWithThreadForumAndUser = new List<PostModel<ThreadModel<string, string>, ForumModel<object>,
                        UserModel, int?>>();

                    foreach (var post in posts)
                    {
                        postWithThreadForumAndUser.Add(new PostModel<ThreadModel<string, string>, ForumModel<object>,
                            UserModel, int?>(post)
                        {
                            Thread = ThreadCrud.Read(post.Thread),
                            Forum = ConnectionProvider.DbConnection.ReadForum(post.Forum),
                            User = UserCrud.Read(post.User),
                            Parent = post.Parent,
                        });
                    }

                    return new BaseResponse<List<PostModel<ThreadModel<string, string>, ForumModel<object>, UserModel, int?>>>
                    {
                        Code = StatusCode.Ok,
                        Response = postWithThreadForumAndUser,
                    };
                }
                else if (request.Related.Count == 2)
                {
                    if (request.Related.Contains("thread") && request.Related.Contains("forum"))
                    {
                        var postWithThreadAndForum = new List<PostModel<ThreadModel<string, string>, ForumModel<object>,
                        string, int?>>();

                        foreach (var post in posts)
                        {
                            postWithThreadAndForum.Add(new PostModel<ThreadModel<string, string>, ForumModel<object>,
                                string, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = ConnectionProvider.DbConnection.ReadForum(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<ThreadModel<string, string>, ForumModel<object>, string, int?>>>
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
                        var postWithUserAndForum = new List<PostModel<int, ForumModel<object>,
                            UserModel, int?>>();

                        foreach (var post in posts)
                        {
                            postWithUserAndForum.Add(new PostModel<int, ForumModel<object>, UserModel, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = ConnectionProvider.DbConnection.ReadForum(post.Forum),
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<int, ForumModel<object>, UserModel, int?>>>
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
                        var postWithForum = new List<PostModel<int, ForumModel<object>, string, int?>>();

                        foreach (var post in posts)
                        {
                            postWithForum.Add(new PostModel<int, ForumModel<object>, string, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = ConnectionProvider.DbConnection.ReadForum(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            });
                        }

                        return new BaseResponse<List<PostModel<int, ForumModel<object>, string, int?>>>
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
                    var threadWithUserAndForum = new List<ThreadModel<ForumModel<object>, UserModel>>();
                    foreach (var thread in threads)
                    {
                        threadWithUserAndForum.Add(new ThreadModel<ForumModel<object>, UserModel>(thread)
                        {
                            User = UserCrud.Read(thread.User),
                            Forum = ConnectionProvider.DbConnection.ReadForum(thread.Forum),
                        });
                    }

                    return new BaseResponse<List<ThreadModel<ForumModel<object>, UserModel>>>
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
                        var threadWithForum = new List<ThreadModel<ForumModel<object>, string>>();

                        foreach (var thread in threads)
                        {
                            threadWithForum.Add(new ThreadModel<ForumModel<object>, string>(thread)
                            {
                                User = thread.User,
                                Forum = ConnectionProvider.DbConnection.ReadForum(thread.Forum),
                            });
                        }

                        return new BaseResponse<List<ThreadModel<ForumModel<object>, string>>>
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
