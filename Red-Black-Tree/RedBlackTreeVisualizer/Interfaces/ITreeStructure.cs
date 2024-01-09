using System.Collections.Generic;
using RedBlackTreeVisualizer.RedBlackTree;

namespace RedBlackTreeVisualizer.Interfaces;

public interface ITreeStructure<T>
{
    int Size { get; }

    int Depth { get; }

    int Balance { get; }

    void Add(int id, T item);

    bool HasId(int id);

    bool Remove(int id);

    (IEnumerable<RedBlackNode<T>>, int) Search(int key);

    IEnumerable<(int, T)> SequentialTraversal { get; }
}
