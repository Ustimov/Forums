using System.Runtime.Serialization;

namespace Forum.Dtos
{
    [DataContract]
    public class BaseResponse<T>
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "response")]
        public T Response { get; set; }
    }
}
