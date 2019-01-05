using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Models.UserModel
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Files = new List<FileModel.File>();
        }

        public int UserId { get; set; }

        [Required] [StringLength(100)] public string Login { get; set; }

        [Required] [StringLength(100)] public string Email { get; set; }

        [Required] [StringLength(100)] public string Password { get; set; }

        public ICollection<FileModel.File> Files { get; set; }
    }
}