using System;
using System.Runtime.Serialization;

namespace Forum.Models
{
    public class BasePostModel
    {
        [IgnoreDataMember]
        public DateTime Date { get; set; }

        [DataMember(Name = "date")]
        public string DateString
        {
            get { return Date.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { Date = DateTime.Parse(value); }
        }

        [DataMember(Name = "dislikes")]
        public int Dislikes { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "isApproved")]
        public bool IsApproved { get; set; }

        [DataMember(Name = "isEdited")]
        public bool IsEdited { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "isHighlighted")]
        public bool IsHighlighted { get; set; }

        [DataMember(Name = "isSpam")]
        public bool IsSpam { get; set; }

        [DataMember(Name = "likes")]
        public int Likes { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "points")]
        public int Points { get { return Likes - Dislikes; } }

        public BasePostModel()
        {

        }

        public BasePostModel(BasePostModel basePostModel)
        {
            Date = basePostModel.Date;
            Dislikes = basePostModel.Dislikes;
            Id = basePostModel.Id;
            IsApproved = basePostModel.IsApproved;
            IsEdited = basePostModel.IsEdited;
            IsDeleted = basePostModel.IsDeleted;
            IsHighlighted = basePostModel.IsHighlighted;
            IsSpam = basePostModel.IsSpam;
            Likes = basePostModel.Likes;
            Message = basePostModel.Message;
        }
    }
}
