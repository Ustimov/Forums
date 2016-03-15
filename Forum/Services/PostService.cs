﻿using System;
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

                return new BaseResponse<PostModel<int, string, string, int>>
                {
                    Code = StatusCode.Ok,
                    Response = PostCrud.Read(request)
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
                    return new BaseResponse<PostModel<int, string, string, int>> { Code = StatusCode.Ok, Response = post };
                }
                else if (request.Related.Count == 3 && request.Related.Contains("user") &&
                    request.Related.Contains("thread") && request.Related.Contains("forum"))
                {
                    return new BaseResponse<PostModel<ThreadModel<string, string>, ForumModel<string>, UserModel, int>>
                    {
                        Code = StatusCode.Ok,
                        Response = new PostModel<ThreadModel<string, string>, ForumModel<string>, UserModel, int>
                        {
                            Thread = ThreadCrud.Read(post.Thread),
                            Forum = ForumCrud.Read(post.Forum),
                            User = UserCrud.Read(post.User),
                            Parent = post.Parent,
                        },
                    };
                }
                else if (request.Related.Count == 2)
                {
                    if (request.Related.Contains("user") && request.Related.Contains("thread"))
                    {
                        return new BaseResponse<PostModel<ThreadModel<string, string>, string, UserModel, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, string, UserModel, int>
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
                        return new BaseResponse<PostModel<int, ForumModel<string>, UserModel, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, ForumModel<string>, UserModel, int>
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
                        return new BaseResponse<PostModel<ThreadModel<string, string>, ForumModel<string>, string, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, ForumModel<string>, string, int>
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
                        return new BaseResponse<PostModel<int, string, UserModel, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, string, UserModel, int>
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
                        return new BaseResponse<PostModel<ThreadModel<string, string>, string, string, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<ThreadModel<string, string>, string, string, int>
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
                        return new BaseResponse<PostModel<int, ForumModel<string>, string, int>>
                        {
                            Code = StatusCode.Ok,
                            Response = new PostModel<int, ForumModel<string>, string, int>
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

        // List posts
        // http://some.host.ru/db/api/post/list/?since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListPosts request)
        {
            throw new NotImplementedException();

            return new ListPostsResponse
            {
                Code = 0,
                
            };
        }

        public object Post(RemovePost request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"update Post set IsDeleted=true where Id=@Id", new { Id = request.Post });

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

                return new BaseResponse<PostModel<int, string, string, int>>
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

            return new BaseResponse<PostModel<int, string, string, int>>
            {
                Code = StatusCode.Ok,
                Response = PostCrud.Read(request.Post),
            };
        }

    }
}
