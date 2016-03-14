using System.Runtime.Serialization;

namespace Forum.Models
{
    public class BaseForumModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "short_name")]
        public string ShortName { get; set; }

        public BaseForumModel()
        {

        }

        public BaseForumModel(BaseForumModel baseForumModel)
        {
            Id = baseForumModel.Id;
            Name = baseForumModel.Name;
            ShortName = baseForumModel.ShortName;
        }
    }
}