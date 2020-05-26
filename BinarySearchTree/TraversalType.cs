namespace BinarySearchTree
{
    /// <summary>
    /// Определяет типо обхода дерева <see cref="BinarySearchTree{TKey, TData}"/>.
    /// </summary>
    public enum TraversalType
    {
        /// <summary>
        /// Префиксный обход.
        /// </summary>
        PreOrderTraversal,
        /// <summary>
        /// Постфиксный обход.
        /// </summary>
        PostOrderTraversal,
        /// <summary>
        /// Инфиксный обход.
        /// </summary>
        InOrderTravers
    }
}
