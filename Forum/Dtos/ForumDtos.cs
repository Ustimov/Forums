using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Forum
{
    [Route("/db/api/forum/create/")]
    [DataContract]
    public class CreateForum : ForumModel<string>
    {

    }
    
    [Route("/db/api/forum/details/")]
    [DataContract]
    public class ForumDetails
    {
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }

        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [Route("/db/api/forum/listPosts/")]
    [DataContract]
    public class ForumListPosts : BaseList
    {

    }

    [Route("/db/api/forum/listThreads/")]
    [DataContract]
    public class ForumListThreads : BaseList
    {

    }

    [Route("/db/api/forum/listUsers/")]
    [DataContract]
    public class ForumListUsers
    {
        [DataMember(Name = "limit")]
        public int? Limit { get; set; }

        [DataMember(Name = "order")]
        public string Order { get; set; } = "desc";

        [DataMember(Name = "since_id")]
        public int? SinceId { get; set; }

        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [DataContract]
    public class CreateForumResponse : BaseResponse<ForumModel<string>>
    {

    }

    [DataContract]
    public class ForumDetailsResponse : BaseResponse<ForumModel<object>>
    {

    }

    [DataContract]
    public class ForumListUsersResponse : BaseResponse<IEnumerable<UserModel>>
    {

    }

    [DataContract]
    public class ForumListThreadsResponse : BaseResponse<List<ThreadModel<object, object>>>
    {

    }

    [DataContract]
    public class ForumListPostsResponse : BaseResponse<List<PostModel<object, object, object, object>>>
    {

    }
}
