using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class StatusResponseModel
    {
        [DataMember(Name = "user")]
        public int User { get; set; }

        [DataMember(Name = "thread")]
        public int Thread { get; set; }

        [DataMember(Name = "forum")]
        public int Forum { get; set; }

        [DataMember(Name = "post")]
        public int Post { get; set; }
    }
}