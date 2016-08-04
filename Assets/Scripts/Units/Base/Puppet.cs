using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PuppetType = ConstFile.PuppetType;
using Team = ConstFile.Team;


[RequireComponent(typeof(Collider2D))]
public abstract class Puppet : Mortal
{
    public PuppetType currType;
    protected List<Transform> enemyList = new List<Transform>();
    public ConstFile.Actions currAction;
    //public ConstFile.Notes currNote;
    public static int idCounter;
    public int id;
    public Vector2 gridLocation;

    public GameObject whole;
    public GameObject half;
    public GameObject quarter;
    public GameObject eigth;
    public GameObject sixteenth;

    public ArenaNode currNode;

    public Instructions instructions;

    protected virtual void Start()
    {
        DeathMethod();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        DeathMethod();
    }

    #region House Keeping 
    /*
	Shouldn't need to touch these much, mostly automated by events.
	*/

    protected void EnemyAdd(Team puppetTeam, Transform enemy, PuppetType type)
    {
        if (puppetTeam != this.team)
        {
            enemyList.Add(enemy);
        }
    }

    protected void EnemyRemove(Team eTeam, Transform enemy, PuppetType type)
    {
        if (eTeam != this.team)
        {
            enemyList.Remove(enemy);
        }
    }

    #endregion

    #region Abstract Methods

    protected abstract void Attack();
    /*
    public virtual void MakeMove(PlayInstructs instrux)
    {
        ResetNotes();
        Color col = GetComponentInChildren<SpriteRenderer>().color;
        GetComponentInChildren<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 1);
    }

    public virtual PlayInstructs CurrInstruction()
    {
        Color col = GetComponentInChildren<SpriteRenderer>().color;
        GetComponentInChildren<SpriteRenderer>().color = new Color(col.r, col.g, col.b, .5f);
        return null;
    }
    */
    protected abstract void Rest();

    #endregion

    #region Helper Functions

    /// <summary>
    /// Sorts enemies by distance.
    /// </summary>
    protected virtual void SortEnemyList()
    {
        enemyList.Sort((x, y) => CalcUtil.DistCompare(transform, x, y));
    }

    #endregion

    public virtual void Setup(Team newTeam, PuppetType newType, Vector2 newLoc)
    {
        energy = 100;
        team = newTeam;
        currType = newType;
        gridLocation = newLoc;
        UnitManager.Instance.AddUnit += EnemyAdd;
        UnitManager.Instance.RemoveUnit += EnemyRemove;
        ArenaNode n = ArenaManager.Instance.nodeGrid[(int)newLoc.x, (int)newLoc.y];
        n.AddPuppet(this);
    }

    [BitStrap.Button]
    public void DeathTest()
    {
        UnitManager.Instance.AddUnit -= EnemyAdd;
        UnitManager.Instance.RemoveUnit -= EnemyRemove;
        UnitManager.Instance.UnitDied(this);
    }


    protected virtual void DeathMethod()
    {
        if (energy <= 0)
        {
            UnitManager.Instance.AddUnit -= EnemyAdd;
            UnitManager.Instance.RemoveUnit -= EnemyRemove;
            UnitManager.Instance.UnitDied(this);
        }
    }

    protected void SetNote()
    {
        /*
        switch (currNote)
        {
            
            case ConstFile.Notes.SIXTEENTH:
                sixteenth.SetActive(true);
                break;
            case ConstFile.Notes.EIGHTH:
                eigth.SetActive(true);
                break;
            case ConstFile.Notes.QUARTER:
                quarter.SetActive(true);
                break;
            case ConstFile.Notes.HALF:
                half.SetActive(true);
                break;
            case ConstFile.Notes.WHOLE:
                whole.SetActive(true);
                break;
        }
        */
    }

    protected void ResetNotes()
    {
        /*
        sixteenth.SetActive(false);
        eigth.SetActive(false);
        quarter.SetActive(false);
        half.SetActive(false);
        whole.SetActive(false);
        */
    }
}
