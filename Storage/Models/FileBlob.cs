using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class FileBlob
    {

        public int Id { get; set; }

        [Required]
        public byte[] Blob { get; set; }

        public int StorageFileId { get; set; }
        public StorageFile StorageFile { get; set; }
    }
}
