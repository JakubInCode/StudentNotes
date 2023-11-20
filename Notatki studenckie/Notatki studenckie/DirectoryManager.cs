using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki_studenckie
{
    internal class DirectoryManager
    {
        public string DesktopPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string MainFolderName { get; } = "Notatki studenckie";

        public void CreateMainFolder()
        {
            string mainFolderPath = Path.Combine(DesktopPath, MainFolderName);

            if (!Directory.Exists(mainFolderPath))
            {
                Directory.CreateDirectory(mainFolderPath);
            }
        }

        public void CreateSemesterFolders()
        {
            for (int i = 1; i <= 7; i++)
            {
                string semesterFolderName = "Semestr " + i;
                string semesterFolderPath = Path.Combine(DesktopPath, MainFolderName, semesterFolderName);

                if (!Directory.Exists(semesterFolderPath))
                {
                    Directory.CreateDirectory(semesterFolderPath);
                }

                string[] subjects = GetSubjectsForSemester(i);
                foreach (string subject in subjects)
                {
                    string subjectFolderPath = Path.Combine(semesterFolderPath, subject);
                    if (!Directory.Exists(subjectFolderPath))
                    {
                        Directory.CreateDirectory(subjectFolderPath);
                    }
                }
            }
        }

        private string[] GetSubjectsForSemester(int semester)
        {
            switch (semester)
            {
                case 1:
                    return new string[] { "Analiza matematyczna I", "Elektrotechnika", "Algebra liniowa" };
                case 2:
                    return new string[] { "Analiza matematyczna II", "Architektura komputerów", "Algorytmy i struktury danych" };
                case 3:
                    return new string[] { "Programowanie III", "Sieci komputerowe I", "Podstawy baz danych" };
                case 4:
                    return new string[] { "Programowanie IV", "Grafika komputerowa I", "Metody numeryczne" };
                case 5:
                    return new string[] { "Programowanie V", "Programowanie niskopoziomowe", "Inżynieria oprogramowania" };
                case 6:
                    return new string[] { "Programowanie VI", "Programowanie VII", "Systemy wizyjne" };
                case 7:
                    return new string[] { "Praca inżynierska", "Systemy sztucznej inteligencji" };
                default:
                    return new string[0];
            }
        }
    }
}
