using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class ThreadModel<TForum, TUser> : BaseThreadModel
    {
        [DataMember(Name = "forum")]
        public TForum Forum { get; set; }

        [DataMember(Name = "user")]
        public TUser User { get; set; }

        public ThreadModel()
        {

        }

        public ThreadModel(BaseThreadModel baseThreadModel) : base(baseThreadModel)
        {

        }
    }
}
