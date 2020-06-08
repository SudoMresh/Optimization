using System;

namespace DijkstraModule
{
    public class NotFoundEdgeExeption : Exception
    {
        public string NotFoundFirstEdge { get; set; }
        public string NotFoundSecondEdge { get; set; }

        public string ShowError()
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(NotFoundFirstEdge))
            {
                result += $"Вершина {NotFoundFirstEdge} не найдена.\n";
            }

            if (!string.IsNullOrEmpty(NotFoundSecondEdge))
            {
                result += $"Вершина {NotFoundSecondEdge} не найдена.\n";
            }

            return result;
        }
    }
}
