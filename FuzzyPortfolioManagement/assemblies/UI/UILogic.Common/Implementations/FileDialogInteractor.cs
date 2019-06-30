using Microsoft.Win32;
using UILogic.Common.Interfaces;

namespace UILogic.Common.Implementations
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