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

    public ConstFile.Actions currAction;
    public Vector2 currTarget;
    public ConstFile.NoteLen currNote;
    bool decisionMade = false;

    [System.NonSerialized]
    public int moveCnt = 0;

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

    protected void Move(ConstFile.Direction dir)
    {
        for (int i = 0; i < moveCnt; i++)
        {
            switch (dir)
            {
                case ConstFile.Direction.LEFT:
                    if (currNode.left != null)
                    {
                        if (currNode.left.AddPuppet(this))
                        {
                            currNode.right.RemovePuppet();
                        }
                    }
                    // Remove proper energy
                    break;

                case ConstFile.Direction.RIGHT:
                    if (currNode.right != null)
                    {
                        if (currNode.right.AddPuppet(this))
                        {
                            currNode.left.RemovePuppet();
                        }
                    }
                    
                    // Remove proper energy
                    break;
                case ConstFile.Direction.UP:
                    if (currNode.up != null)
                    {
                        if (currNode.up.AddPuppet(this))
                        {
                            currNode.down.RemovePuppet();
                        }
                    }
                    break;
                case ConstFile.Direction.DOWN:
                    if (currNode.down != null)
                    {
                        if (currNode.down.AddPuppet(this))
                        {
                            currNode.up.RemovePuppet();
                        }
                    }
                    break;
            }
        }
    }
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

    public void MakeDecision()
    {
        if (instructions != null)
        {
            moveCnt = 0;
            decisionMade = false;
            for (int i = 0; i < instructions.instructs.Count && !decisionMade; i++)
            {
                Instruct inst = instructions.instructs[i];
                float const1 = EvalInstruct(inst.cond1, inst.cond1Val);
                float const2 = EvalInstruct(inst.cond2, inst.cond2Val);

                decisionMade = inst.comp == ConstFile.BOOLEAN.GREATER_THAN ? const1 > const2 : const1 < const2;

                if (decisionMade)
                {
                    currAction = inst.action;
                    currNote = inst.length;
                    if (currAction == ConstFile.Actions.MOVE_ENEMY || currAction == ConstFile.Actions.ATTACK)
                    {
                        currTarget = enemyList[0].position;
                    }

                    if (currAction == ConstFile.Actions.MOVE_FORWARD
                        || currAction == ConstFile.Actions.MOVE_BACK
                        || currAction == ConstFile.Actions.MOVE_ENEMY)
                    {
                        switch (currNote)
                        {
                            case ConstFile.NoteLen.SIXTEENTH:
                                moveCnt = 1;
                                break;
                            case ConstFile.NoteLen.EIGHTH:
                                moveCnt = 2;
                                break;
                            case ConstFile.NoteLen.QUARTER:
                                moveCnt = 4;
                                break;
                            case ConstFile.NoteLen.HALF:
                                moveCnt = 8;
                                break;
                            case ConstFile.NoteLen.WHOLE:
                                moveCnt = 16;
                                break;
                        }
                    }
                }
            }
            if (!decisionMade)
            {
                decisionMade = true;
                currAction = ConstFile.Actions.REST;
                currNote = ConstFile.NoteLen.HALF;
            }
        }
        else
        {
            Error("No instructions");
        }
    }

    public void ExecuteChoice()
    {
        switch (currAction)
        {
            case ConstFile.Actions.ATTACK:
                break;
            case ConstFile.Actions.MOVE_BACK:
                break;
            case ConstFile.Actions.MOVE_ENEMY:
                break;
            case ConstFile.Actions.MOVE_FORWARD:
                if (team == Team.LEFT)
                {
                    Move(ConstFile.Direction.RIGHT);
                }
                else
                {
                    Move(ConstFile.Direction.LEFT);
                }
                break;
            case ConstFile.Actions.REST:
                break;
        }
        moveCnt = 0;
    }


    float EvalInstruct(ConstFile.ConditionOptions opt, float val)
    {
        float ret = 9999999999999999999999f;
            switch (opt)
            {
                case ConstFile.ConditionOptions.ENEMY_DISTANCE:
                    if (enemyList.Count > 0)
                    {
                        SortEnemyList();
                        ret = Vector2.Distance(enemyList[0].position, transform.position);
                    }
                    break;
                case ConstFile.ConditionOptions.ENERGY:
                    ret = energy;
                    break;
                case ConstFile.ConditionOptions.VALUE:
                    ret = val;
                    break;
            }
        
        return ret;
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



    #region Logging

    protected void Log(string l)
    {

        Debug.Log(string.Format("{0}\n{1}", l, PuppetInfo()));
    }

    protected void Error(string l)
    {
        Debug.LogError(string.Format("{0}\n{1}", l, PuppetInfo()));
    }

    protected void Warning(string l)
    {
        Debug.LogWarning(string.Format("{0}\n{1}", l, PuppetInfo()));
    }
    protected string PuppetInfo()
    {
        return string.Format("Unit Type: {0}\nUnit Team: {1}\nUnit Dead: {2}\nGrid Loc: {3}\nID: {4}",
            currType, team, dead, gridLocation, id);
    }

    #endregion
}
