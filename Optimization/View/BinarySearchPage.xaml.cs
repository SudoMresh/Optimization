using BinarySearchTree;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Optimization.View
{
    /// <summary>
    /// Логика взаимодействия для BinarySearchPage.xaml
    /// </summary>
    public partial class BinarySearchPage : Page
    {
        private BinarySearchTree<int, string> _tree;

        public int AddKey { get; set; }
        public string AddValue { get; set; }
        public int SearchKey { get; set; }
        public int RemoveKey { get; set; }

        public ObservableCollection<BinarySearchTree<int, string>> Items { get; private set; }

        public BinarySearchPage()
        {
            DataContext = this;

            Items = new ObservableCollection<BinarySearchTree<int, string>>();

            _tree = new BinarySearchTree<int, string>(5, "Some value for key 5");
            _tree.AddRange(new Dictionary<int, string>
            {
                {4, "Some value for key 4" },
                {15, "Some value for key 15" },
                {1, "Some value for key 1" },
            });

            InitializeComponent();
            UpdateItemsList();
        }

        public void OnTraversTypeChanged(object sender, SelectionChangedEventArgs e) => UpdateItemsList();

        private void UpdateItemsList()
        {
            var traversType = (TraversalType)TraversTypeCB.SelectedIndex;
            var elements = _tree.ToList(traversType);

            Items.Clear();

            for (int i = 0; i < elements.Count; i++)
            {
                Items.Add(elements[i]);
            }
        }

        public void OnSearchButtonClick(object sender, RoutedEventArgs eventArgs)
        {
            string content;
            if (_tree.TryFind(SearchKey, out BinarySearchTree<int, string> node))
            {
                content = $"По ключу \'{SearchKey}\' было найдено значение \'{node.Value}\'";
            }
            else
            {
                content = $"Ключ \'{SearchKey}\' отсутствует в дереве.";
            }

            MessageBox.Show(content, "Поиск");
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs eventArgs)
        {
            if (string.IsNullOrWhiteSpace(AddValue))
            {
                MessageBox.Show($"Добавление пустого значения не допустимо.", "Внимание!");
                return;
            }

            BinarySearchTree<int, string>.Add(ref _tree, new BinarySearchTree<int, string>(AddKey, AddValue));
            UpdateItemsList();
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs eventArgs)
        {
            BinarySearchTree<int, string>.Remove(ref _tree, RemoveKey);
            UpdateItemsList();
        }
    }
}
