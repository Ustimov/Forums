using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Forum.Dtos.Base
{
    [DataContract]
    public class BaseResponse<T>
    {
        [DataMember(Name = "code")]
        public StatusCode Code { get; set; }

        [DataMember(Name = "response")]
        public T Response { get; set; }
    }

    public class BaseList
    {
        [DataMember(Name = "since")]
        public DateTime? Since { get; set; }

        [DataMember(Name = "since_id")]
        public int? SinceId { get; set; }

        [DataMember(Name = "limit")]
        public int? Limit { get; set; }

        [DataMember(Name = "order")]
        public string Order { get; set; } = "desc";

        [DataMember(Name = "related")]
        public List<string> Related { get; set; }

        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [DataContract]
    public class BasePost
    {
        [DataMember(Name = "post")]
        public int Post { get; set; }
    }

    [DataContract]
    public class BaseUser
    {
        [DataMember(Name = "user")]
        public string Email { get; set; }
    }

    [DataContract]
    public class BaseFollow
    {
        [DataMember(Name = "follower")]
        public string Follower { get; set; }

        [DataMember(Name = "followee")]
        public string Followee { get; set; }
    }

    [DataContract]
    public class BaseThread
    {
        [DataMember(Name = "thread")]
        public int Thread { get; set; }
    }

    [DataContract]
    public class BaseSubscribe : BaseThread
    {
        [DataMember(Name = "user")]
        public string User { get; set; }
    }

    [DataContract]
    public class ErrorResponse : BaseResponse<string>
    {

    }
}
