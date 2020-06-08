using FloydWarshall;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Optimization.View
{
    public partial class FloydWarshallPage : Page
    {
        private FloydWarshallAlgo algorithm;

        public FloydWarshallPage()
        {
            InitializeComponent();

            this.algorithm = new FloydWarshallAlgo();
            // RefreshResultTextWithMatrix();
        }


        private void HandleMatrixCreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (CountPoint.Text == "")
            {
                MessageBox.Show("Вы не ввели количество вершин!"); return;
            }
            int size = Int32.Parse(CountPoint.Text);

            if (size <= 1) { MessageBox.Show("Количество вершин должно быть больше 1!"); return; }

            this.algorithm.CreateMatrix(size);

            this.RefreshResultTextWithMatrix();

        }

        private void RefreshResultTextWithMatrix()
        {
            result.Text = "";

            this.AppendResultText(this.algorithm.ToStringDistances());
        }

        private void AppendResultText(String txt)
        {
            result.Text = result.Text + "\n" + txt;
        }

        private void HandleAddVertexClick(object sender, RoutedEventArgs e)
        {
            if (!this.algorithm.IsCreated()) { MessageBox.Show("Матрица расстояний не создана!\nПопробуйте ввести количество вершин и нажать кнопку Создать матрицу расстояний."); return; }

            if (CountPoint.Text == "")
            {
                MessageBox.Show("Введите количество вершин!"); return;
            }

            int size = Int32.Parse(CountPoint.Text);

            if (StartVertex.Text == "")
            {
                MessageBox.Show("Введите начальную вершину!"); return;
            }
            int startVertex = Int32.Parse(StartVertex.Text);

            if (FinishVertex.Text == "")
            {
                MessageBox.Show("Введите конечную вершину!"); return;
            }
            int finishVertex = Int32.Parse(FinishVertex.Text);

            if (startVertex == finishVertex) { MessageBox.Show("Вершины должны быть разные."); return; }

            if (startVertex > size - 1 || finishVertex > size - 1) { MessageBox.Show("Предупреждение!\nОбратите внимание, что индексация для вершин начинает с 0 для записи их в матрицу расстояний и индекс не должен быть больше количества вершин!"); return; }

            if (WeightEdge.Text == "")
            {
                MessageBox.Show("Введите вес для заданой вершины!"); return;
            }
            int edgeWeight = Int32.Parse(WeightEdge.Text);

            this.algorithm.AddVertex(startVertex, finishVertex, edgeWeight);

            this.RefreshResultTextWithMatrix();
        }

        private void HandleSolveClick(object sender, RoutedEventArgs e)
        {
            if (!this.algorithm.IsCreated()) { MessageBox.Show("Матрица расстояний не создана!\nПопробуйте ввести количество вершин и нажать кнопку Создать матрицу расстояний."); return; }

            this.algorithm.Solve();

            this.AppendResultText("Результат:\n" + this.algorithm.ToStringDistances());
            this.AppendResultText("Матрица предков:\n" + this.algorithm.ToStringParents());
        }

        private void HandleFindPathClick(object sender, RoutedEventArgs e)
        {
            if (!this.algorithm.IsCreated()) { MessageBox.Show("Матрица расстояний не создана!\nПопробуйте ввести количество вершин и нажать кнопку Создать матрицу расстояний."); return; }

            if (AddStartSolveVertex.Text == "")
            {
                MessageBox.Show("Введите начальную вершину для поиска пути!"); return;
            }
            int startVertexPath = Int32.Parse(AddStartSolveVertex.Text);

            if (AddFinishSolveVertex.Text == "")
            {
                MessageBox.Show("Введите конечную вершину для поиска пути!"); return;
            }
            int finishVertexPath = Int32.Parse(AddFinishSolveVertex.Text);

            if (AddStartSolveVertex.Text == "" && AddFinishSolveVertex.Text == "")
            {
                MessageBox.Show("Введите начальную и конечную вершины для поиска пути!"); return;
            }
            int size = this.algorithm.GetMatrixSize();

            if (startVertexPath == finishVertexPath) { MessageBox.Show("Вершины должны быть разные."); return; }
            if (startVertexPath > size - 1 || finishVertexPath > size - 1 || startVertexPath < 0 || finishVertexPath < 0) { MessageBox.Show("Предупреждение!\nОбратите внимание, что индексация для вершин начинает с 0 и индекс не должен быть больше количества вершин!"); return; }

            List<int> path = this.algorithm.FindPath(startVertexPath, finishVertexPath);

            if (path == null) { this.AppendResultText("Путь не найден!"); return; }

            this.AppendResultText("Путь:\n" + string.Join(" -> ", path.ToArray()));
        }
    }
}
