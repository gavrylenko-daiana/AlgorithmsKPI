using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RedBlackTreeVisualizer.Enums;
using RedBlackTreeVisualizer.Interfaces;

namespace RedBlackTreeVisualizer.RedBlackTree
{
    public sealed partial class RedBlackTree<T> : ITreeStructure<T>, IEnumerable<(int, T)>
    {
        public RedBlackNode<T>? Root { get; set; }

        public int Size { get; set; }

        public int Depth
        {
            get
            {
                return Root == null ? -1 : Root.Height;
            }
        }

        public int Balance
        {
            get
            {
                return Root == null ? 0 : Root.Balance;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerable<(int, T)> SequentialTraversal
        {
            get
            {
                foreach (RedBlackNode<T> node in InOrderNodeIterator)
                {
                    yield return (node.Key, node.Value);
                }
            }
        }

        public RedBlackTree()
        {
            Root = null;
            Size = 0;
        }

        public void Add(int key, T value)
        {
            Root = Add(Root, new RedBlackNode<T>(key, value));

            if (Root.IsRed)
            {
                Root.SetColour(NodeColor.Black);
            }
        }

        public bool Remove(int key)
        {
            var result = true;

            try
            {
                if (Root == null)
                {
                    result = false;
                }
                else
                {
                    var done = false;
                    Root = Remove(Root, key, ref done);

                    Root?.SetColour(NodeColor.Black);
                }
            }
            catch (InvalidOperationException)
            {
                result = false;
            }

            return result;
        }

        public (IEnumerable<RedBlackNode<T>>, int) Search(int key)
        {
            List<RedBlackNode<T>> nodes = new();
            RedBlackNode<T>? current = Root;
            var count = 0;

            while (current != null)
            {
                count += 1;

                if (key == current.Key)
                {
                    nodes.Add(current);
                }

                if (key <= current.Key)
                {
                    current = current.Left;
                }
                else if (key > current.Key)
                {
                    current = current.Right;
                }
            }


            return (nodes, count);
        }

        public bool HasId(int key)
        {
            RedBlackNode<T>? current = this.Root;

            while (current != null && key != current.Key)
            {
                if (key < current.Key)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return current != null;
        }

        public override string ToString()
        {
            StringBuilder nodes = new();

            foreach((var key, _) in SequentialTraversal)
            {
                nodes.Append(key + " = ");
            }

            nodes.Remove(nodes.Length - 4, 4);

            return nodes.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ITreeStructure<T>)this).SequentialTraversal.GetEnumerator();
        }

        public IEnumerator<(int, T)> GetEnumerator()
        {
            return ((ITreeStructure<T>)this).SequentialTraversal.GetEnumerator();
        }

        private RedBlackNode<T>? Remove(RedBlackNode<T>? root, int key, ref bool done)
        {
            var compareResult = root!.Key.CompareTo(key);

            if (compareResult == 0)
            {
                if (root.Left != null && root.Right != null)
                {
                    compareResult = 1;
                    root.Key = root.InOrderPredecessor!.Key;
                    key = root.Key;
                    root.ResetHeight();
                }
                else if (root.Left != null)
                {
                    Size -= 1;
                    root.Left.ResetHeight();

                    if (IsNodeRed(root.Left))
                    {
                        root.Left.SetColour(NodeColor.Black);
                        done = true;
                    }

                    root = root.Left;
                }
                else if (root.Right != null)
                {
                    Size -= 1;
                    root.Right.ResetHeight();

                    if (IsNodeRed(root.Right))
                    {
                        root.Right.SetColour(NodeColor.Black);
                        done = true;
                    }

                    root = root.Right;
                }
                else
                {
                    Size -= 1;
                    done = IsNodeRed(root);
                    root = null;
                }
            }

            if (compareResult > 0)
            {
                if (root!.Left != null)
                {
                    root.Left = Remove(root.Left, key, ref done);

                    if (!done)
                    {
                        root = RemoveRebalanceLeft(root, ref done);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (compareResult < 0)
            {
                if (root!.Right != null)
                {
                    root.Right = Remove(root.Right, key, ref done);

                    if (!done)
                    {
                        root = RemoveRebalanceRight(root, ref done);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return root;
        }

        private static RedBlackNode<T> RemoveRebalanceRight(RedBlackNode<T> root, ref bool done)
        {
            RedBlackNode<T> parent = root;
            RedBlackNode<T>? sibling = root.Left;

            if (IsNodeRed(sibling))
            {
                root = root.RotateRight();
                sibling = parent.Left;
            }

            if (sibling != null)
            {
                if (!IsNodeRed(sibling.Left) && !IsNodeRed(sibling.Right))
                {
                    if (IsNodeRed(parent))
                    {
                        done = true;
                    }

                    parent.SetColour(NodeColor.Black);
                    sibling.SetColour(NodeColor.Red);
                }
                else
                {
                    var parentIsRed = parent.IsRed;
                    var sameRoot = root == parent;

                    if (IsNodeRed(sibling.Left))
                    {
                        parent = parent.RotateRight();
                    }
                    else
                    {
                        parent.Left = parent.Left!.RotateLeft();
                        parent = parent.RotateRight();
                    }

                    parent.SetIsRed(parentIsRed);
                    parent.Left!.SetColour(NodeColor.Black);
                    parent.Right!.SetColour(NodeColor.Black);

                    if (sameRoot)
                    {
                        root = parent;
                    }
                    else
                    {
                        root.Right = parent;
                    }

                    done = true;
                }
            }

            return root;
        }

        private static RedBlackNode<T> RemoveRebalanceLeft(RedBlackNode<T> root, ref bool done)
        {
            RedBlackNode<T> parent = root;
            RedBlackNode<T>? sibling = root.Right;

            if (IsNodeRed(sibling))
            {
                root = root.RotateLeft();
                sibling = parent.Right;
            }

            if (sibling != null)
            {
                if (!IsNodeRed(sibling.Left) && !IsNodeRed(sibling.Right))
                {
                    if (IsNodeRed(parent))
                    {
                        done = true;
                    }

                    parent.SetColour(NodeColor.Black);
                    sibling.SetColour(NodeColor.Red);
                }
                else
                {
                    var parentIsRed = parent.IsRed;
                    var sameRoot = root == parent;

                    if (IsNodeRed(sibling.Right))
                    {
                        parent = parent.RotateLeft();
                    }
                    else
                    {
                        parent.Right = parent.Right!.RotateRight();
                        parent = parent.RotateLeft();
                    }

                    parent.SetIsRed(parentIsRed);
                    parent.Left!.SetColour(NodeColor.Black);
                    parent.Right!.SetColour(NodeColor.Black);

                    if (sameRoot)
                    {
                        root = parent;
                    }
                    else
                    {
                        root.Left = parent;
                    }

                    done = true;
                }
            }

            return root;
        }


        private RedBlackNode<T> Add(RedBlackNode<T>? root, RedBlackNode<T> node)
        {
            if (root == null)
            {
                root = node;
                Size += 1;
            }
            else
            {
                root.ResetHeight();
                var compareResult = root.Key.CompareTo(node.Key);

                if (compareResult > 0)
                {
                    root.Left = Add(root.Left, node);
                }
                else if (compareResult < 0)
                {
                    root.Right = Add(root.Right, node);
                }
                else
                {
                    throw new InvalidOperationException("Duplicates are not possible in red and black tree");
                }
            }

            root = root.Key.CompareTo(node.Key) > 0 ? Add_Case1_LeftTwoRedChidren(root) : Add_Case1_RightTwoRedChidren(root);

            return root;
        }

        private static RedBlackNode<T> Add_Case1_LeftTwoRedChidren(RedBlackNode<T> root)
        {
            if (IsNodeRed(root.Left) && (IsNodeRed(root.Left!.Left) || IsNodeRed(root.Left!.Right)))
            {
                if (IsNodeRed(root.Right))
                {
                    MoveBlackDown(root);
                }
                else
                {
                    root = Add_Case2_TwoLeftReds(root);
                }
            }

            return root;
        }

        private static RedBlackNode<T> Add_Case1_RightTwoRedChidren(RedBlackNode<T> root)
        {
            if (IsNodeRed(root.Right) && (IsNodeRed(root.Right!.Right) || IsNodeRed(root.Right!.Left)))
            {
                if (IsNodeRed(root.Left))
                {
                    MoveBlackDown(root);
                }
                else
                {
                    root = Add_Case2_TwoRightReds(root);
                }
            }

            return root;
        }

        private static RedBlackNode<T> Add_Case2_TwoLeftReds(RedBlackNode<T> root)
        {
            if (IsNodeRed(root.Left!.Left))
            {
                root = root.RotateRight();
            }
            else if (IsNodeRed(root.Left.Right))
            {
                root = Add_Case3_LeftRightReds(root);
            }

            return root;
        }

        private static RedBlackNode<T> Add_Case2_TwoRightReds(RedBlackNode<T> root)
        {
            if (IsNodeRed(root.Right!.Right))
            {
                root = root.RotateLeft();
            }
            else if (IsNodeRed(root.Right.Left))
            {
                root = Add_Case3_RightLeftReds(root);
            }

            return root;
        }

        private static RedBlackNode<T> Add_Case3_RightLeftReds(RedBlackNode<T> root)
        {
            root.Right = root.Right!.RotateRight();
            root = root.RotateLeft();

            return root;
        }

        private static RedBlackNode<T> Add_Case3_LeftRightReds(RedBlackNode<T> root)
        {
            root.Left = root.Left!.RotateLeft();
            root = root.RotateRight();

            return root;
        }

        
        private IEnumerable<RedBlackNode<T>> InOrderNodeIterator
        {
            get
            {
                RedBlackNode<T>? current = Root;
                Stack<RedBlackNode<T>> parentStack = new();

                while (current != null || parentStack.Count != 0)
                {
                    if (current != null)
                    {
                        parentStack.Push(current);
                        current = current.Left;
                    }
                    else
                    {
                        current = parentStack.Pop();
                        yield return current;
                        current = current.Right;
                    }
                }
            }
        }
        
        private static bool IsNodeRed(RedBlackNode<T>? node)
        {
            return (node != null && node.IsRed);
        }

        private static void MoveBlackDown(RedBlackNode<T> root)
        {
            root.SetColour(NodeColor.Red);
            root.Left!.SetColour(NodeColor.Black);
            root.Right!.SetColour(NodeColor.Black);
        }
    }
}
