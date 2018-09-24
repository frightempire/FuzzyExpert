using Microsoft.Win32;
using UILogic.Common.Interfaces;

namespace UILogic.Common.Implementations
{
    public class ImplicationRuleFileDialogInteractor: IFileDialogInteractor
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            bool? fileDialogResult = openFileDialog.ShowDialog();

            if (fileDialogResult != true)
                return false;

            FilePath = openFileDialog.FileName;
            return true;
        }
    }
}
