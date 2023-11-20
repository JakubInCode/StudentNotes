using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Notatki_studenckie
{
    internal class FileItem
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public BitmapSource? Icon { get; set; }
    }
}
