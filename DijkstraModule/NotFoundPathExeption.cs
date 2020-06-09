using System;
using System.Collections.Generic;
using System.Text;

namespace DijkstraModule
{
    public class NotFoundPathExeption : Exception
    {
        public string StartVertex { get; set; }
        public string EndVertex { get; set; }

        public string ShowError()
        {
            return $"Проверте пусть с вершины {StartVertex} в вершину {EndVertex}.";
        }
    }
}
