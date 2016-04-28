using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class UserModel : BaseUserModel
    {
        [DataMember(Name = "email")]
        public override string Email { get; set; }
    }
}
