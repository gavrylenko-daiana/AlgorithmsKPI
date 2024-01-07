using Maze.Models;

namespace Maze.Helpers;

public static class MazeHelper
{
    public static IEnumerable<(int, int)> GetChildren(MazeModel maze, (int, int) node)
    {
        (int row, int col) = node;

        var directions = new List<(int, int)>
        {
            (row - 1, col),
            (row + 1, col),
            (row, col - 1),
            (row, col + 1)
        };

        foreach (var (newRow, newCol) in directions)
        {
            if (newRow >= 0 && newRow < maze.Layout.GetLength(0) &&
                newCol >= 0 && newCol < maze.Layout.GetLength(1))
            {
                if (maze.Layout[newRow, newCol] == 0)
                {
                    yield return (newRow, newCol);
                }
            }
        }
    }
}