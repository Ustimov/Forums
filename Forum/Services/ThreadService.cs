using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Base;
using Forum.Dtos.Thread;
using Forum.Models;
using Forum.Helpers;

namespace Forum.Services
{
    public class ThreadService : Service
    {
        public object Post(CloseThread ct)
        {
            try
            {
                ConnectionProvider.DbConnection.CloseThread(ct);

                return new CloseThreadResponse { Code = StatusCode.Ok, Response = ct.Thread };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.Ok, Response = e.Message };
            }
        }

        public object Post(CreateThread ct)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.CreateThread(ct);
                ct.Id = cnn.LastInsertId();

                return new CreateThreadResponse { Code = StatusCode.Ok, Response = ct };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ThreadDetails td)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                var thread = cnn.ReadThread(td.Thread);

                if (thread == null)
                {
                    return new ErrorResponse { Code = StatusCode.ObjectNotFound, Response = "Thread Not Found" };
                }

                if (td.Related != null)
                {
                    if (td.Related.Contains("thread"))
                    {
                        return new ErrorResponse { Code = StatusCode.IncorrectRequest, Response = "Incorrect Request" };
                    }

                    if (td.Related.Contains("user"))
                    {
                        thread.User = cnn.ReadUser(thread.User as string);
                    }

                    if (td.Related.Contains("forum"))
                    {
                        thread.Forum = cnn.ReadForum(thread.Forum as string);
                    }
                }

                return new ThreadDetailsResponse { Code = StatusCode.Ok, Response = thread };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(ThreadListThreads tlt)
        {
            try
            {   
                return new ThreadListThreadsResponse
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadAllThreads(
                        tlt.Forum, tlt.User, tlt.Since, tlt.Order, tlt.Limit),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        #region NEED REFACTOR

        private void AddPosts(List<PostModel<object, object, object, object>> posts, int postId, DateTime? since, int thread,
            string order, int? limit)
        {
            if (limit != null && limit == posts.Count)
            {
                return;
            }

            var exist = posts.Find((p) => p.Id == postId);
            if (exist != null)
            {
                return;
            }

            var post = ConnectionProvider.DbConnection.ReadPost(postId);
            posts.Add(post);

            var path = PostExtensions.ReadPath(post.Id);

            var childs = PostExtensions.ReadChilds(path, null, since, thread, order);

            foreach (var child in childs)
            {
                if (limit != null && limit == posts.Count)
                {
                    return;
                }

                AddPosts(posts, child, since, thread, order, limit);
            }
        }

        public object Get(ThreadListPosts request)
        {
            try
            {
                List<PostModel<object, object, object, object>> posts = new List<PostModel<object, object, object, object>>();

                if (request.Sort == "flat")
                {
                    posts = ConnectionProvider.DbConnection.ReadAllPosts(request.Forum, request.Thread, request.Since,
                        request.Order, request.Limit, true);
                }
                else if (request.Sort == "tree")
                {
                    var ids = PostExtensions.ReadParents(request.Order, request.Limit, request.Since, request.Thread);

                    foreach (var id in ids)
                    {
                        if (request.Limit != null && request.Limit == posts.Count)
                        {
                            break;
                        }

                        AddPosts(posts, id, request.Since, request.Thread, request.Order, request.Limit);
                    }
                }
                else if (request.Sort == "parent_tree")
                {
                    var ids = PostExtensions.ReadParents(request.Order, request.Limit, request.Since, request.Thread);

                    foreach (var id in ids)
                    {
                        var post = ConnectionProvider.DbConnection.ReadPost(id);
                        posts.Add(post);

                        var childs = PostExtensions.ReadChilds(post.Id, null, request.Since,
                            request.Thread);

                        foreach (var child in childs)
                        {
                            posts.Add(ConnectionProvider.DbConnection.ReadPost(child));
                        }
                    }
                }

                return new BaseResponse<List<PostModel<object, object, object, object>>>
                {
                    Code = StatusCode.Ok,
                    Response = posts,
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        public object Post(OpenThread ot)
        {
            try
            {
                ConnectionProvider.DbConnection.OpenThread(ot);

                return new OpenThreadResponse { Code = StatusCode.Ok, Response = ot.Thread };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(RemoveThread rt)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.RemoveThread(rt);
                cnn.RemovePosts(rt);

                return new RemoveThreadResponse { Code = StatusCode.Ok, Response = rt.Thread };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(RestoreThread rt)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.RestoreThread(rt);
                cnn.RestorePosts(rt);

                return new RestoreThreadResponse { Code = StatusCode.Ok, Response = rt.Thread };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(Subscribe u)
        {
            try
            {
                ConnectionProvider.DbConnection.Subscribe(u);

                return new SubscribeResponse { Code = StatusCode.Ok, Response = u };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(Unsubscribe u)
        {
            try
            {
                ConnectionProvider.DbConnection.Unsubscribe(u);

                return new UnsubscribeResponse { Code = StatusCode.Ok, Response = u };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(UpdateThread ut)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.UpdateThread(ut);

                return new UpdateThreadResponse
                {
                    Code = StatusCode.Ok,
                    Response = cnn.ReadThread(ut.Thread),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(VoteThread request)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;

                if (request.Value == 1)
                {
                    cnn.LikeThread(request);
                }
                else if (request.Value == -1)
                {
                    cnn.DislikeThread(request);
                }

                return new VoteThreadResponse
                {
                    Code = StatusCode.Ok,
                    Response = cnn.ReadThread(request.Thread),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
}
