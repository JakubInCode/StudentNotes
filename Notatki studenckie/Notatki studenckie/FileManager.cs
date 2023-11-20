using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notatki_studenckie
{
    public class FileManager
    {
        private ListBox _fileList_lb;

        public FileManager(ListBox fileList_lb)
        {
            _fileList_lb = fileList_lb;
        }

        public void LoadFiles(string folderPath)
        {
            _fileList_lb.Items.Clear();

            try
            {
                string[] files = Directory.GetFiles(folderPath);

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);

                    FileItem fileItem = new FileItem
                    {
                        Name = fileName,
                        Path = filePath,
                        Icon = GetFileIcon(filePath)
                    };

                    _fileList_lb.Items.Add(fileItem);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BitmapSource GetFileIcon(string filePath)
        {
            using (System.Drawing.Icon? sysIcon = System.Drawing.Icon.ExtractAssociatedIcon(filePath))
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    sysIcon.Handle,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(16, 16)
                );
            }
        }

    }
}
