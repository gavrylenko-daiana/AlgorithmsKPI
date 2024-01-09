using RedBlackTreeVisualizer.Enums;
using RedBlackTreeVisualizer.Models;
using RedBlackTreeVisualizer.RedBlackTree;

namespace RedBlackTreeVisualizer.Tests.UnitTesting.xUnit
{
    public class RedBlackTreeTests
    {
        [Fact]
        public void RedBlackNode_Constructors_SetPropertiesCorrectly()
        {
            // Arrange
            var node = new RedBlackNode<int>(42, 100);

            // Act & Assert
            Assert.Equal(42, node.Key);
            Assert.Equal(100, node.Value);
            Assert.True(node.IsRed);

            var blackNode = new RedBlackNode<int>(42, 100, NodeColor.Black);
            Assert.False(blackNode.IsRed);
        }

        [Fact]
        public void RedBlackTree_HasId_ReturnsTrueForExistingKey()
        {
            // Arrange
            var tree = new RedBlackTree<int>();
            tree.Add(50, 500);
            tree.Add(25, 250);
            tree.Add(75, 750);

            // Act & Assert
            Assert.True(tree.HasId(25));
        }

        [Fact]
        public void RedBlackTree_HasId_ReturnsFalseForNonExistingKey()
        {
            // Arrange
            var tree = new RedBlackTree<int>();
            tree.Add(50, 500);
            tree.Add(25, 250);
            tree.Add(75, 750);

            // Act & Assert
            Assert.False(tree.HasId(100));
        }

        [Fact]
        public void RedBlackTree_SeqentialTraversal_ReturnsCorrectSequence()
        {
            // Arrange
            var tree = new RedBlackTree<int>();
            tree.Add(50, 500);
            tree.Add(25, 250);
            tree.Add(75, 750);
            tree.Add(10, 100);
            tree.Add(30, 300);
            tree.Add(60, 600);
            tree.Add(80, 800);

            // Act
            var sequence = tree.SequentialTraversal.ToList();

            // Assert
            Assert.Equal(7, sequence.Count);
            Assert.Equal((10, 100), sequence[0]);
            Assert.Equal((25, 250), sequence[1]);
            Assert.Equal((30, 300), sequence[2]);
            Assert.Equal((50, 500), sequence[3]);
            Assert.Equal((60, 600), sequence[4]);
            Assert.Equal((75, 750), sequence[5]);
            Assert.Equal((80, 800), sequence[6]);
        }
    }
}
