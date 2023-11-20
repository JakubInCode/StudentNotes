using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace Notatki_studenckie
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// </summary>
    public partial class TextEditor : Window
    {
        private string _subjectFolder;

        public TextEditor(string subjectFolder)
        {
            InitializeComponent();
            _subjectFolder = subjectFolder;
        }

        public string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        private void font_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (font_cb.SelectedItem is ComboBoxItem selectedItem)
            {
                string? fontName = selectedItem.Content.ToString();
                textBox.FontFamily = new System.Windows.Media.FontFamily(fontName);
            }
        }

        private void fontSize_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontSize_cb.SelectedItem is ComboBoxItem selectedItem &&
                int.TryParse(selectedItem.Content.ToString(), out int fontSize))
            {
                textBox.FontSize = fontSize;
            }
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.InitialDirectory = _subjectFolder;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(filePath, textBox.Text);
                    MessageBox.Show("Plik został zapisany.", "Zapis", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas zapisywania pliku: " + ex.Message, "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

