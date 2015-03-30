using UnityEngine;

public class Verification
{

    public enum VerificationResult
    {
        Ok,
        InvalidStart,
        InvalidEnd,
        Incomplete
    }

    Maze maze;

    public Verification(Maze maze)
    {
        this.maze = maze;
    }
	
    public VerificationResult VerifyMaze(Maze mazeToCheck)
    {
        var mazeData = (bool[,])mazeToCheck.mapData.Clone();

        if (mazeToCheck.start.x < 0 || mazeToCheck.start.y < 0)
        {
            return VerificationResult.InvalidStart;
        }
        if (mazeData [(int)mazeToCheck.start.x, (int)mazeToCheck.start.y])
        {
            return VerificationResult.InvalidStart;
        }
        if (mazeToCheck.end.x < 0 || mazeToCheck.end.y < 0)
        {
            return VerificationResult.InvalidEnd;
        }
        if (mazeData [(int)mazeToCheck.end.x, (int)mazeToCheck.end.y])
        {
            return VerificationResult.InvalidEnd;
        }
        if (Travel(mazeData, mazeToCheck.start))
        {
            return VerificationResult.Ok;
        }
        return VerificationResult.Incomplete;
    }

    // Fonction récursive appelée pour chaque case
    // Algorithme de Dijkstra
    bool Travel(bool[,] mazeData, Vector2 coor)
    {
        if (coor.Equals(maze.end))
        {
            return true;
        }

        mazeData [(int)coor.x, (int)coor.y] = true;
        if (coor.x > 0 && !mazeData [(int)coor.x - 1, (int)coor.y])
        {
            if (Travel(mazeData, new Vector2((int)coor.x - 1, coor.y)))
            {
                return true;
            }
        }
        if (coor.x < maze.sizeX - 1 && !mazeData [(int)coor.x + 1, (int)coor.y])
        {
            if (Travel(mazeData, new Vector2((int)coor.x + 1, coor.y)))
            {
                return true;
            }
        }
        if (coor.y > 0 && !mazeData [(int)coor.x, (int)coor.y - 1])
        {
            if (Travel(mazeData, new Vector2(coor.x, (int)coor.y - 1)))
            {
                return true;
            }
        }
        if (coor.y < maze.sizeZ - 1 && !mazeData [(int)coor.x, (int)coor.y + 1])
        {
            if (Travel(mazeData, new Vector2(coor.x, (int)coor.y + 1)))
            {
                return true;
            }
        }
        return false;		
    }
}
