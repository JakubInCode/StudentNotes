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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows;
using System.IO;
using System.Diagnostics;

namespace Notatki_studenckie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DirectoryManager _directoryManager;
        private FileManager _fileManager;

        public MainWindow()
        {
            InitializeComponent();

            _directoryManager = new DirectoryManager();

            _directoryManager.CreateMainFolder();
            _directoryManager.CreateSemesterFolders();

            _fileManager = new FileManager(fileList_lb);

            semester_cb.SelectionChanged += semester_cb_SelectionChanged;
        }

        private void textEditor_btn_Click(object sender, RoutedEventArgs e)
        {
            if (semester_cb.SelectedItem is ComboBoxItem selectedSemesterItem && subject_cb.SelectedItem is string selectedSubject)
            {
                string? semesterFolderName = selectedSemesterItem.Content.ToString();
                string? semesterFolderPath = System.IO.Path.Combine(_directoryManager.DesktopPath, _directoryManager.MainFolderName, semesterFolderName, selectedSubject);

                if (!string.IsNullOrEmpty(semesterFolderPath))
                {
                    TextEditor _textEditor = new TextEditor(semesterFolderPath);
                    _textEditor.Closed += (s, args) => chooseSubject_btn_Click(null, null);
                    _textEditor.ShowDialog();
                }

            }
        }

        private void semester_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (semester_cb.SelectedItem is ComboBoxItem selectedSemesterItem)
            {
                string? semesterFolderName = selectedSemesterItem.Content.ToString();
                string? semesterFolderPath = System.IO.Path.Combine(_directoryManager.DesktopPath, _directoryManager.MainFolderName, semesterFolderName);

                if (Directory.Exists(semesterFolderPath))
                {
                    string[] subjectFolders = Directory.GetDirectories(semesterFolderPath);
                    string[] subjectNames = subjectFolders.Select(folder => System.IO.Path.GetFileName(folder)).ToArray();

                    subject_cb.ItemsSource = subjectNames;
                }
                else
                {
                    subject_cb.ItemsSource = null;
                }
            }
        }

        private void subject_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseSubject_btn.IsEnabled = (subject_cb.SelectedItem != null);
        }

        private void uploadFile_btn_Click(object sender, RoutedEventArgs e)
        {
            if (subject_cb.SelectedItem is string selectedSubject)
            {
                string? selectedSemester = (semester_cb.SelectedItem as ComboBoxItem)?.Content.ToString();
                string folderPath = System.IO.Path.Combine(_directoryManager.DesktopPath, _directoryManager.MainFolderName, selectedSemester, selectedSubject);

                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Multiselect = true;

                if (Directory.Exists(folderPath))
                {
                    openFileDialog.InitialDirectory = folderPath;
                }

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        foreach (string filePath in openFileDialog.FileNames)
                        {
                            string fileName = System.IO.Path.GetFileName(filePath);
                            string destinationPath = System.IO.Path.Combine(folderPath, fileName);
                            File.Copy(filePath, destinationPath, overwrite: false);
                        }

                        MessageBox.Show("Pliki zostały dodane.", "Dodawanie plików", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd podczas dodawania plików: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    chooseSubject_btn_Click(null, null);
                }
            }
        }

        private void openFile_btn_Click(object sender, RoutedEventArgs e)
        {
            if (fileList_lb.SelectedItem is FileItem selectedFile)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = selectedFile.Path,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd otwierania pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void chooseSubject_btn_Click(object sender, RoutedEventArgs e)
        {
            string? selectedSemester = (semester_cb.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? selectedSubject = (subject_cb.SelectedItem as string);

            if (!string.IsNullOrEmpty(selectedSemester) && !string.IsNullOrEmpty(selectedSubject))
            {
                string? folderPath = System.IO.Path.Combine(_directoryManager.DesktopPath, _directoryManager.MainFolderName, selectedSemester, selectedSubject);
                _fileManager.LoadFiles(folderPath);
            }
            else
            {
                MessageBox.Show("Wybierz semestr i przedmiot!");
            }
        }

        private void deleteFile_btn_Click(object sender, RoutedEventArgs e)
        {
            if (fileList_lb.SelectedItem is FileItem selectedFile)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten plik?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        File.Delete(selectedFile.Path);
                        MessageBox.Show("Plik został usunięty.", "Usuwanie pliku", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd podczas usuwania pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                chooseSubject_btn_Click(null, null);
            }
        }
    }
}
