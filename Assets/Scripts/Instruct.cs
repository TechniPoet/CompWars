using UnityEngine;

[System.Serializable]
public class Instruct
{
    public ConstFile.ConditionOptions cond1;
    public ConstFile.BOOLEAN comp;
    public ConstFile.ConditionOptions cond2;
    public float cond1Val;
    public float cond2Val;

    public ConstFile.Actions action;
    public ConstFile.NoteLen length;
}
