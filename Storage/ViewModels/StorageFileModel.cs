using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.ViewModels
{
    public class StorageFileModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
