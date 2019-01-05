using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FileSharing.ViewModels
{
    public class FileUploadModel
    {
        [Required(ErrorMessage = "Выберите файл")]
        public IFormFile UploadFile { get; set; }

        public string Password { get; set; }
    }

}