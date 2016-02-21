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
                        Followers = new List<UserModel>(),
                        Following = new List<UserModel>(),
                        Subscriptions = new List<UserModel>(),
                        Id = 3,
                        IsAnonymous = false,
                        Name = "Jey",
                        Username = "user2",
                    }
                }
            };
        }
    }
}
