using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Models
{
    [Table("Links")]
    public class Link
    {
        public int LinkId { get; set; }

        [StringLength(100)]
        public string AccessPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string Uri { get; set; }

        [Required]
        public int FileId { get; set; }
        [Required]
        public File File { get; set; }
    }
}
