namespace FuzzyExpert.CommonUILogic.Interfaces
{
    public interface IFileDialogInteractor
    {
        string FilePath { get; set; }

        bool OpenFileDialog();
    }
}