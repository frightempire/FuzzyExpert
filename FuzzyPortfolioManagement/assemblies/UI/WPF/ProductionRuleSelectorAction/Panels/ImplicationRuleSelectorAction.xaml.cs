using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ProductionRuleSelectorAction.Panels
{
    public partial class ImplicationRuleSelectorAction : Window
    {
        public ImplicationRuleSelectorAction()
        {
            InitializeComponent();
        }


        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            bool? fileDialogResult = openFileDialog.ShowDialog();

            if (fileDialogResult == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
                ImplicationRulesTextBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
    }
}
