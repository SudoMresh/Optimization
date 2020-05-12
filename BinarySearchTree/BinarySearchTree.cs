using System;

namespace BinarySearchTree
{
    /// <summary>
    /// Двоичное дерево поиска.
    /// </summary>
    /// <typeparam name="TKey">Ключ поиска.</typeparam>
    /// <typeparam name="TData">Значение эл-та дерева.</typeparam>
    public class BinarySearchTree<TKey, TData> where TKey : IComparable<TKey>
    {
        private TKey _key;
        private TData _data;

        private BinarySearchTree<TKey, TData> _left;
        private BinarySearchTree<TKey, TData> _rigth;

        /// <summary>
        /// Ключ построения дерева.
        /// </summary>
        public TKey Key => _key;
        /// <summary>
        /// Значение узла.
        /// </summary>
        public TData Value
        {
            get => _data;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Значение не может принимать значение null.");
                }

                _data = value;
            }
        }
        /// <summary>
        /// Дочерний элемент.
        /// </summary>
        public BinarySearchTree<TKey, TData> Left => _left;
        /// <summary>
        /// Дочерний элемент.
        /// </summary>
        public BinarySearchTree<TKey, TData> Right => _rigth;

        public BinarySearchTree(TKey key, TData data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Ключ не может иметь значение null.");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Значение не может принимать значение null.");
            }

            _key = key;
            _data = data;
        }

        public override string ToString() => $"Key: {Key} | Value: {Value}]";

        #region Стандартные операции
        /// <summary>
        /// Поиск эл-та в текущем дереве.
        /// </summary>
        /// <param name="node">Корень дерева.</param>
        /// <param name="key">Ключ поиска.</param>
        /// <param name="result">результат поиска.</param>
        public static bool TryFind(BinarySearchTree<TKey, TData> node, TKey key, out BinarySearchTree<TKey, TData> result)
        {
            result = default;

            if (node == default)
            {
                return false;
            }

            switch (Math.Sign(node.Key.CompareTo(key)))
            {
                case 0: result = node; return true;
                case -1: return TryFind(node.Right, key, out result);
                case 1: return TryFind(node.Left, key, out result);

                default: throw new Exception($"WTF, как тут вообще можно оказаться?");
            }
        }

        /// <summary>
        /// Добавляет новый эл-т в дерево или заменяет существующий.
        /// </summary>
        /// <param name="tree">Корень дерева.</param>
        /// <param name="newNode">Эл-т для добавления.</param>
        public static void Add(ref BinarySearchTree<TKey, TData> tree, BinarySearchTree<TKey, TData> newNode)
        {
            if (tree == default)
            {
                tree = newNode;
            }

            switch (Math.Sign(tree.Key.CompareTo(newNode.Key)))
            {
                case 0: tree.Value = newNode.Value; break;
                case -1: Add(ref tree._rigth, newNode); break;
                case 1: Add(ref tree._left, newNode); break;

                default: throw new Exception($"WTF, как тут вообще можно оказаться?");
            }
        }

        /// <summary>
        /// Находит узел дерева по заданому ключу и удаляет его, если таков есть.
        /// </summary>
        public static void Remove(ref BinarySearchTree<TKey, TData> tree, TKey key)
        {
            if (tree == default)
            {
                return;
            }

            switch (Math.Sign(tree.Key.CompareTo(key)))
            {
                case 0: tree = RemoveNode(ref tree); break;
                case 1: Remove(ref tree._left, key); break;
                case -1: Remove(ref tree._rigth, key); break;
            }
        }

        private static BinarySearchTree<TKey, TData> RemoveNode(ref BinarySearchTree<TKey, TData> tree)
        {
            // если отсутствуют оба дочерних
            if (tree.Left == default && tree.Right == default)
            {
                tree = default;
            }
            // если есть хотя бы один
            else if (tree.Left == default)
            {
                tree = tree.Right;
            }
            else if (tree.Right == default)
            {
                tree = tree.Left;
            }
            // есть оба дочерних эл-та
            else
            {
                if (tree.Right.Left == default)
                {
                    var temp = tree.Left;

                    tree = tree.Right;
                    tree._left = temp;
                }
                else
                {
                    // самый левый узел правого поддерева
                    var left = tree.Right.Left;
                    while (left.Left != default) left = left.Left;

                    tree._key = left.Key;
                    tree.Value = left.Value;

                    // рекурсивно удаляется самый левый правого поддерева
                    Remove(ref tree._rigth, left.Key);
                }
            }

            return tree;
        }
        #endregion
    }
}