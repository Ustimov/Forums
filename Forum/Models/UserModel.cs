using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember(Name = "about")]
        public string About { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
        
        [DataMember(Name = "followers")]
        // TODO: Correct model?
        public List<UserModel> Followers { get; set; }

        [DataMember(Name = "following")]
        // TODO: Correct model?
        public List<UserModel> Following { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "isAnonymous")]
        public bool IsAnonymous { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "subscriptions")]
        // TODO: Correct model?
        public List<UserModel> Subscriptions { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }
    }
}
