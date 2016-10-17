using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArenaNodeDebug : MonoBehaviour {
    public ArenaNode node;
    public SpriteRenderer rend;
    public Text up, down, left, right, middle;


	// Use this for initialization
	void Start ()
    {
        rend.gameObject.SetActive(ConstFile.DEBUG);
	}
	
	public void SetValues(ArenaNode node)
    {
        this.node = node;
        SetupText(middle, node);
        SetupText(up, node.up);
        SetupText(down, node.down);
        SetupText(left, node.left);
        SetupText(right, node.right);
    }

    void SetupText(Text t, ArenaNode node)
    {
        if (node != null)
        {
            t.text = string.Format("({0},{1})", node.x, node.y);
        }
        else
        {
            t.text = "NULL";
        }
    }
}
