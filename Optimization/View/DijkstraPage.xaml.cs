using Dijkstra;
using DijkstraModule;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Optimization.View
{
    /// <summary>
    /// Interaction logic for DijkstraPage.xaml
    /// </summary>
    public partial class DijkstraPage : Page
    {
        private Dijkstra.DijkstraModule dijkstraModule;
        private Graph graph = new Graph();

        public string AddVertexName { get; set; }
        public double AddValue { get; set; }
        public string FinishVertexName { get; set; }
        public string StartSolveVertexName { get; set; }
        public string FinishSolveVertexName { get; set; }
        public string StartVertexName { get; set; }
        public double WeightEdge { get; set; }
        public ObservableCollection<string> Path { get; private set; }

        private string _addedEdges = string.Empty;

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
            try
            {
                dijkstraModule = new Dijkstra.DijkstraModule(graph);
                StartSolveVertexName = AddStartSolveVertex.Text;
                FinishSolveVertexName = AddFinishSolveVertex.Text;
                Path.Add($"\nКороткий путь:\n");
                Path.Add(dijkstraModule.FindShortestPath(StartSolveVertexName, FinishSolveVertexName));
                path.ItemsSource = Path;
            }
            catch (NotFoundEdgeExeption ex)
            {
                MessageBox.Show(ex.ShowError());
            }
        }
        public void OnAddEdgeButtonClick(object sender, RoutedEventArgs e)
        {
            StartVertexName = AddStartVertex.Text;
            FinishVertexName = AddFinishVertex.Text;

            try
            {
                if (double.TryParse(WeightEdgeEl.Text, out double parsedVal))
                {
                    WeightEdge = parsedVal;
                    graph.AddEdge(StartVertexName, FinishVertexName, WeightEdge);
                    Path.Add(string.Format("Добавлен путь с вершины {0} в вершину {1} с весом {2}", StartVertexName, FinishVertexName, WeightEdge));
                    _addedEdges += $"{StartVertexName} {FinishVertexName} {WeightEdge} \n";
                    path.ItemsSource = Path;
                }
                else
                {
                    MessageBox.Show("Вы ввели не коректный вес.\nПожайлуста проверте правильность введеных данных");
                }
            }
            catch (NotFoundEdgeExeption ex)
            {
                MessageBox.Show(ex.ShowError());
            }
        }

        public void OnReadFromFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string[] allLines = File.ReadAllLines(openFileDialog.FileName);
            bool isVertex = false;
            bool isEdge = false;

            foreach (var item in allLines)
            {
                if (item == "vertex")
                {
                    isEdge = false;
                    isVertex = true;
                    continue;
                }

                if (item == "edge")
                {
                    isEdge = true;
                    isVertex = false;
                    continue;
                }

                if (isEdge)
                {
                    string[] vals = item.Split(' ');
                    try
                    {
                        if (double.TryParse(vals[2], out double parsedVal))
                        {
                            WeightEdge = parsedVal;
                            graph.AddEdge(vals[0], vals[1], WeightEdge);
                            Path.Add(string.Format("Добавлен путь с вершины {0} в вершину {1} с весом {2}", vals[0], vals[1], WeightEdge));
                            _addedEdges += $"{vals[0]} {vals[1]} {WeightEdge} \n";
                            path.ItemsSource = Path;
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели не коректный вес.\nПожайлуста проверте правильность введеных данных");
                        }
                    }
                    catch (NotFoundEdgeExeption ex)
                    {
                        MessageBox.Show(ex.ShowError());
                    }
                }

                if (isVertex)
                {
                    var foundVertex = graph.FindVertex(item);

                    if (foundVertex == null)
                    {
                        graph.AddVertex(item);
                        Path.Add(string.Format("Вершина {0} добавлена", item));
                        path.ItemsSource = Path;
                    }
                }
            }
        }

        public void OnSaveToFileClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;

            string writeStr = "vertex\n";
            List<string> edges = new List<string>();

            foreach (var item in graph.Vertices)
            {
                writeStr += item.Name + "\n";
            }

            writeStr += "edge\n" + _addedEdges;

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, writeStr);
        }
    }
}
