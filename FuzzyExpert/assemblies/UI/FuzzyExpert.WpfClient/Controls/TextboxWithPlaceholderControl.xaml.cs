using System.Windows;
using System.Windows.Controls;

namespace FuzzyExpert.WpfClient.Controls
{
    public partial class TextboxWithPlaceholderControl : UserControl
    {
        public TextboxWithPlaceholderControl()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextboxWithPlaceholderControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TextboxWithPlaceholderControl));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register("TooltipText", typeof(string), typeof(TextboxWithPlaceholderControl), new UIPropertyMetadata(string.Empty));

        public bool TooltipText
        {
            get => (bool)GetValue(TooltipTextProperty);
            set => SetValue(TooltipTextProperty, value);
        }

        public static readonly DependencyProperty IsBigTextProperty =
            DependencyProperty.Register("IsBigText", typeof(bool), typeof(TextboxWithPlaceholderControl), new UIPropertyMetadata(false));

        public bool IsBigText
        {
            get => (bool)GetValue(IsBigTextProperty);
            set => SetValue(IsBigTextProperty, value);
        }
    }
}