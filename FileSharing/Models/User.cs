using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Models
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            this.Files = new List<File>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public ICollection<File> Files { get; set; }
    }
}
