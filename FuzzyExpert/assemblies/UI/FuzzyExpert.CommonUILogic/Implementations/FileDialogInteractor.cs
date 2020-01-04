using FuzzyExpert.CommonUILogic.Interfaces;
using Microsoft.Win32;

namespace FuzzyExpert.CommonUILogic.Implementations
{
    public class FileDialogInteractor: IFileDialogInteractor
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".txt|.csv"
            };

            bool? fileDialogResult = openFileDialog.ShowDialog();

            if (fileDialogResult != true)
                return false;

            FilePath = openFileDialog.FileName;
            return true;
        }
    }
}