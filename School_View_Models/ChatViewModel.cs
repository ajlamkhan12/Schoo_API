using System;
using System.Collections.Generic;

namespace School_View_Models
{
    public class ChatViewModel
    {
        public int? Id { get; set; }

        public int SenderId { get; set; }

        public int? RecieverId { get; set; }

        public int? GroupId { get; set; }

        public int? MessageType { get; set; }

        public int? Reply_To_Message_Id { get; set; }

        public string Content { get; set; }

        public bool? IsViewed { get; set; }

        public string Media_Url { get; set; }
        public string SenderName { get; set; }

        //public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime CreatedDate { get; set; }
        public bool IsGroup { get; set; }

        //public DateTime? DeletedOn { get; set; }

        //public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        //public int? UpdatedBy { get; set; }

        //public int? CreatedBy { get; set; }

        //public int? DeletedBy { get; set; }

        //public int? DeletedFor { get; set; }
    }

    public class CommunicationModel
    {
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public int GroupId { get; set; }
        public string MessageType { get; set; }
        public int Reply_To_Message_Id { get; set; }
        public string Content { get; set; }
        public bool IsViewed { get; set; }
        public string Media_Url { get; set; }
    }
}
