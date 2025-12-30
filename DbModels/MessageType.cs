using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels
{
    public class MessageType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string MessageTypeName { get; set; } = string.Empty;
    }
}
