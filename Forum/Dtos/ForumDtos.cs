using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Forum
{
    // All params are required
    [Route("/db/api/forum/create/")]
    [DataContract]
    public class CreateForum
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "short_name")]
        public string ShortName { get; set; }

        [DataMember(Name = "user")]
        public string User { get; set; }
    }

    [DataContract]
    public class CreateResponse : BaseResponse<CreateForumResponseModel>
    {

    }
    
    [Route("/db/api/forum/details/")]
    [DataContract]
    public class ForumDetails
    {
        // Optional
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }

        // Required
        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [DataContract]
    public class DetailsResponse : BaseResponse<DetailsForumResponseModel>
    {

    }

    [Route("/db/api/forum/listPosts/")]
    [DataContract]
    public class ListPosts : BaseList
    {
        // Possible Related values: ['thread', 'forum', 'user']
    }

    [DataContract]
    public class ListPostsResponse : BaseResponse<List<PostModel<ThreadModel<string, string>, ForumModel<string>,
        string, int>>>
    {

    }

    [Route("/db/api/forum/listThreads/")]
    [DataContract]
    public class ListThreads : BaseList
    {
        // Possible Related values: ['forum', 'user']
    }

    [DataContract]
    public class ListThreadsResponse : BaseResponse<List<ThreadModel<ForumModel<string>, string>>>
    {

    }

    [Route("/db/api/forum/listUsers/")]
    [DataContract]
    public class ListUsers
    {
        // Optional
        [DataMember(Name = "limit")]
        public int? Limit { get; set; }

        // Optional
        // Possible values: ['desc', 'asc']. Default: 'desc'
        [DataMember(Name = "order")]
        public string Order { get; set; } = "desc";

        // Optional
        // return entities in interval [since_id, max_id]
        [DataMember(Name = "since_id")]
        public int? SinceId { get; set; }

        // Required
        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [DataContract]
    public class ListUsersResponse : BaseResponse<List<UserModel>>
    {

    }
}
