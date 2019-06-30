namespace UILogic.Common.Interfaces
{
    public interface IFileDialogInteractor
    {
        string FilePath { get; set; }

        bool OpenFileDialog();
    }
}
