using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArenaNode
{
    public Vector2 worldPosition;
    public int x, y;
    public ArenaNode up, left, right, down;

    public ArenaNode(int newX, int newY, Vector2 worldPos)
    {
        x = newX;
        y = newY;
        worldPosition = worldPos;
    }
}
