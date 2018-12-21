using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Models
{
    [Table("Files")]
    public class File
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int LinkId { get; set; }

        [Required]
        [StringLength(100)]
        public string Filename { get; set; }

        [Required]
        [StringLength(512)]
        public string RealPath { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiresDate { get; set; }
    }
}
