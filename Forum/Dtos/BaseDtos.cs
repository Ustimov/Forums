using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Forum.Dtos.Base
{
    [DataContract]
    public class BaseResponse<T>
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "response")]
        public T Response { get; set; }
    }

    public class BaseList
    {
        // Optional
        [DataMember(Name = "since")]
        public DateTime Since { get; set; }

        // Optional
        [DataMember(Name = "limit")]
        public int Limit { get; set; }

        // Optional
        // Possible values: ['desc', 'asc']. Default: 'desc'
        [DataMember(Name = "order")]
        public string Order { get; set; } = "desc";

        // Optional
        [DataMember(Name = "related")]
        public List<string> Related { get; set; }

        // Required
        [DataMember(Name = "forum")]
        public string Forum { get; set; }
    }

    [DataContract]
    public class BasePost
    {
        // Required
        [DataMember(Name = "post")]
        public int Post { get; set; }
    }
}
