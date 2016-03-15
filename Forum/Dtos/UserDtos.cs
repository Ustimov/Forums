using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Dtos.Base;
using Forum.Models;

namespace Forum.Dtos.User
{
    [Route("/dbdb/api/user/create/")]
    [DataContract]
    public class CreateUser : UserModel
    {
        // isAnonymous - optional
        // Required: username, name, about, email
    }

    [DataContract]
    public class CreateResponse : BaseResponse<UserModel>
    {

    }

    [Route("/dbdb/api/user/details/")]
    [DataContract]
    public class UserDetails : BaseUser
    {

    }

    [DataContract]
    public class DetailsResponse : BaseResponse<UserModel>
    {

    }

    [Route("/dbdb/api/user/follow/")]
    [DataContract]
    public class Follow : BaseFollow
    {

    }

    [DataContract]
    public class FollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/dbdb/api/user/listFollowers/")]
    [DataContract]
    public class ListFollowers : BaseList
    {
        [DataMember(Name = "user")]
        public string Email { get; set; }
    }

    [DataContract]
    public class ListFollowersResponse : BaseResponse<List<UserModel>>
    {

    }

    [Route("/dbdb/api/user/listFollowing")]
    [DataContract]
    public class ListFollowing : BaseList
    {
        [DataMember(Name = "user")]
        public string Email { get; set; }
    }

    [DataContract]
    public class ListFollowingResponse : BaseResponse<List<UserModel>>
    {

    }

    [Route("/dbdb/api/user/listPosts/")]
    [DataContract]
    public class ListPosts : BaseList
    {

    }

    [DataContract]
    public class ListPostsResponse : BaseResponse<List<PostModel<int, string, string, int>>>
    {

    }

    [Route("/dbdb/api/user/unfollow/")]
    [DataContract]
    public class Unfollow : BaseFollow
    {

    }

    [DataContract]
    public class UnfollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/dbdb/api/user/updateProfile/")]
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
}
