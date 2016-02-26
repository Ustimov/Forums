using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Base;
using Forum.Dtos.Thread;
using Forum.Models;

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

        // Create new thread
        // {"forum": "forum1", "title": "Thread With Sufficiently Large Title", "isClosed": true, "user": "example3@mail.ru", "date": "2014-01-01 00:00:01", "message": "hey hey hey hey!", "slug": "Threadwithsufficientlylargetitle", "isDeleted": true}
        public object Post(Create request)
        {
            return new CreateResponse
            {
                Code = 0, 
                Response = new ThreadModel<string>
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

        // Get thread details
        // thread/details/?thread=1
        public object Get(Details request)
        {
            return new DetailsResponse
            {
                Code = 0,
                Response = new ThreadModel<string>
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

        // List threads
        // thread/list/?since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListThreads request)
        {
            return new ListThreadsResponse
            {
                Code = 0,
                Response = new List<ThreadModel<string>>
                {
                    new ThreadModel<string>
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

        // Subscribe user to this thread
        // {"user": "richard.nixon@example.com", "thread": 4}
        public object Post(Subscribe request)
        {
            return new SubscribeResponse
            {
                Code = 0,
                Response = new BaseSubscribe
                {
                    Thread = request.Thread,
                    User = request.User,
                }
            };
        }

        // Unsubscribe user from this thread
        // {"user": "richard.nixon@example.com", "thread": 4}
        public object Post(Unsubscribe request)
        {
            return new UnsubscribeResponse
            {
                Code = 0,
                Response = new BaseSubscribe
                {
                    Thread = request.Thread,
                    User = request.User,
                }
            };
        }

        // Edit thread
        // {"message": "hey hey hey hey!", "slug": "newslug", "thread": 1}
        public object Post(Update request)
        {
            return new UpdateResponse
            {
                Code = 0,
                Response = new ThreadModel<string>
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
                Response = new ThreadModel<string>
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
