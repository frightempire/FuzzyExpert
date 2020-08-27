using Microsoft.Win32;

namespace FuzzyExpert.WpfClient.Helpers
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