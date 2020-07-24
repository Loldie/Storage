using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.ViewModels
{
    public class DayFilesListViewModel
    {
        public IEnumerable<StorageFileModel> storageFileModels { get; set; }

        public DateTime Date { get; set; }
    }
}
