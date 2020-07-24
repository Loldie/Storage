using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class UserStorageFileEntry
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime UploadDate { get; set; }

        public int StorageFileId { get; set; }

        public StorageFile StorageFile { get; set; }


    }
}
