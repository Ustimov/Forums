using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceHost;
using Forum.Dtos.Base;
using Forum.Models;

namespace Forum.Dtos.User
{
    [Route("/user/create/")]
    [DataContract]
    public class Create : UserModel
    {
        // isAnonymous - optional
        // Required: username, name, about, email
    }

    [DataContract]
    public class CreateResponse : BaseResponse<UserModel>
    {

    }

    [Route("/user/details/")]
    [DataContract]
    public class Details : BaseUser
    {

    }

    [DataContract]
    public class DetailsResponse : BaseResponse<UserModel>
    {

    }

    [Route("/user/follow/")]
    [DataContract]
    public class Follow : BaseFollow
    {

    }

    [DataContract]
    public class FollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/user/listFollowers/")]
    [DataContract]
    public class ListFollowers : BaseList
    {

    }

    [DataContract]
    public class ListFollowersResponse : BaseResponse<List<UserModel>>
    {

    }

    [Route("/user/listFollowing")]
    [DataContract]
    public class ListFollowing : BaseList
    {

    }

    [DataContract]
    public class ListFollowingResponse : BaseResponse<List<UserModel>>
    {

    }

    [Route("/user/listPosts/")]
    [DataContract]
    public class ListPosts : BaseList
    {

    }

    [DataContract]
    public class ListPostsResponse : BaseResponse<List<PostModel<int, string>>>
    {

    }

    [Route("/user/unfollow/")]
    [DataContract]
    public class Unfollow : BaseFollow
    {

    }

    [DataContract]
    public class UnfollowResponse : BaseResponse<UserModel>
    {

    }

    [Route("/user/updateProfile/")]
    [DataContract]
    public class UpdateProfile : UserModel
    {

    }

    [DataContract]
    public class UpdateProfileResponse : BaseResponse<UserModel>
    {

    }
}
