using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Post
{
    [Route("/db/api/post/create/")]
    [DataContract]
    public class CreatePost : PostModel<int, string>
    {
        // Required date, thread, message, user, forum

        // Optional parent, isApproved, isHighlighted, isEdited, isSpam, isDeleted
    }

    [DataContract]
    public class CreateResponse : BaseResponse<PostModel<int, string>>
    {

    }

    [Route("/db/api/post/details/")]
    [DataContract]
    public class PostDetails
    {
        // Optional
        // Possible values: ['user', 'thread', 'forum']
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }

        // Required
        [DataMember(Name = "post")]
        public int Post { get; set; }
    }

    [DataContract]
    public class DetailsResponse : BaseResponse<PostModel<int, string>>
    {

    }

    [Route("/db/api/post/list/")]
    [DataContract]
    public class ListPosts : BaseList
    {
        // Thread or Forum is required
        [DataMember(Name = "thread")]
        public int Thread { get; set; }
    }

    [DataContract]
    public class ListPostsResponse : BaseResponse<List<PostModel<int, string>>>
    {

    }

    [Route("/db/api/post/remove/")]
    [DataContract]
    public class Remove : BasePost
    {

    }

    [DataContract]
    public class RemoveResponse : BaseResponse<BasePost>
    {
        
    }

    [Route("/db/api/post/restore/")]
    [DataContract]
    public class Restore : BasePost
    {

    }

    [DataContract]
    public class RestoreResponse : BaseResponse<BasePost>
    {

    }

    [Route("/db/api/post/update/")]
    [DataContract]
    public class Update : BasePost
    {
        // Required
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    [DataContract]
    public class UpdateResponse : BaseResponse<PostModel<int, string>>
    {

    }

    [Route("/db/api/post/vote/")]
    [DataContract]
    public class Vote : BasePost
    {
        // Possible values: [1, -1]
        [DataMember(Name = "vote")]
        public int Value { get; set; }
    }

    [DataContract]
    public class VoteResponse : BaseResponse<PostModel<int, string>>
    {

    }
}
