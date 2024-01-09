using RedBlackTreeVisualizer.Enums;

namespace RedBlackTreeVisualizer.RedBlackTree
{
    public sealed class RedBlackNode<T>
    {
        public bool IsRed => _isRed;
        public bool IsLeaf => Left == null && Right == null;
        public int Balance => GetNodeBalance();

        private bool _isRed;
        private int _height;

        public int Key { get; set; }
        public T Value { get; set; }

        public RedBlackNode<T>? Left { get; set; }
        public RedBlackNode<T>? Right { get; set; }

        public int Height
        {
            get
            {
                if (_height == int.MinValue)
                {
                    _height = GetNodeHeight();
                }

                return _height;
            }
        }

        public RedBlackNode<T>? InOrderPredecessor
        {
            get
            {
                RedBlackNode<T>? previous = null;
                RedBlackNode<T>? current = Left;

                while (current != null)
                {
                    previous = current;
                    current = current.Right;
                }

                return previous;
            }
        }
        
        public RedBlackNode(int key, T value)
        {
            Left = null;
            Right = null;
            _height = int.MinValue;
            Key = key;
            Value = value;
            _isRed = true;
        }

        public RedBlackNode(int key, T value, NodeColor colour)
        {
            Left = null;
            Right = null;
            _height = int.MinValue;
            Key = key;
            Value = value;
            SetColour(colour);
        }

        public RedBlackNode<T> RotateLeft()
        {
            RedBlackNode<T> pivot = Right!;

            Right = pivot.Left;
            pivot.Left = this;

            pivot.ResetHeight();
            ResetHeight();

            pivot.Left.SetColour(NodeColor.Red);
            pivot.SetColour(NodeColor.Black);

            return pivot;
        }

        public RedBlackNode<T> RotateRight()
        {
            RedBlackNode<T> pivot = Left!;

            Left = pivot.Right;
            pivot.Right = this;

            pivot.ResetHeight();
            ResetHeight();

            pivot.Right.SetColour(NodeColor.Red);
            pivot.SetColour(NodeColor.Black);

            return pivot;
        }

        public void SetIsRed(bool isRed)
        {
            _isRed = isRed;
        }

        public void SetColour(NodeColor colour)
        {
            _isRed = colour == NodeColor.Red;
        }

        public void ResetHeight()
        {
            _height = int.MinValue;
        }

        private static int GetChildNodeHeight(RedBlackNode<T>? node)
        {
            return node == null ? -1 : node.Height;
        }

        private int GetNodeBalance()
        {
            return GetChildNodeHeight(Left) - GetChildNodeHeight(Right);
        }

        private int GetNodeHeight()
        {
            return System.Math.Max(GetChildNodeHeight(Left), GetChildNodeHeight(Right)) + 1;
        }
    }
}
