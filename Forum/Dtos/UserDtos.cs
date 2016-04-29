using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Dtos.Base;
using Forum.Models;

namespace Forum.Dtos.User
{
    [Route("/db/api/user/create/")]
    [DataContract]
    public class CreateUser : UserModel
    {
        // isAnonymous - optional
        // Required: username, name, about, email
    }

    [DataContract]
    public class CreateUserResponse : BaseResponse<UserModel>
    {

    }

    [Route("/db/api/user/details/")]
    [DataContract]
    public class UserDetails : BaseUser
    {

    }

    [DataContract]
    public class UserDetailsResponse : BaseResponse<UserModel>
    {

    }

    [Route("/db/api/user/follow/")]
    [DataContract]
    public class Follow : BaseFollow
    {

    }

    [DataContract]
    public class FollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/db/api/user/listFollowers/")]
    [DataContract]
    public class ListFollowers : BaseList
    {
        [DataMember(Name = "user")]
        public string Email { get; set; }
    }

    [DataContract]
    public class ListFollowersResponse : BaseResponse<IEnumerable<UserModel>>
    {

    }

    [Route("/db/api/user/listFollowing")]
    [DataContract]
    public class ListFollowing : BaseList
    {
        [DataMember(Name = "user")]
        public string Email { get; set; }
    }

    [DataContract]
    public class ListFollowingResponse : BaseResponse<IEnumerable<UserModel>>
    {

    }

    [Route("/db/api/user/listPosts/")]
    [DataContract]
    public class UserListPosts : BaseList
    {
        [DataMember(Name = "user")]
        public string User { get; set; }
    }

    [Route("/db/api/user/unfollow/")]
    [DataContract]
    public class Unfollow : BaseFollow
    {

    }

    [DataContract]
    public class UnfollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/db/api/user/updateProfile/")]
    [DataContract]
    public class UpdateProfile : UserModel
    {
        [DataMember(Name="user")]
        public override string Email { get; set; }
    }

    [DataContract]
    public class UpdateProfileResponse : BaseResponse<UserModel>
    {

    }

    [DataContract]
    public class UserListPostsResponse : BaseResponse<IEnumerable<PostModel<int, string, string, int?>>>
    {

    }
}
