﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Post
{
    [Route("/post/create/")]
    [DataContract]
    public class Create : PostModel<int, string>
    {
        // Required date, thread, message, user, forum

        // Optional parent, isApproved, isHighlighted, isEdited, isSpam, isDeleted
    }

    [DataContract]
    public class CreateResponse : BaseResponse<PostModel<int, string>>
    {

    }

    [Route("/post/details/")]
    [DataContract]
    public class Details
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

    [Route("/post/list/")]
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

    [Route("/post/remove/")]
    [DataContract]
    public class Remove : BasePost
    {

    }

    [DataContract]
    public class RemoveResponse : BaseResponse<BasePost>
    {
        
    }

    [Route("/post/restore/")]
    [DataContract]
    public class Restore : BasePost
    {

    }

    [DataContract]
    public class RestoreResponse : BaseResponse<BasePost>
    {

    }

    [Route("/post/update/")]
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

    [Route("/post/vote/")]
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
