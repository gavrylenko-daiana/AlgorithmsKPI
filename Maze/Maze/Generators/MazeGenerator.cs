namespace Maze.Generators;

using System;
using Maze.Models;

public class MazeGenerator
{
    public MazeModel GenerateMaze()
    {
        var maze = new int[,]
        {
            {0, 0, 0},
            {0, 1, 1},
            {0, 0, 0}
        };

        var start = (0, 0);
        var end = (2, 2);

        return new MazeModel
        {
            Layout = maze,
            Start = start,
            End = end
        };
    }
}
