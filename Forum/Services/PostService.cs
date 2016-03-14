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
            return new ListPostsResponse
            {
                Code = 0,
                
            };
        }

        // Mark post as removed
        // {"post": 3}
        public object Post(Remove request)
        {
            return new RemoveResponse
            {
                Code = 0,
                Response = new BasePost
                {
                    Post = request.Post,
                }
            };
        }

        // Cancel removal
        // {"post": 3}
        public object Post(Restore request)
        {
            return new RestoreResponse
            {
                Code = 0,
                Response = new BasePost
                {
                    Post = request.Post,
                }
            };
        }

        // Edit post
        // {"post": 3, "message": "my message 1"}
        public object Post(Update request)
        {
            return new UpdateResponse
            {
                Code = 0,
                Response = new PostModel<int, string, string, int>
                {
                    Date = DateTime.Parse("2014-01-03 00:08:01"),
                    Dislikes = 0,
                    Forum = "forum1",
                    Id = 5,
                    IsApproved = false,
                    IsDeleted = true,
                    IsEdited = false,
                    IsHighlighted = false,
                    IsSpam = false,
                    Likes = 0,
                    Message = request.Message,
                    Points = 0,
                    Thread = 3,
                    User = "richard.nixon@example.com",
                }
            };
        }

        // like/dislike post
        // {"vote": -1, "post": 5}
        public object Post(Vote request)
        {
            return new VoteResponse
            {
                Code = 0,
                Response = new PostModel<int, string, string, int>
                {
                    Date = DateTime.Parse("2014-01-03 00:08:01"),
                    Dislikes = 0,
                    Forum = "forum1",
                    Id = 5,
                    IsApproved = false,
                    IsDeleted = true,
                    IsEdited = false,
                    IsHighlighted = false,
                    IsSpam = false,
                    Likes = 0,
                    Message = "fdfsd",
                    Points = 0,
                    Thread = 3,
                    User = "richard.nixon@example.com",
                }
            };
        }

    }
}
