
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DbModels
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }

        public int? RecieverId { get; set; }
        [ForeignKey(nameof(RecieverId))]
        public User? Reciever { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }

        [Required]
        public int MessageType { get; set; }

        public int? Reply_To_Message_Id { get; set; }

        public string Content { get; set; }

        public bool IsViewed { get; set; }

        public string Media_Url { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedOn { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public User? Updated { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User? Created { get; set; }

        public int? DeletedBy { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        public User? Deleted { get; set; }

        public int? DeletedFor { get; set; }
        [ForeignKey(nameof(DeletedFor))]
        public User? Deleted_For { get; set; }
    }
}
