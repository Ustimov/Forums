using System;
using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class BaseThreadModel
    {
        [IgnoreDataMember]
        public DateTime Date { get; set; }

        [DataMember(Name = "date")]
        public string DateString
        {
            get { return Date.ToString("yyyy-MM-dd HH-mm-ss"); }
            set { Date = DateTime.Parse(value); }
        }

        [DataMember(Name = "dislikes")]
        public int Dislikes { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "isClosed")]
        public bool IsClosed { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "likes")]
        public int Likes { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "points")]
        public int Points { get; set; }

        [DataMember(Name = "posts")]
        public int Posts { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        public BaseThreadModel()
        {

        }

        public BaseThreadModel(BaseThreadModel baseThreadModel)
        {
            Date = baseThreadModel.Date;
            Dislikes = baseThreadModel.Dislikes;
            Id = baseThreadModel.Id;
            IsClosed = baseThreadModel.IsClosed;
            IsDeleted = baseThreadModel.IsDeleted;
            Likes = baseThreadModel.Likes;
            Message = baseThreadModel.Message;
            Points = baseThreadModel.Points;
            Posts = baseThreadModel.Posts;
            Slug = baseThreadModel.Slug;
            Title = baseThreadModel.Title;
        }
    }
}