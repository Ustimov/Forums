using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class ForumModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "short_name")]
        public string ShortName { get; set; }

        [DataMember(Name = "user")]
        public string User { get; set; }
    }
}
