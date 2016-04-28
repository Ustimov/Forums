using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Base;
using Forum.Dtos.Thread;
using Forum.Models;
using Forum.Helpers;
using Dapper;
using MySql.Data.MySqlClient;

namespace Forum.Services
{
    public class ThreadService : Service
    {
        public object Post(CloseThread request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Thread set IsClosed=true where Id=@Id", new { Id = request.Thread });

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Thread };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(CreateThread request)
        {
            try
            {
                ThreadCrud.Create(request);

                return new BaseResponse<ThreadModel<string, string>>
                {
                    Code = StatusCode.Ok,
                    Response = ThreadCrud.Read(request)
                };
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public object Get(ThreadDetails request)
        {
            try
            {
                var thread = ThreadCrud.Read(request.Thread);

                if (thread == null)
                {
                    return new BaseResponse<string> { Code = StatusCode.ObjectNotFound, Response = "Thread not found" };
                }

                if (request.Related == null)
                {
                    return new BaseResponse<ThreadModel<string, string>>
                    {
                        Code = StatusCode.Ok,
                        Response = thread,
                    };
                }
                else if (request.Related.Count == 2 && request.Related.Contains("user") && request.Related.Contains("forum"))
                {
                    return new BaseResponse<ThreadModel<ForumModel<object>, UserModel>>
                    {
                        Code = StatusCode.Ok,
                        Response = new ThreadModel<ForumModel<object>, UserModel>(thread)
                        {
                            Forum = ConnectionProvider.DbConnection.ReadForum(thread.Forum),
                            User = ConnectionProvider.DbConnection.ReadUser(thread.User),
                        },
                    };
                }
                else if (request.Related.Count == 1)
                {
                    if (request.Related.Contains("forum"))
                    {
                        return new BaseResponse<ThreadModel<ForumModel<object>, string>>
                        {
                            Code = StatusCode.Ok,
                            Response = new ThreadModel<ForumModel<object>, string>(thread)
                            {
                                Forum = ConnectionProvider.DbConnection.ReadForum(thread.Forum),
                                User = thread.User,
                            },
                        };
                    }
                    else if (request.Related.Contains("user"))
                    {
                        return new BaseResponse<ThreadModel<string, UserModel>>
                        {
                            Code = StatusCode.Ok,
                            Response = new ThreadModel<string, UserModel>(thread)
                            {
                                Forum = thread.Forum,
                                User = ConnectionProvider.DbConnection.ReadUser(thread.User),
                            },
                        };
                    }
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

        public object Get(ThreadListThreads request)
        {
            try
            {   
                return new BaseResponse<List<ThreadModel<object, object>>>
                {
                    Code = StatusCode.Ok,
                    Response = ConnectionProvider.DbConnection.ReadAllThreads(request.Forum, request.User, request.Since, request.Order, request.Limit),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void AddPosts(List<PostModel<int, string, string, int?>> posts, int postId, DateTime? since, int thread,
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

            var post = PostCrud.Read(postId);
            posts.Add(post);

            var path = PostCrud.ReadPath(post.Id);

            var childs = PostCrud.ReadChilds(path, null, since, thread, order);

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
                List<PostModel<int, string, string, int?>> posts = new List<PostModel<int, string, string, int?>>();

                if (request.Sort == "flat")
                {
                    posts = PostCrud.ReadAll(request.Forum, request.Thread, request.Since,
                        request.Order, request.Limit, true);
                }
                else if (request.Sort == "tree")
                {
                    var ids = PostCrud.ReadParents(request.Order, request.Limit, request.Since, request.Thread);

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
                    var ids = PostCrud.ReadParents(request.Order, request.Limit, request.Since, request.Thread);

                    foreach (var id in ids)
                    {
                        var post = PostCrud.Read(id);
                        posts.Add(post);

                        var childs = PostCrud.ReadChilds(post.Id, null, request.Since,
                            request.Thread);

                        foreach (var child in childs)
                        {
                            posts.Add(PostCrud.Read(child));
                        }
                    }
                }

                return new BaseResponse<List<PostModel<int, string, string, int?>>>
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

        public object Post(OpenThread request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Thread set IsClosed=false where Id=@Id", new { Id = request.Thread });

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Thread };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(RemoveThread request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Thread set IsDeleted=true where Id=@Id", new { Id = request.Thread });

                ConnectionProvider.DbConnection.Execute(
                    @"update Post set IsDeleted=true where Thread=@Id", new { Id = request.Thread });

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Thread };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(RestoreThread request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Thread set IsDeleted=false where Id=@Id", new { Id = request.Thread });

                ConnectionProvider.DbConnection.Execute(
                    @"update Post set IsDeleted=false where Thread=@Id", new { Id = request.Thread });

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Thread };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(Subscribe request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"insert into Subscribe (User, Thread) values (@User, @Thread)",
                    new { User = request.User, Thread = request.Thread });

                return new BaseResponse<BaseSubscribe>
                {
                    Code = StatusCode.Ok,
                    Response = request,
                };
            }
            catch (MySqlException e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(Unsubscribe request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"delete from Subscribe where User=@User and Thread=@Thread",
                    new { User = request.User, Thread = request.Thread });

                return new BaseResponse<BaseSubscribe>
                {
                    Code = StatusCode.Ok,
                    Response = request,
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(UpdateThread request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Thread set Message=@Message, Slug=@Slug where Id=@Id",
                    new { Message = request.Message, Slug = request.Slug, Id = request.Thread });

                return new BaseResponse<ThreadModel<string, string>>
                {
                    Code = StatusCode.Ok,
                    Response = ThreadCrud.Read(request.Thread),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(VoteThread request)
        {
            try
            {
                if (request.Value == 1)
                {
                    ConnectionProvider.DbConnection.Execute(
                        @"update Thread set Likes=Likes+1 where Id=@Id", new { Id = request.Thread });
                }
                else if (request.Value == -1)
                {
                    ConnectionProvider.DbConnection.Execute(
                        @"update Thread set Dislikes=Dislikes+1 where Id=@Id", new { Id = request.Thread });
                }

                return new BaseResponse<ThreadModel<string, string>>
                {
                    Code = StatusCode.Ok,
                    Response = ThreadCrud.Read(request.Thread),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
