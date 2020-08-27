using System.Windows;
using System.Windows.Controls;

namespace FuzzyExpert.WpfClient.Controls
{
    public partial class TooltipTextboxControl : UserControl
    {
        public TooltipTextboxControl()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TooltipTextboxControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(string), typeof(TooltipTextboxControl));

        public string Tooltip
        {
            get => (string)GetValue(TooltipProperty);
            set => SetValue(TooltipProperty, value);
        }

        public static readonly DependencyProperty IsBigTextProperty =
            DependencyProperty.Register("IsBigText", typeof(bool), typeof(TooltipTextboxControl), new UIPropertyMetadata(false));

        public bool IsBigText
        {
            get => (bool)GetValue(IsBigTextProperty);
            set => SetValue(IsBigTextProperty, value);
        }
    }
}
