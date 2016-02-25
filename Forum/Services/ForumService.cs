using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Models;
using Forum.Dtos.Forum;

namespace Forum.Services
{
    // Create new forum
    // {"name": "Forum With Sufficiently Large Name", "short_name": "forumwithsufficientlylargename", "user": "richard.nixon@example.com"}:
    public class ForumService : Service
    {
        public object Post(Create request)
        {
            return new CreateResponse
            {
                Code = 0,
                Response = new CreateForumResponseModel
                {
                    Id = 1,
                    User = request.User,
                    Name = request.Name,
                    ShortName = request.ShortName,
                }
            };
        }

        // Get forum details
        // http://some.host.ru/db/api/forum/details/?related=user&forum=forum3:
        public object Get(Details request)
        {
            return new DetailsResponse
            {
                Code = 0,
                Response = new DetailsForumResponseModel
                {
                    Id = 4,
                    Name = "\u0424\u043e\u0440\u0443\u043c \u0422\u0440\u0438",
                    ShortName = "forum3",
                    User = new UserModel
                    {
                        About = "hello im user2",
                        Email = "example2@mail.ru",
                        Followers = new List<string>(),
                        Following = new List<string>(),
                        Subscriptions = new List<int>(),
                        Id = 3,
                        IsAnonymous = false,
                        Name = "Jey",
                        Username = "user2",
                    }
                }
            };
        }

        // Get posts from this forum
        // http://some.host.ru/db/api/forum/listPosts/?related=thread&related=forum&since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListPosts request)
        {
            return new ListPostsResponse
            {
                Code = 0,
                Response = new List<PostModel<ThreadModel<string>, ForumModel>>
                {
                    new PostModel<ThreadModel<string>, ForumModel>
                    {
                        Date = new DateTime(),
                        Dislikes = 0,
                        Forum = new ForumModel
                        {
                            Id = 2,
                            Name = "Forum I",
                            ShortName = "forum1",
                            User = "example3@mail.ru",
                        },
                        Id = 5,
                        IsApproved = false,
                        IsDeleted = true,
                        IsEdited = false,
                        IsHighlighted = false,
                        IsSpam = false,
                        Likes = 0,
                        Message = "my message 1",
                        Points = 0,
                        Thread = new ThreadModel<string>
                        {
                            Date = new DateTime(),
                            Dislikes = 0,
                            Forum = "forum1",
                            Id = 3,
                            IsClosed = false,
                            IsDeleted = false,
                            Likes = 0,
                            Message = "hey hey!",
                            Points = 0,
                            Posts = 2,
                            Slug = "thread2",
                            Title = "Thread II",
                            User = "example3@mail.ru",
                        },
                        User = "richard.nixon@example.com",
                    }
                }
            };
        }

        // Get posts from this forum
        // http://some.host.ru/db/api/forum/listPosts/?related=thread&related=forum&since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListThreads request)
        {
            return new ListThreadsResponse
            {
                Code = 0,
                Response = new List<ThreadModel<ForumModel>>
                {
                    new ThreadModel<ForumModel>
                    {
                        Date = DateTime.Parse("2014-01-01 00:00:01"),
                        Dislikes = 0,
                        Forum = new ForumModel
                        {
                            Id = 2,
                            Name = "Forum I",
                            ShortName = "forum1",
                            User = "example3@mail.ru",
                        },
                        Id = 1,
                        IsClosed = true,
                        IsDeleted = true,
                        Likes = 0,
                        Message = "hey hey hey hey!",
                        Points = 0,
                        Posts = 0,
                        Slug = "Threadwithsufficientlylargetitle",
                        Title = "Thread With Sufficiently Large Title",
                        User = "example3@mail.ru",
                    }
                }
            };
        }

        public object Get(ListUsers request)
        {
            return new ListUsersResponse
            {
                Code = 0,
                Response = new List<UserModel>
                {
                    new UserModel
                    {
                        About = null,
                        Email = "richard.nixon@example.com",
                        Followers = new List<string>(),
                        Following = new List<string>(),
                        Id = 2,
                        IsAnonymous = true,
                        Name = null,
                        Subscriptions = new List<int>(),
                        Username = null,
                    }
                }
            };
        }
    }
}
