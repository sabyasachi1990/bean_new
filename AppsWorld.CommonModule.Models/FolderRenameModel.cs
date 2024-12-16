using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class FolderRenameModel
    {
        public string NewName { get; set; }
        public string CursorName { get; set; }
        public int FileShareName { get; set; }
        public string Path { get; set; }
        public string OldName { get; set; }
    }
}
