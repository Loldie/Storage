using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class StorageFile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] HashCode { get; set; }

        public FileBlob FileBlob { get; set; }

        public List<UserStorageFileEntry> UserStorageFileEntries { get; set; }
    }
}
