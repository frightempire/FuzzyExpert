using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FuzzyExpert.WpfClient.Annotations;
using FuzzyExpert.WpfClient.Models;

namespace FuzzyExpert.WpfClient.Controls
{
    public partial class FilteringListControl : UserControl, INotifyPropertyChanged
    {
        public FilteringListControl()
        {
            InitializeComponent();
            Items = new ObservableCollection<ContentModel>();
            Root.DataContext = this;

            DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem, obj => true);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<ContentModel>), typeof(FilteringListControl));

        public ObservableCollection<ContentModel> Items
        {
            get => (ObservableCollection<ContentModel>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        private ICollectionView _itemView;

        private string _filter;
        public string Filter
        {
            get => _filter;
            set
            {
                if (value == _filter)
                {
                    return;
                }

                _filter = value;

                _itemView = CollectionViewSource.GetDefaultView(Items);
                _itemView.Filter = o => string.IsNullOrEmpty(_filter) || ((ContentModel)o).Content.Contains(_filter);
                _itemView.Refresh();

                OnPropertyChanged(nameof(Filter));
            }
        }

        public RelayCommand DeleteSelectedItemCommand { get; }

        private void DeleteSelectedItem(object list)
        {
            var listBox = (ListBox)list;
            if (listBox.Items.Count <= 0)
            {
                return;
            }
            IEditableCollectionView items = listBox.Items;
            items.Remove(listBox.SelectedItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}