using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Models;
using Forum.Dtos.Base;

namespace Forum.Dtos.Common
{
    [Route("/db/api/clear/")]
    public class Clear
    {

    }

    [DataContract]
    public class ClearResponse : BaseResponse<string>
    {

    }

    [Route("/db/api/status/")]
    public class Status
    {

    }

    [DataContract]
    public class StatusResponse : BaseResponse<StatusResponseModel>
    {

    }
}
