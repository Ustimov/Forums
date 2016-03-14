using System;
using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class PostModel<TThread, TForum, TUser, TParent> : BasePostModel
    {
        [DataMember(Name = "forum")]
        public TForum Forum { get; set; }

        [DataMember(Name = "parent")]
        public TParent Parent { get; set; }

        [DataMember(Name = "thread")]
        public TThread Thread { get; set; }

        [DataMember(Name = "user")]
        public TUser User { get; set; }

        public PostModel()
        {

        }

        public PostModel(BasePostModel basePostModel) : base(basePostModel)
        {

        }
    }
}
 