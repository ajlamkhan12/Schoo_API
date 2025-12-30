
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DbModels
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }

        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public User? Member { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedOn { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User? Created { get; set; }

        public int? DeletedBy { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        public User? Deleted { get; set; }

        public string Group_Image_Url { get; set; }
    }
}
