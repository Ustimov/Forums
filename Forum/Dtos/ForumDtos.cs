using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;

namespace Forum.Dtos.Forum
{
    // All params are required
    [Route("/forum/create/")]
    [DataContract]
    public class Create
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
    
    [Route("/forum/details/")]
    [DataContract]
    public class Details
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

}
