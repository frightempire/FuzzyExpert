namespace FuzzyExpert.WpfClient.Helpers
{
    public interface IFileDialogInteractor
    {
        string FilePath { get; set; }

        bool OpenFileDialog();
    }
}