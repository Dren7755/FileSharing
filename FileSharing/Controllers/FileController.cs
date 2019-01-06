﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FileSharing.Models.FileModel;
using FileSharing.Models.UserModel;
using FileSharing.Infrastructure;
using FileSharing.Models.LinkModel;
using FileSharing.ViewModels;

namespace FileSharing.Controllers
{
    public class FileController : Controller
    {
        private DataContext dataContext;

        private IHostingEnvironment hostingEnvironment;

        public FileController(DataContext dataContext, IHostingEnvironment hostingEnvironment)
        {
            this.dataContext = dataContext;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            return View(dataContext.Files.ToList().OrderByDescending(f => f.CreatedDate));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(FileUploadModel fileUploadModel)
        {
            if (!ModelState.IsValid) return View();

            IFormFile uploadFile = fileUploadModel.UploadFile;
            User currentUser = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            string filePath = FileHelper.CreateFilePath(
                hostingEnvironment.WebRootPath,
                uploadFile.FileName,
                currentUser.Login
            );
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(fileStream);
            }

            Models.FileModel.File file = new Models.FileModel.File
            {
                FileName = uploadFile.FileName,
                RealPath = filePath,
                Size = uploadFile.Length,
                ContentType = uploadFile.ContentType,
                CreatedDate = DateTime.UtcNow,
                User = currentUser
            };
            file.Link = LinkFactory.CreateLink(file, fileUploadModel.Password);
            dataContext.Files.Add(file);
            await dataContext.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Get(string id)
        {
            Models.FileModel.File file = await LoadFile(f => f.Link.Uri == id);
            if (file.Link.AccessPassword == null)
                return PhysicalFile(file.RealPath, file.ContentType, file.FileName);

            ViewBag.fileId = file.FileId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Get(int fileId, string accessPassword)
        {
            Models.FileModel.File file = await LoadFile(f => f.FileId == fileId);
            if (file.Link.AccessPassword == accessPassword)
                return PhysicalFile(file.RealPath, file.ContentType, file.FileName);

            ModelState.AddModelError("", "Неверный пароль доступа к файлу");
            ViewBag.fileId = file.FileId;
            return View();
        }

        private async Task<Models.FileModel.File> LoadFile(
            System.Linq.Expressions.Expression<Func<Models.FileModel.File, bool>> predicate
        )
        {
            Models.FileModel.File file = await dataContext.Files.FirstOrDefaultAsync(predicate);
            if (file == null)
                throw new Exception("File not found");
            await dataContext.Entry(file).Reference(f => f.Link).LoadAsync();
            return file;
        }
    }
}