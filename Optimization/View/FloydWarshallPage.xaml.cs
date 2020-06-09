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

            int size;
            
            if (!Int32.TryParse(CountPoint.Text.Trim(), out size))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");

                return;
            }

            if (size <= 1) { MessageBox.Show("Количество вершин графа должно быть больше 1!"); return; }

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
                MessageBox.Show("Введите начальную вершину!"); 
                
                return;
            }

            int startVertex;
            
            if (!Int32.TryParse(StartVertex.Text.Trim(), out startVertex))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");

                return;
            }

            if (startVertex < 0) { MessageBox.Show("Вводимая начальная вершина должна быть больше, либо равно 0!"); return; }

            if (FinishVertex.Text == "")
            {
                MessageBox.Show("Введите конечную вершину!"); return;
            }

            int finishVertex;

            if (!Int32.TryParse(FinishVertex.Text.Trim(), out finishVertex))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");
                return;
            };

            if (finishVertex < 0) { MessageBox.Show("Вводимая конечная вершина должна быть больше, либо равно 0!"); return; }

            if (startVertex == finishVertex) { MessageBox.Show("Вершины должны быть разные."); return; }

            if (startVertex > size - 1 || finishVertex > size - 1) { MessageBox.Show("Предупреждение!\nОбратите внимание, что индексация для вершин начинает с 0 для записи их в матрицу расстояний и индекс не должен быть больше количества вершин!"); return; }

            if (WeightEdge.Text == "")
            {
                MessageBox.Show("Введите вес для заданой вершины!"); return;
            }

            int edgeWeight;

            if (!Int32.TryParse(WeightEdge.Text.Trim(), out edgeWeight))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");
                return;
            };

            if (edgeWeight < 0) { MessageBox.Show("Вводимый вес должен быть больше, либо равен 0!"); return; }

            this.algorithm.AddVertex(startVertex, finishVertex, edgeWeight);

            this.RefreshResultTextWithMatrix();
        }

        private void HandleSolveClick(object sender, RoutedEventArgs e)
        {
            if (!this.algorithm.IsCreated()) { MessageBox.Show("Матрица расстояний не создана!\nПопробуйте ввести количество вершин и нажать кнопку 'Создать матрицу расстояний'."); return; }
            if (!this.algorithm.IsHaveVertex()) { MessageBox.Show("Прежде чем найти найкратчайшие пути, заполните, пожалуйста, матрицу расстояний!\nДля этого нужно добавить хотя бы 1 вершиину!"); return; }

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

            int startVertexPath;

            if (!Int32.TryParse(AddStartSolveVertex.Text.Trim(), out startVertexPath))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");
                return;
            };

            if (startVertexPath < 0) { MessageBox.Show("Вводимая конечная вершина должна быть больше, либо равно 0!"); return; }

            if (AddFinishSolveVertex.Text == "")
            {
                MessageBox.Show("Введите конечную вершину для поиска пути!"); return;
            }

            int finishVertexPath;

            if (!Int32.TryParse(AddFinishSolveVertex.Text.Trim(), out finishVertexPath))
            {
                MessageBox.Show("Вы ввели буквенное или символьнное обозначение!\nПрограмма принимает на ввод только положительные числа.");
                return;            
            };

            if (finishVertexPath < 0) { MessageBox.Show("Вводимая конечная вершина должна быть больше, либо равно 0!"); return; }

            if (AddStartSolveVertex.Text == "" && AddFinishSolveVertex.Text == "")
            {
                MessageBox.Show("Введите начальную и конечную вершины для поиска наикратчайшего пути!"); return;
            }
            int size = this.algorithm.GetMatrixSize();

            if (startVertexPath == finishVertexPath) { this.AppendResultText("Путь для этой пары вершин не найден!"); return; }
            if (startVertexPath > size - 1 || finishVertexPath > size - 1 || startVertexPath < 0 || finishVertexPath < 0) { MessageBox.Show("Предупреждение!\nОбратите внимание, что индексация для вершин начинает с 0 и индекс не должен быть больше количества вершин!"); return; }

            List<int> path = this.algorithm.FindPath(startVertexPath, finishVertexPath);

            if (path == null) { this.AppendResultText("Путь для этой пары вершин не найден!"); return; }

            this.AppendResultText("Путь:\n" + string.Join(" -> ", path.ToArray()));
        }
    }
}
