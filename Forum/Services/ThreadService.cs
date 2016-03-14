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

namespace Forum.Services
{
    public class ThreadService : Service
    {
        // Mark thread as closed
        // {"thread": 1}
        public object Post(Close request)
        {
            return new CloseResponse
            {
                Code = 0,
                Response = new BaseThread
                {
                    Thread = request.Thread,
                },
            };
        }

        public object Post(CreateThread request)
        {
            try
            {
                ThreadCrud.Create(request);
                return new BaseResponse<ThreadModel<string, string>> { Code = StatusCode.Ok, Response = request };
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

                if (request.Related.Count == 0)
                {
                    return new BaseResponse<ThreadModel<string, string>>
                    {
                        Code = StatusCode.Ok,
                        Response = thread,
                    };
                }
                else if (request.Related.Count == 2 && request.Related.Contains("user") && request.Related.Contains("forum"))
                {
                    return new BaseResponse<ThreadModel<ForumModel, UserModel>>
                    {
                        Code = StatusCode.Ok,
                        Response = new ThreadModel<ForumModel, UserModel>(thread)
                        {
                            Forum = ForumCrud.Read(thread.Forum),
                            User = UserCrud.Read(thread.User),
                        },
                    };
                }
                else if (request.Related.Count == 1)
                {
                    if (request.Related.Contains("forum"))
                    {
                        return new BaseResponse<ThreadModel<ForumModel, string>>
                        {
                            Code = StatusCode.Ok,
                            Response = new ThreadModel<ForumModel, string>(thread)
                            {
                                Forum = ForumCrud.Read(thread.Forum),
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
                                User = UserCrud.Read(thread.User),
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

        // List threads
        // thread/list/?since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListThreads request)
        {
            return new ListThreadsResponse
            {
                Code = 0,
                Response = new List<ThreadModel<string, string>>
                {
                    new ThreadModel<string, string>
                    {
                        DateString = "2014-01-01 00:00:01",
                        Forum = "forum1",
                        Id = 1,
                        IsClosed = true,
                        IsDeleted = true,
                        Message = "hey hey hey hey!",
                        Slug = "Threadwithsufficientlylargetitle",
                        Title = "Thread With Sufficiently Large Title",
                        User = "example3@mail.ru",
                    },
                },
            };
        }

        // Get posts from this thread
        // thread/listPosts/?since=2014-01-02+00%3A00%3A00&limit=2&order=asc&thread=3
        public object Get(ListPosts request)
        {
            return new ListsPostsResponse
            {
                Code = 0,
                Response = new List<PostModel<int, string>>
                {
                    new PostModel<int, string>
                    {
                        DateString = "2014-01-03 00:01:01",
                        Dislikes = 0,
                        Forum = "forum1",
                        Id = 4,
                        IsApproved = true,
                        IsDeleted = false,
                        IsEdited = false,
                        IsHighlighted = false,
                        IsSpam = false,
                        Likes = 0,
                        Message = "my message 1",
                        Thread = 3,
                        User = "example@mail.ru",
                    }
                },
            };
        }

        // Mark thread as opened
        // {"thread": 1}
        public object Post(Open request)
        {
            return new OpenResponse
            {
                Code = 0,
                Response = new BaseThread
                {
                    Thread = request.Thread,
                }
            };
        }

        // Mark thread as removed
        // {"thread": 1}
        public object Post(Remove request)
        {
            return new RemoveResponse
            {
                Code = 0,
                Response = new BaseThread
                {
                    Thread = request.Thread,
                }
            };
        }

        // Cancel removal
        // {"thread": 1}
        public object Post(Restore request)
        {
            return new RestoreResponse
            {
                Code = 0,
                Response = new BaseThread
                {
                    Thread = request.Thread,
                }
            };
        }

        public object Post(Subscribe request)
        {
            try
            {
                ConnectionProvider.DbConnection.Execute(
                    @"insert into Subscribe values (User=@User, Thread=@Thread)",
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

        // Edit thread
        // {"message": "hey hey hey hey!", "slug": "newslug", "thread": 1}
        public object Post(Update request)
        {
            return new UpdateResponse
            {
                Code = 0,
                Response = new ThreadModel<string, string>
                {
                    DateString = "2014-01-01 00:00:01",
                    Forum = "forum1",
                    Id = 1,
                    IsClosed = true,
                    IsDeleted = true,
                    Message = "hey hey hey hey!",
                    Slug = "Threadwithsufficientlylargetitle",
                    Title = "Thread With Sufficiently Large Title",
                    User = "example3@mail.ru",
                },
            };
        }

        // like/dislike thread
        // {"vote": 1, "thread": 1}
        public object Post(Vote request)
        {
            return new VoteResponse
            {
                Code = 0,
                Response = new ThreadModel<string, string>
                {
                    DateString = "2014-01-01 00:00:01",
                    Forum = "forum1",
                    Id = 1,
                    IsClosed = true,
                    IsDeleted = true,
                    Message = "hey hey hey hey!",
                    Slug = "Threadwithsufficientlylargetitle",
                    Title = "Thread With Sufficiently Large Title",
                    User = "example3@mail.ru",
                },
            };
        }
    }
}
