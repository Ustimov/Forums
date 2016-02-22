using System;
using System.Runtime.Serialization;

namespace Forum.Models
{
    [DataContract]
    public class PostModel
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

        [DataMember(Name = "forum")]
        public ForumModel Forum { get; set; }

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

        [DataMember(Name = "parent")]
        public PostModel Parent { get; set; }

        [DataMember(Name = "points")]
        public int Points { get; set; }

        [DataMember(Name = "thread")]
        public ThreadModel<string> Thread { get; set; }

        [DataMember(Name = "user")]
        public string User { get; set; }
    }
}
 