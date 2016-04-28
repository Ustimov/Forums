using System;
using ServiceStack.ServiceInterface;
using Forum.Dtos.Post;
using Forum.Dtos.Base;
using Forum.Helpers;

namespace Forum.Services
{
    public class PostService : Service
    {
        public object Post(CreatePost cp)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                cnn.CreatePost(cp);
                cp.Id = cnn.LastInsertId();

                /*
                var parentPath = String.Empty;

                if (request.Parent != null)
                {
                    parentPath = ConnectionProvider.DbConnection.ExecuteScalar<string>(
                        @"select Path from Post where Id=@Post", new { Post = request.Parent });
                }

                ConnectionProvider.DbConnection.Execute(
                    @"update Post set Path=@Path where Id=@Post",
                    new
                    {
                        Post = post.Id,
                        Path = (parentPath == String.Empty ? String.Empty : parentPath + ".") + post.Id.ToString("D10"),
                    });
                */

                return new CreatePostResponse
                {
                    Code = StatusCode.Ok,
                    Response = cp,
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(PostDetails pd)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;
                var post = cnn.ReadPost(pd.Post);

                if (post == null)
                {
                    return new ErrorResponse { Code = StatusCode.ObjectNotFound, Response = "Post Not Found" };
                }

                if (pd.Related != null)
                {
                    if (pd.Related.Contains("user"))
                    {
                        post.User = cnn.ReadUser(post.User as string);
                    }

                    if (pd.Related.Contains("thread"))
                    {
                        post.Thread = cnn.ReadThread(post.Thread as int?);
                    }

                    if (pd.Related.Contains("forum"))
                    {
                        post.Forum = cnn.ReadForum(post.Forum as string);
                    }
                }

                return new PostDetailsResponse { Code = StatusCode.Ok, Response = post };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Get(PostListPosts plp)
        {
            try
            {
                var posts = ConnectionProvider.DbConnection.ReadAllPosts(
                    plp.Forum, plp.Thread, plp.Since, plp.Order, plp.Limit);

                return new PostListPostsResponse
                {
                    Code = StatusCode.Ok,
                    Response = posts
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(RemovePost rp)
        {
            try
            {
                ConnectionProvider.DbConnection.RemovePost(rp);

                return new RemovePostResponse { Code = StatusCode.Ok, Response = rp.Post };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(RestorePost rp)
        {
            try
            {
                ConnectionProvider.DbConnection.RestorePost(rp);

                return new RestorePostResponse { Code = StatusCode.Ok, Response = rp.Post };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(UpdatePost up)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;

                return new UpdatePostResponse
                {
                    Code = StatusCode.Ok,
                    Response = cnn.ReadPost(up.Post),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }

        public object Post(VotePost vp)
        {
            try
            {
                var cnn = ConnectionProvider.DbConnection;

                if (vp.Value == 1)
                {
                    cnn.LikePost(vp);
                }
                else if (vp.Value == -1)
                {
                    cnn.DislikePost(vp);
                }

                return new VoteThreadResponse
                {
                    Code = StatusCode.Ok,
                    Response = cnn.ReadPost(vp.Post),
                };
            }
            catch (Exception e)
            {
                return new ErrorResponse { Code = StatusCode.UndefinedError, Response = e.Message };
            }
        }
    }
}
