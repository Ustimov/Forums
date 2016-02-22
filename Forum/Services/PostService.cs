using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Post;
using Forum.Dtos.Base;
using Forum.Models;

namespace Forum.Services
{
    public class PostService : Service
    {
        // Create new post
        // {"isApproved": true, "user": "example@mail.ru", "date": "2014-01-01 00:00:01", "message": "my message 1", "isSpam": false, "isHighlighted": true, "thread": 4, "forum": "forum2", "isDeleted": false, "isEdited": true}
        public object Post(Create request)
        {
            return new CreateResponse
            {
                Code = 0,
                Response = new PostModel<int, string>
                {
                    Date = request.Date,
                    Forum = request.Forum,
                    Id = request.Id,
                    IsApproved = request.IsApproved,
                    IsDeleted = request.IsDeleted,
                    IsEdited = request.IsEdited,
                    IsHighlighted = request.IsHighlighted,
                    IsSpam = request.IsSpam,
                    Message = request.Message,
                    Thread = request.Thread,
                    User = request.User,
                }
            };
        }

        // Get post details
        // http://some.host.ru/db/api/post/details/?post=3
        public object Get(Details request)
        {
            return new DetailsResponse
            {
                Code = 0,
                Response = new PostModel<int, string>
                {
                    Date = DateTime.Parse("2014-01-02 00:02:01"),
                    Dislikes = 0,
                    Forum = "forum2",
                    Id = 3,
                    IsApproved = false,
                    IsDeleted = false,
                    IsEdited = false,
                    IsHighlighted = false,
                    IsSpam = false,
                    Likes = 0,
                    Message = "my message 1",
                    // TODO
                    //Parent = 2,
                    Points = 0,
                    Thread = 4,
                    User = "example@mail.ru",
                }
            };
        }

        // List posts
        // http://some.host.ru/db/api/post/list/?since=2014-01-01+00%3A00%3A00&order=desc&forum=forum1
        public object Get(ListPosts request)
        {
            return new ListPostsResponse
            {
                Code = 0,
                Response = new List<PostModel<int, string>>
                {
                    new PostModel<int, string>
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
                        Message = "my message 1",
                        Points = 0,
                        Thread = 3,
                        User = "richard.nixon@example.com",
                    }
                }
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
                Response = new PostModel<int, string>
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
                Response = new PostModel<int, string>
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
