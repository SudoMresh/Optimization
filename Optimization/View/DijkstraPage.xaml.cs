using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Dijkstra;

namespace Optimization.View
{
    /// <summary>
    /// Interaction logic for DijkstraPage.xaml
    /// </summary>
    public partial class DijkstraPage : Page
    {
        private DijkstraModule dijkstraModule;
        private Graph graph = new Graph();

        public string AddVertexName { get; set; }
        public double AddValue { get; set; }
        public string FinishVertexName { get; set; }
        public string StartSolveVertexName { get; set; }
        public string FinishSolveVertexName { get; set; }
        public string StartVertexName { get; set; }
        public double WeightEdge { get; set; }
        public ObservableCollection<string> Path { get; private set; }

        public DijkstraPage()
        {
            Path = new ObservableCollection<string>();

            InitializeComponent();
        }

        public void OnAddVertexName(object sender, RoutedEventArgs e)
        {
            AddVertexName = AddKeyInput.Text;
            var foundVertex = graph.FindVertex(AddVertexName);

            if (foundVertex == null)
            {
                graph.AddVertex(AddVertexName);
                Path.Add(string.Format("Вершина {0} добавлена", AddVertexName));
                path.ItemsSource = Path;
            }
            else
                MessageBox.Show("Вы ввели уже этоту вершину!");
        }
        public void OnSolveButtonClick(object sender, RoutedEventArgs e)
        {
            dijkstraModule = new DijkstraModule(graph);
            StartSolveVertexName = AddStartSolveVertex.Text;
            FinishSolveVertexName = AddFinishSolveVertex.Text;
            Path.Add(dijkstraModule.FindShortestPath(StartSolveVertexName, FinishSolveVertexName));
            path.ItemsSource = Path;
        }
        public void OnAddEdgeButtonClick(object sender, RoutedEventArgs e)
        {
            StartVertexName = AddStartVertex.Text;
            FinishVertexName = AddFinishVertex.Text;
            WeightEdge = double.Parse(WeightEdgeEl.Text);
            graph.AddEdge(StartVertexName, FinishVertexName, WeightEdge);
            Path.Add(string.Format("Добавлен путь с вершины {0} в вершину {1} с весом {2}", StartVertexName, FinishVertexName, WeightEdge));
            path.ItemsSource = Path;
        }
    }
}
