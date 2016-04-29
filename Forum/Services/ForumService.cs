using System;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Forum;
using Forum.Extensions;
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

        public object Get(ForumListPosts flp)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                var posts = cnn.ReadAllPosts(flp.Forum, null, flp.Since, flp.Order, flp.Limit);

                if (flp.Related != null)
                {
                    if (flp.Related.Contains("user"))
                    {
                        foreach (var post in posts)
                        {
                            post.User = cnn.ReadUser(post.User as string);
                        }
                    }

                    if (flp.Related.Contains("thread"))
                    {
                        foreach (var post in posts)
                        {
                            post.Thread = cnn.ReadThread(post.Thread as int?);
                        }
                    }

                    if (flp.Related.Contains("forum"))
                    {
                        foreach (var post in posts)
                        {
                            post.Forum = cnn.ReadForum(post.Forum as string);
                        }
                    }
                }

                return new ForumListPostsResponse { Code = StatusCode.Ok, Response = posts };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
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
