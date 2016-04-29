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
    public class CloseThread : BaseThread
    {

    }

    [DataContract]
    public class CloseThreadResponse : BaseResponse<int>
    {

    }

    [Route("/db/api/thread/create/")]
    [DataContract]
    public class CreateThread : ThreadModel<string, string>
    {
        // Optional: isDeleted
        // Required: forum, title, isClosed, user, date, message, slug
    }

    [DataContract]
    public class CreateThreadResponse : BaseResponse<ThreadModel<string, string>>
    {

    }

    [Route("/db/api/thread/details/")]
    [DataContract]
    public class ThreadDetails : BaseThread
    {
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }
    }

    [Route("/db/api/thread/list/")]
    [DataContract]
    public class ThreadListThreads : BaseList
    {
        // Required user or forum
        [DataMember(Name = "user")]
        public string User { get; set; }
    }

    [Route("/db/api/thread/listPosts/")]
    [DataContract]
    public class ThreadListPosts : BaseList
    {
        // Required
        [DataMember(Name = "thread")]
        public int Thread { get; set; }

        [DataMember(Name = "sort")]
        public string Sort { get; set; } = "flat";
    }

    [Route("/db/api/thread/open/")]
    [DataContract]
    public class OpenThread : BaseThread
    {

    }

    [DataContract]
    public class OpenThreadResponse : BaseResponse<int>
    {

    }

    [Route("/db/api/thread/remove/")]
    [DataContract]
    public class RemoveThread : BaseThread
    {

    }

    [DataContract]
    public class RemoveThreadResponse : BaseResponse<int>
    {

    }

    [Route("/db/api/thread/restore/")]
    [DataContract]
    public class RestoreThread : BaseThread
    {

    }

    [DataContract]
    public class RestoreThreadResponse : BaseResponse<int>
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
    public class UpdateThread : BaseThread
    {
        // Required
        [DataMember(Name = "message")]
        public string Message { get; set; }

        // Required
        [DataMember(Name = "slug")]
        public string Slug { get; set; }
    }

    [DataContract]
    public class UpdateThreadResponse : BaseResponse<ThreadModel<object, object>>
    {

    }

    [Route("/db/api/thread/vote/")]
    [DataContract]
    public class VoteThread : BaseThread
    {
        // [-1, 1]
        [DataMember(Name = "vote")]
        public int Value { get; set; }
    }

    [DataContract]
    public class VoteThreadResponse : BaseResponse<ThreadModel<object, object>>
    {

    }

    [DataContract]
    public class ThreadListThreadsResponse : BaseResponse<List<ThreadModel<object, object>>>
    {

    }

    [DataContract]
    public class ThreadDetailsResponse : BaseResponse<ThreadModel<object, object>>
    {

    }
}
