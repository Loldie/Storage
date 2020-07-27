using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storage.Models;
using Storage.ViewModels;
using System.Security.Cryptography;

namespace Storage.Controllers
{
    public class HomeController : Controller
    {

        private StorageContext db;

        public HomeController(StorageContext context)
        {
            db = context;
        }

        [Route("")]
        public IActionResult Index()
        {

            IEnumerable<DayFilesListViewModel> filesData = db.UserStorageFileEntries.Where(u => u.UserName == User.Identity.Name)
                .Join(db.StorageFiles, u => u.StorageFileId, s => s.Id, (u, s) => new StorageFileModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    UploadDate = u.UploadDate
                }).OrderByDescending(s => s.UploadDate).ToList().GroupBy(s => s.UploadDate.Date).Select(s => new DayFilesListViewModel
                {
                    Date = s.Key,
                    storageFileModels = s.Select(p => p).ToList()
                });
                
     
            return View(filesData);
        }

        [HttpPost]
        public IActionResult AddFile(IFormFileCollection uploads)
        {

            DayFilesListViewModel newFiles = new DayFilesListViewModel() { storageFileModels = new List<StorageFileModel>() };


            foreach (var uploadedFile in uploads)
            {
                
                byte[] fileData = null;

                using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    fileData = binaryReader.ReadBytes((int)uploadedFile.Length);
                }

                byte[] hashCode = null;

                using (SHA256 sha256 = SHA256.Create())
                {
                    hashCode = sha256.ComputeHash(fileData);
                }


                var existFile = db.StorageFiles.FirstOrDefault(s => s.HashCode == hashCode);
                if (existFile != null)
                {
                    var existEntry = db.UserStorageFileEntries.FirstOrDefault(u => u.UserName == User.Identity.Name && u.StorageFileId == existFile.Id);
                    if (existEntry == null)
                    {
                        UserStorageFileEntry userStorageOldFileEntry = new UserStorageFileEntry { StorageFile = existFile, UserName = User.Identity.Name };
                        db.UserStorageFileEntries.Add(userStorageOldFileEntry);
                        db.SaveChanges();
                        newFiles.storageFileModels.Add(new StorageFileModel { Id = existFile.Id, Name = existFile.Name, UploadDate = userStorageOldFileEntry.UploadDate });
                    }
                    continue;

                }

                StorageFile storageFile = new StorageFile { ContentType = uploadedFile.ContentType, Name = uploadedFile.FileName, HashCode = hashCode };
                FileBlob fileBlob = new FileBlob { Blob = fileData, StorageFile = storageFile };
                UserStorageFileEntry userStorageNewFileEntry = new UserStorageFileEntry { StorageFile = storageFile, UserName = User.Identity.Name };

                db.StorageFiles.Add(storageFile);
                db.FileBlobs.Add(fileBlob);
                db.UserStorageFileEntries.Add(userStorageNewFileEntry);
                db.SaveChanges();
                newFiles.storageFileModels.Add(new StorageFileModel { Id = storageFile.Id, Name = storageFile.Name, UploadDate = userStorageNewFileEntry.UploadDate });

            }

            if (newFiles.storageFileModels.Count() != 0)
            {
                newFiles.Date = newFiles.storageFileModels.First().UploadDate.Date;
            }
            return PartialView(newFiles);
        }

        [HttpPost]
        public IActionResult DownloadFile(int fileId)
        {
            StorageFile downloadFile = db.StorageFiles.Include(s => s.FileBlob).FirstOrDefault(s => s.Id == fileId);

            return File(downloadFile.FileBlob.Blob, downloadFile.ContentType, downloadFile.Name);
        }

        [HttpPost]
        public IActionResult DeleteFile(int fileId)
        {
            UserStorageFileEntry delEntry = db.UserStorageFileEntries.Include(u => u.StorageFile).ThenInclude(s => s.UserStorageFileEntries).FirstOrDefault(u => u.UserName == User.Identity.Name && u.StorageFileId == fileId);
            db.UserStorageFileEntries.Remove(delEntry);

            if (delEntry.StorageFile.UserStorageFileEntries.Count == 1)
                db.StorageFiles.Remove(delEntry.StorageFile);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
