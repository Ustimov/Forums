using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Post;
using Forum.Dtos.Base;
using Forum.Models;
using Forum.Helpers;
using Dapper;

namespace Forum.Services
{
    public class PostService : Service
    {
        public object Post(CreatePost request)
        {
            try
            {
                PostCrud.Create(request);

                var post = PostCrud.Read(request);

                var parentPath = String.Empty;

                if (request.Parent != null)
                {
                    parentPath = ConnectionProvider.DbConnection.ExecuteScalar<string>(
                        @"select Path from Post where Id=@Post", new { Post = request.Parent });
                }

                ConnectionProvider.DbConnection.Execute(
                    @"update Post set Path=@Path where Id=@Post",
                    new
                    {
                        Post = post.Id,
                        Path = (parentPath == String.Empty ? String.Empty : parentPath + ".") + post.Id.ToString("D10"),
                    });

                return new BaseResponse<PostModel<int, string, string, int?>>
                {
                    Code = StatusCode.Ok,
                    Response = post,
                };
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public object Get(PostDetails request)
        {
            try
            {
                var post = PostCrud.Read(request.Post);

                if (post == null)
                {
                    return new BaseResponse<string> { Code = StatusCode.ObjectNotFound, Response = "Post not found" };
                }

                if (request.Related == null)
                {
                    return new BaseResponse<PostModel<int, string, string, int?>> { Code = StatusCode.Ok, Response = post };
                }
                else if (request.Related.Count == 3 && request.Related.Contains("user") &&
                    request.Related.Contains("thread") && request.Related.Contains("forum"))
                {
                    return new BaseResponse<PostModel<ThreadModel<string, string>, ForumModel<string>, UserModel, int?>>
                    {
                        Code = StatusCode.Ok,
                        Response = new PostModel<ThreadModel<string, string>, ForumModel<string>, UserModel, int?>(post)
                        {
                            Thread = ThreadCrud.Read(post.Thread),
                            Forum = ForumCrud.Read(post.Forum),
                            User = UserCrud.Read(post.User),
                            Parent = (post.Parent == 0 ? null : post.Parent),
                        },
                    };
                }
                else if (request.Related.Count == 2)
                {
                    if (request.Related.Contains("user") && request.Related.Contains("thread"))
                    {
                        return new BaseResponse<PostModel<ThreadModel<string, string>, string, UserModel, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, string, UserModel, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = post.Forum,
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            },
                        };
                    }
                    else if (request.Related.Contains("user") && request.Related.Contains("forum"))
                    {
                        return new BaseResponse<PostModel<int, ForumModel<string>, UserModel, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, ForumModel<string>, UserModel, int?>(post)
                            {
                                Thread = post.Id,
                                Forum = ForumCrud.Read(post.Forum),
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            },
                        };
                    }
                    else if (request.Related.Contains("thread") && request.Related.Contains("forum"))
                    {
                        return new BaseResponse<PostModel<ThreadModel<string, string>, ForumModel<string>, string, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, ForumModel<string>, string, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = ForumCrud.Read(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            },
                        };
                    }
                }
                else if (request.Related.Count == 1)
                {
                    if (request.Related.Contains("user"))
                    {
                        return new BaseResponse<PostModel<int, string, UserModel, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, string, UserModel, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = post.Forum,
                                User = UserCrud.Read(post.User),
                                Parent = post.Parent,
                            },
                        };
                    }
                    else if (request.Related.Contains("thread"))
                    {
                        return new BaseResponse<PostModel<ThreadModel<string, string>, string, string, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, string, string, int?>(post)
                            {
                                Thread = ThreadCrud.Read(post.Thread),
                                Forum = post.Forum,
                                User = post.User,
                                Parent = post.Parent,
                            },
                        };
                    }
                    else if (request.Related.Contains("forum"))
                    {
                        return new BaseResponse<PostModel<int, ForumModel<string>, string, int?>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, ForumModel<string>, string, int?>(post)
                            {
                                Thread = post.Thread,
                                Forum = ForumCrud.Read(post.Forum),
                                User = post.User,
                                Parent = post.Parent,
                            },
                        };
                    }
                }

                return new BaseResponse<string> { Code = StatusCode.IncorrectRequest, Response = "Incorrect request" };
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public object Get(PostListPosts request)
        {
            try
            {
                var posts = PostCrud.ReadAll(request.Forum, request.Thread, request.Since, request.Order, request.Limit);

                return new BaseResponse<List<PostModel<int, string, string, int?>>>
                {
                    Code = StatusCode.Ok,
                    Response = posts
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(RemovePost request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set IsDeleted=true where Id=@Id", new { Id = request.Post });

                /*
                ConnectionProvider.DbConnection.Execute(
                    @"update Path set IsDeleted=true where Post=@Id", new { Id = request.Post });
                */

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Post };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(RestorePost request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set IsDeleted=false where Id=@Id", new { Id = request.Post });

                /*
                ConnectionProvider.DbConnection.Execute(
                    @"update Path set IsDeleted=false where Post=@Id", new { Id = request.Post });
                */

                return new BaseResponse<int> { Code = StatusCode.Ok, Response = request.Post };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(UpdatePost request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set Message=@Message where Id=@Id",
                    new { Message = request.Message, Id = request.Post });

                return new BaseResponse<PostModel<int, string, string, int?>>
                {
                    Code = StatusCode.Ok,
                    Response = PostCrud.Read(request.Post),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public object Post(VotePost request)
        {
            System.Diagnostics.Debug.WriteLine($"Thread: { request.Post } | Value {request.Value}");
            if (request.Value == 1)
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set Likes=Likes+1 where Id=@Id", new { Id = request.Post });
            }
            else if (request.Value == -1)
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set Dislikes=Dislikes+1 where Id=@Id", new { Id = request.Post });
            }

            return new BaseResponse<PostModel<int, string, string, int?>>
            {
                Code = StatusCode.Ok,
                Response = PostCrud.Read(request.Post),
            };
        }

    }
}
