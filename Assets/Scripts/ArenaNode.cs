using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ArenaNode
{
    public Vector2 worldPosition;
    public int x, y;
    public ArenaNode up, left, right, down;
    public Puppet onNode;

    public ArenaNode(int newX, int newY, Vector2 worldPos)
    {
        x = newX;
        y = newY;
        worldPosition = worldPos;
    }
    
    public bool AddPuppet(Puppet p)
    {
        if (onNode != null)
        {
            return false;
        }
        else
        {
            onNode = p;
            p.currNode = this;
            p.gridLocation = new Vector2(x, y);
            p.transform.position = worldPosition;
            return true;
        }
    }
    

    public void RemovePuppet()
    {
        onNode = null;
    }
}
