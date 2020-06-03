using NetworkPetri.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Optimization.View
{
    /// <summary>
    /// Логика взаимодействия для PetriNetwork.xaml
    /// </summary>
    public partial class PetriNetwork : Page
    {
        private NetworkPetri.Model.NetworkPetri _network;

        public PetriNetwork()
        {
            InitializeComponent();
            _network = new NetworkPetri.Model.NetworkPetri();
        }

        private void Refresh()
        {
            lbPositions.Items.Clear();
            lbTransitions.Items.Clear();
            lbEdges.Items.Clear();
            lbResult.Items.Clear();

            foreach (var edge in _network.Edges.Values)
                lbEdges.Items.Add(edge.ToString());
            foreach (var transition in _network.Transitions.Values)
                lbTransitions.Items.Add(transition.ToString());
            foreach (var position in _network.Positions.Values)
                lbPositions.Items.Add(position.ToString());

            ShowAddPanel();
        }

        private void ShowAddPanel()
        {
            var isAddPosition = radioButtonPosition.IsChecked.HasValue && radioButtonPosition.IsChecked.Value;
            var isAddTransition = radioButtonTransition.IsChecked.HasValue && radioButtonTransition.IsChecked.Value;
            var isAddEdge = radioButtonEdge.IsChecked.HasValue && radioButtonEdge.IsChecked.Value;

            panelEdge.Visibility = Visibility.Hidden;
            panelTransition.Visibility = Visibility.Hidden;
            panelPosition.Visibility = Visibility.Hidden;

            if (isAddPosition)
            {
                panelPosition.Visibility = Visibility.Visible;
                labelPositionId.Content = $"ID : {_network.NextIdPositions}";
            }
            else if (isAddTransition)
            {
                panelTransition.Visibility = Visibility.Visible;
                labelTransitionId.Content = $"ID : {_network.NextIdTransitions}";
            }
            else if (isAddEdge)
            {
                panelEdge.Visibility = Visibility.Visible;
            }
        }

        private void AddPosition()
        {
            var marks = int.Parse(textboxPositionCount.Text);
            if (marks < 0)
                marks = 0;
            _network.AddPosition(marks);
            Refresh();
        }

        private void AddTransition()
        {
            _network.AddTransition();
            Refresh();
        }

        private void AddEdge()
        {
            int inputId = int.Parse(textboxInputId.Text);
            int outputId = int.Parse(textboxOutputId.Text);
            int requirement = int.Parse(textBoxEdgeRequirement.Text);
            if (requirement < 1)
                requirement = 1;

            if (radioButtonPositionToTransition.IsChecked.HasValue && radioButtonPositionToTransition.IsChecked.Value)
            {
                if (!_network.Positions.ContainsKey(inputId))
                {
                    MessageBox.Show($"Нет позиции ID = {inputId}");
                    return;
                }
                if (!_network.Transitions.ContainsKey(outputId))
                {
                    MessageBox.Show($"Нет перехода ID = {outputId}");
                    return;
                }

                var input = _network.Positions[inputId];
                var output = _network.Transitions[outputId];

                if (_network.ContainsEdge(input, output))
                {
                    MessageBox.Show($"Уже содержится переход из {input} в {output}");
                    return;
                }

                _network.AddEdge(input, output, requirement);
            }
            else if (radioButtonTransitionToPosition.IsChecked.HasValue && radioButtonTransitionToPosition.IsChecked.Value)
            {
                if (!_network.Transitions.ContainsKey(inputId))
                {
                    MessageBox.Show($"Нет перехода ID = {inputId}");
                    return;
                }
                if (!_network.Positions.ContainsKey(outputId))
                {
                    MessageBox.Show($"Нет позиции ID = {outputId}");
                    return;
                }

                var input = _network.Transitions[inputId];
                var output = _network.Positions[outputId];

                if (_network.ContainsEdge(input, output))
                {
                    MessageBox.Show($"Уже содержится переход из {input} в {output}");
                    return;
                }

                _network.AddEdge(input, output, requirement);
            }

            Refresh();
        }

        private void Delete()
        {
            int id = int.Parse(textboxDeleteId.Text);
            var isPosition = radioButtonPositionDelete.IsChecked.HasValue && radioButtonPositionDelete.IsChecked.Value;
            var isTransition = radioButtonTransitionDelete.IsChecked.HasValue && radioButtonTransitionDelete.IsChecked.Value;
            var isEdge = radioButtonEdgeDelete.IsChecked.HasValue && radioButtonEdgeDelete.IsChecked.Value;

            if (isPosition)
            {
                if (!_network.Positions.ContainsKey(id))
                {
                    MessageBox.Show($"Нет позиции ID = {id}");
                    return;
                }
                _network.RemovePosition(id);
            }
            else if (isTransition)
            {
                if (!_network.Transitions.ContainsKey(id))
                {
                    MessageBox.Show($"Нет перехода ID = {id}");
                    return;
                }
                _network.RemoveTransition(id);
            }
            else if (isEdge)
            {
                if (!_network.Edges.ContainsKey(id))
                {
                    MessageBox.Show($"Нет ребра ID = {id}");
                    return;
                }
                _network.RemoveEdge(id);
            }
            else
                MessageBox.Show($"Выберите тип");

            Refresh();
        }

        private void ShowInfoPosition()
        {
            if (lbPositions.SelectedItem == null)
                return;
            lbInfo.Items.Clear();
            string itemText = lbPositions.SelectedItem.ToString();
            int id = Operations.GetItemId(itemText);
            var position = _network.Positions[id];
            foreach (var edgeId in position.Edges)
                lbInfo.Items.Add(_network.Edges[edgeId].ToString());
        }

        public void ShowInfoTransition()
        {
            if (lbTransitions.SelectedItem == null)
                return;
            lbInfo.Items.Clear();
            string itemText = lbTransitions.SelectedItem.ToString();
            int id = Operations.GetItemId(itemText);
            var transition = _network.Transitions[id];
            foreach (var edgeId in transition.Edges)
                lbInfo.Items.Add(_network.Edges[edgeId].ToString());
        }

        private void radioButtonPosition_Click(object sender, RoutedEventArgs e)
        {
            ShowAddPanel();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void buttonAddPosition_Click(object sender, RoutedEventArgs e)
        {
            AddPosition();
        }

        private void buttonAddTransition_Click(object sender, RoutedEventArgs e)
        {
            AddTransition();
        }

        private void buttonAddEdge_Click(object sender, RoutedEventArgs e)
        {
            AddEdge();
        }

        private void buttonDeleteId_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void lbPositions_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowInfoPosition();
        }

        private void lbTransitions_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowInfoTransition();
        }

        private void btnAllTransitions_Click(object sender, RoutedEventArgs e)
        {
            _network.TryDoAllTransitions(out string msg);
            Refresh();
            lbResult.Items.Add(msg);
        }

        private void btnSingleTransition_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxInputId.Text, out int id))
            {
                _network.TryDoTransition(id, out string msg);
                Refresh();
                lbResult.Items.Add(msg);
            }
        }
    }
}
