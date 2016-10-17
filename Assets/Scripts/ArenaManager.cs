using UnityEngine;
using System.Collections.Generic;

public class ArenaManager : UnitySingleton<ArenaManager>
{
	public Transform arenaTop;
	public Transform arenaBottom;

    public static float screenWidth, arenaHeight;
    
    

    public static Vector3 leftSide;
    Vector3 rightSide;
    public static Vector3 midPoint;

    List<GameObject> lines = new List<GameObject>();
    
	public int nodesWide;
	public int nodesHigh;

    public static float nodeWidth, nodeHeight;

    public ArenaNode[,] nodeGrid;

    [Header("Prefabs")]
    public GameObject linePrefab;
    public GameObject nodePrefab;
    public GameObject gameObjectPrefab;

    [Header("Parents")]
    public Transform nodeGridParent;
    public Transform vertLineParent;
    public Transform horizLineParent;

    // Use this for initialization
    void Start () {
        GenerateGrid();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public Vector2 GridToWorld(Vector2 loc)
    {
        return GridToWorld((int)loc.x, (int)loc.y);
    }

    public Vector2 GridToWorld(int x, int y)
    {
        return nodeGrid[x, y].worldPosition;
    }


    /// <summary>
    /// Given the beat, return all the units that are there.
    /// </summary>
    /// <param name="beat"></param>
    /// <returns></returns>
	public List<Puppet> GetUnitsFromBeat(int beat)
	{
		List<Puppet> ret = new List<Puppet>();
		int rowsPerBeat = (nodesWide+1)/16;
        // Collect puppets for left team
        for (int x = 0 + rowsPerBeat*beat; x < rowsPerBeat * (beat+1); x++)
        {
            for (int y = 0; y < nodesHigh; y++)
            {
                if (nodeGrid[x,y].onNode != null && nodeGrid[x, y].onNode.team == ConstFile.Team.LEFT)
                {
                    ret.Add(nodeGrid[x, y].onNode);
                }
            }
        }
        // Collect puppets for right team
        for (int x = nodesWide; x > nodesWide - (rowsPerBeat * (beat + 1)); x--)
        {
            for (int y = 0; y < nodesHigh; y++)
            {
                if (nodeGrid[x, y].onNode != null && nodeGrid[x, y].onNode.team == ConstFile.Team.RIGHT)
                {
                    ret.Add(nodeGrid[x, y].onNode);
                }
            }
        }
        return ret;
	}
    

    


    #region Grid Creation

    [BitStrap.Button]
    public void GenerateGrid()
	{
        FindArenaDimensions();
        ClearLines();
        GenerateLines();
        CreateNodeGrid();
	}

    void GenerateLines()
    {
        nodeGrid = new ArenaNode[nodesWide + 1, nodesHigh + 1];
        nodeWidth = screenWidth / (nodesWide);
        nodeHeight = arenaHeight / nodesHigh;
        for (int i = 0; i < nodesWide+1; i++)
        {
            GameObject line = Instantiate(linePrefab);
            line.transform.position = new Vector3(leftSide.x, midPoint.y, 0);
            line.transform.position += new Vector3(nodeWidth*i,0,0);
            line.transform.localScale = new Vector3(line.transform.localScale.x, arenaHeight*2, line.transform.localScale.z);
            line.transform.parent = vertLineParent;
            lines.Add(line);
        }
        for (int i = 0; i < nodesHigh+1; i++)
        {
            GameObject line = Instantiate(linePrefab);
            line.transform.position = new Vector3(midPoint.x, arenaTop.position.y, 0);
            line.transform.position -= new Vector3(0, nodeHeight*i, 0);
            line.transform.localScale = new Vector3(screenWidth*2, line.transform.localScale.y, line.transform.localScale.z);
            line.transform.parent = horizLineParent;
            lines.Add(line);
        }
    }

    void CreateNodeGrid()
    {
        for (int x = 0; x < nodesWide + 1; x++)
        {
            for (int y = 0; y < nodesHigh + 1; y++)
            {
                float worldX = leftSide.x + (nodeWidth * x);
                float worldY = arenaTop.position.y - (nodeHeight * y);
                Vector2 worldPos = new Vector2(worldX, worldY);
                nodeGrid[x, y] = new ArenaNode(x, y, worldPos);
            }
        }

        for (int x = 0; x < nodesWide + 1; x++)
        {
            GameObject xParent = Instantiate(gameObjectPrefab);
            xParent.name = string.Format("X-{0}", x);
            xParent.transform.parent = nodeGridParent;

            for (int y = 0; y < nodesHigh + 1; y++)
            {
                if (x != 0)
                {
                    nodeGrid[x, y].left = nodeGrid[x - 1, y];
                }
                if (x != nodesWide)
                {
                    nodeGrid[x, y].right = nodeGrid[x + 1, y];
                }
                if (y != 0)
                {
                    nodeGrid[x, y].up = nodeGrid[x, y - 1];
                }
                if (y != nodesHigh)
                {
                    nodeGrid[x, y].down = nodeGrid[x, y + 1];
                }

                GameObject nodeRep = Instantiate(nodePrefab);
                nodeRep.transform.position = nodeGrid[x, y].worldPosition;
                nodeRep.GetComponent<ArenaNodeDebug>().SetValues(nodeGrid[x, y]);
                nodeRep.transform.parent = xParent.transform;
            }
        }
    }

	[BitStrap.Button]
    public void ClearLines()
    {
        foreach (GameObject line in lines)
        {
            DestroyImmediate(line);
        }
        lines.Clear();
		
		while (vertLineParent.childCount != 0)
		{
			DestroyImmediate(vertLineParent.GetChild(0).gameObject);
		}
		while (horizLineParent.childCount != 0)
		{
			DestroyImmediate(horizLineParent.GetChild(0).gameObject);
		}
        while (nodeGridParent.childCount != 0)
        {
            DestroyImmediate(nodeGridParent.GetChild(0).gameObject);
        }
	}

    public void FindArenaDimensions()
    {
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, 0));
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1, .5f, 0));
        screenWidth = Vector3.Distance(leftSide, rightSide);

        arenaHeight = arenaTop.position.y - arenaBottom.position.y;

        midPoint = new Vector3(leftSide.x + screenWidth/2, arenaBottom.position.y + arenaHeight/2, 0);
    }
    #endregion
}
