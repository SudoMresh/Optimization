using System;
using System.Collections.Generic;

namespace BinarySearchTree
{
    public static class BinaryTreeExtensions
    {
        /// <summary>
        /// Последовательный вызов <see cref="BinarySearchTree{TKey, TData}.Add(ref BinarySearchTree{TKey, TData}, BinarySearchTree{TKey, TData})"/>.
        /// </summary>
        public static void AddRange<TKey, TData>(this BinarySearchTree<TKey, TData> tree, IDictionary<TKey, TData> data) where TKey : IComparable<TKey>
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), $"Входной параметр имеет значение null.");
            }

            foreach (var pair in data)
            {
                var node = new BinarySearchTree<TKey, TData>(pair.Key, pair.Value);
                BinarySearchTree<TKey, TData>.Add(ref tree, node);
            }
        }

        /// <summary>
        /// Проброс метода <see cref="BinarySearchTree{TKey, TData}.Add(ref BinarySearchTree{TKey, TData}, BinarySearchTree{TKey, TData})"/>.
        /// </summary>
        public static void Add<TKey, TData>(this BinarySearchTree<TKey, TData> tree, BinarySearchTree<TKey, TData> node) where TKey : IComparable<TKey> => BinarySearchTree<TKey, TData>.Add(ref tree, node);
        /// <summary>
        /// Проброс метода <see cref="BinarySearchTree{TKey, TData}.Add(ref BinarySearchTree{TKey, TData}, BinarySearchTree{TKey, TData})"/>.
        /// </summary>
        public static void Add<TKey, TData>(this BinarySearchTree<TKey, TData> tree, TKey key, TData data) where TKey : IComparable<TKey> => BinarySearchTree<TKey, TData>.Add(ref tree, new BinarySearchTree<TKey, TData>(key, data));
        /// <summary>
        /// Проброс метода <see cref="BinarySearchTree{TKey, TData}.Remove(ref BinarySearchTree{TKey, TData}, TKey)"/>.
        /// </summary>
        public static void Remove<TKey, TData>(this BinarySearchTree<TKey, TData> tree, TKey key) where TKey : IComparable<TKey> => BinarySearchTree<TKey, TData>.Remove(ref tree, key);
        /// <summary>
        /// Проброс метода <see cref="BinarySearchTree{TKey, TData}.TryFind(BinarySearchTree{TKey, TData}, TKey, out BinarySearchTree{TKey, TData})"/>.
        /// </summary>
        public static bool TryFind<TKey, TData>(this BinarySearchTree<TKey, TData> tree, TKey key, out BinarySearchTree<TKey, TData> node) where TKey : IComparable<TKey>
        {
            node = default;

            if (BinarySearchTree<TKey, TData>.TryFind(tree, key, out BinarySearchTree<TKey, TData> result))
            {
                node = result;
                return true;
            }

            return false;
        }

        public static List<BinarySearchTree<TKey, TData>> ToList<TKey, TData>(this BinarySearchTree<TKey, TData> tree, TraversalType type = TraversalType.PreOrderTraversal) where TKey : IComparable<TKey>
        {
            var result = new List<BinarySearchTree<TKey, TData>>();

            switch (type)
            {
                case TraversalType.PreOrderTraversal: ToListByPreOrderTraversal(tree, result); break;
                case TraversalType.PostOrderTraversal: ToListByPostOrderTraversal(tree, result); break;
                case TraversalType.InOrderTravers: ToListByInOrderTraversal(tree, result); break;
            }

            return result;
        }

        public static void ToListByPreOrderTraversal<TKey, TData>(BinarySearchTree<TKey, TData> tree, List<BinarySearchTree<TKey, TData>> outItemsList) where TKey : IComparable<TKey>
        {
            if (tree == default) return;

            outItemsList.Add(tree);

            ToListByPreOrderTraversal(tree.Left, outItemsList);
            ToListByPreOrderTraversal(tree.Right, outItemsList);
        }

        public static void ToListByPostOrderTraversal<TKey, TData>(BinarySearchTree<TKey, TData> tree, List<BinarySearchTree<TKey, TData>> outItemsList) where TKey : IComparable<TKey>
        {
            if (tree == default) return;

            ToListByPostOrderTraversal(tree.Left, outItemsList);
            ToListByPostOrderTraversal(tree.Right, outItemsList);

            outItemsList.Add(tree);
        }

        public static void ToListByInOrderTraversal<TKey, TData>(BinarySearchTree<TKey, TData> tree, List<BinarySearchTree<TKey, TData>> outItemsList) where TKey : IComparable<TKey>
        {
            if (tree == default) return;

            ToListByInOrderTraversal(tree.Left, outItemsList);
            outItemsList.Add(tree);
            ToListByInOrderTraversal(tree.Right, outItemsList);
        }
    }
}
