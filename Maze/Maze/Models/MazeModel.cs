namespace Maze.Models;

public class MazeModel
{
    public int[,] Layout { get; set; }
    public (int, int) Start { get; set; }
    public (int, int) End { get; set; }
}