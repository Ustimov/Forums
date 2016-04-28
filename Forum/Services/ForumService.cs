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
                    forum.User = cnn.ReadUser(forum.User as string);
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
                            User = ConnectionProvider.DbConnection.ReadUser(post.User),
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
                                User = ConnectionProvider.DbConnection.ReadUser(post.User),
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
                                User = ConnectionProvider.DbConnection.ReadUser(post.User),
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
                                User = ConnectionProvider.DbConnection.ReadUser(post.User),
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

        public object Get(ForumListThreads flt)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                var threads = cnn.ReadAllThreads(flt.Forum, null, flt.Since, flt.Order, flt.Limit);

                if (flt.Related != null)
                {
                    if (flt.Related.Contains("user"))
                    {
                        foreach (var thread in threads)
                        {
                            thread.User = cnn.ReadUser(thread.User as string);
                        }
                    }

                    if (flt.Related.Contains("forum"))
                    {
                        foreach (var thread in threads)
                        {
                            thread.Forum = cnn.ReadForum(thread.Forum as string);
                        }
                    }
                }

                return new ForumListThreadsResponse { Code = StatusCode.Ok, Response = threads };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ForumListUsers request)
        {
            try
            {
                return new ForumListUsersResponse
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadAllUsers(request),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
}
