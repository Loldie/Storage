using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class StorageFile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public byte[] HashCode { get; set; }

        public List<UserStorageFileEntry> UserStorageFileEntries { get; set; }
    }
}
