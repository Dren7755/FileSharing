﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharing.Models.FileModel
{
    [Table("Files")]
    public class File
    {
        public int FileId { get; set; }

        [Required] [StringLength(100)] public string FileName { get; set; }

        [Required] [StringLength(512)] public string RealPath { get; set; }

        [Required] public long Size { get; set; }

        [Required] [StringLength(64)] public string ContentType { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiresDate { get; set; }

        [Required] public int UserId { get; set; }
        public UserModel.User User { get; set; }

        [Required] public LinkModel.Link Link { get; set; }
    }
}