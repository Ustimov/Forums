﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Post
{
    [Route("/db/api/post/create/")]
    [DataContract]
    public class CreatePost : PostModel<int, string, string, int?>
    {
        // Required date, thread, message, user, forum

        // Optional parent, isApproved, isHighlighted, isEdited, isSpam, isDeleted
    }

    [DataContract]
    public class CreatePostResponse : BaseResponse<PostModel<int, string, string, int?>>
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
    public class PostDetailsResponse : BaseResponse<PostModel<object, object, object, object>>
    {

    }

    [Route("/db/api/post/list/")]
    [DataContract]
    public class PostListPosts : BaseList
    {
        // Thread or Forum is required
        [DataMember(Name = "thread")]
        public int? Thread { get; set; }
    }

    [DataContract]
    public class ListPostsResponse : BaseResponse<List<PostModel<int, string, string, int?>>>
    {

    }

    [Route("/db/api/post/remove/")]
    [DataContract]
    public class RemovePost : BasePost
    {

    }

    [DataContract]
    public class RemovePostResponse : BaseResponse<int>
    {
        
    }

    [Route("/db/api/post/restore/")]
    [DataContract]
    public class RestorePost : BasePost
    {

    }

    [DataContract]
    public class RestorePostResponse : BaseResponse<int>
    {

    }

    [Route("/db/api/post/update/")]
    [DataContract]
    public class UpdatePost : BasePost
    {
        // Required
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    [DataContract]
    public class UpdatePostResponse : BaseResponse<PostModel<object, object, object, object>>
    {

    }

    [Route("/db/api/post/vote/")]
    [DataContract]
    public class VotePost : BasePost
    {
        // Possible values: [1, -1]
        [DataMember(Name = "vote")]
        public int Value { get; set; }
    }

    [DataContract]
    public class VoteThreadResponse : BaseResponse<PostModel<object, object, object, object>>
    {

    }

    [DataContract]
    public class PostListPostsResponse : BaseResponse<List<PostModel<object, object, object, object>>>
    {

    }
}
