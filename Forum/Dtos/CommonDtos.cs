using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Common
{
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    [Route("/db/api/clear/")]
    public class Clear
    {

    }

    [DataContract]
    public class ClearResponse : BaseResponse<string>
    {

    }

    [Route("/status/")]
    public class Status
    {

    }

    [DataContract]
    public class StatusResponse : BaseResponse<StatusResponseModel>
    {

    }
}
