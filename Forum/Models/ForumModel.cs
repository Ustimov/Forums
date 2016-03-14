using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class ForumModel<TUser> : BaseForumModel
    {
        [DataMember(Name = "user")]
        public TUser User { get; set; }

        public ForumModel()
        {

        }

        public ForumModel(BaseForumModel baseForumModel) : base(baseForumModel)
        {

        }
    }
}
