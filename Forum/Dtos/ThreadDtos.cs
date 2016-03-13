using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Dtos.Base;
using Forum.Models;

namespace Forum.Dtos.Thread
{
    [Route("/db/api/thread/close/")]
    [DataContract]
    public class Close : BaseThread
    {

    }

    [DataContract]
    public class CloseResponse : BaseResponse<BaseThread>
    {

    }

    [Route("/db/api/thread/create/")]
    [DataContract]
    public class CreateThread : ThreadModel<string>
    {
        // Optional: isDeleted
        // Required: forum, title, isClosed, user, date, message, slug
    }

    [DataContract]
    public class CreateResponse : BaseResponse<ThreadModel<string>>
    {

    }

    [Route("/db/api/thread/details/")]
    [DataContract]
    public class Details : BaseThread
    {
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }
    }

    [DataContract]
    public class DetailsResponse : BaseResponse<ThreadModel<string>>
    {

    }

    [Route("/db/api/thread/list/")]
    [DataContract]
    public class ListThreads : BaseList
    {
        // Required user or forum
        [DataMember(Name = "user")]
        public string User { get; set; }
    }

    [DataContract]
    public class ListThreadsResponse : BaseResponse<List<ThreadModel<string>>>
    {

    }

    [Route("/db/api/thread/listPosts/")]
    [DataContract]
    public class ListPosts : BaseList
    {
        // Required
        [DataMember(Name = "thread")]
        public int Thread { get; set; }
    }

    [DataContract]
    public class ListsPostsResponse : BaseResponse<List<PostModel<int, string>>>
    {

    }

    [Route("/db/api/thread/open/")]
    [DataContract]
    public class Open : BaseThread
    {

    }

    [DataContract]
    public class OpenResponse : BaseResponse<BaseThread>
    {

    }

    [Route("/db/api/thread/remove/")]
    [DataContract]
    public class Remove : BaseThread
    {

    }

    [DataContract]
    public class RemoveResponse : BaseResponse<BaseThread>
    {

    }

    [Route("/db/api/thread/restore/")]
    [DataContract]
    public class Restore : BaseThread
    {

    }

    [DataContract]
    public class RestoreResponse : BaseResponse<BaseThread>
    {

    }

    [Route("/db/api/thread/subscribe/")]
    [DataContract]
    public class Subscribe : BaseSubscribe
    {

    }

    [DataContract]
    public class SubscribeResponse : BaseResponse<BaseSubscribe>
    {

    }

    [Route("/db/api/thread/unsubscribe/")]
    [DataContract]
    public class Unsubscribe : BaseSubscribe
    {

    }

    [DataContract]
    public class UnsubscribeResponse : BaseResponse<BaseSubscribe>
    {

    }

    [Route("/db/api/thread/update/")]
    [DataContract]
    public class Update : BaseThread
    {
        // Required
        [DataMember(Name = "message")]
        public string Message { get; set; }

        // Required
        [DataMember(Name = "slug")]
        public string Slug { get; set; }
    }

    [DataContract]
    public class UpdateResponse : BaseResponse<ThreadModel<string>>
    {

    }

    [Route("/db/api/thread/vote/")]
    [DataContract]
    public class Vote : BaseThread
    {
        // [-1, 1]
        [DataMember(Name = "vote")]
        public int Value { get; set; }
    }

    [DataContract]
    public class VoteResponse : BaseResponse<ThreadModel<string>>
    {

    }
}
