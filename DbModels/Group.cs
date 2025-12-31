using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DbModels
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        [MaxLength(255)]
        public string? Admin { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedOn { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User? Created_By { get; set; }

        public int? DeletedBy { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        public User? Deleted_By { get; set; }

        public string? Group_Image_Url { get; set; }
    }
}
