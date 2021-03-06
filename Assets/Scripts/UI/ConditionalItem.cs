﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ConditionalItem
{
	public ConstFile.ConditionOptions cond1Ind;
	public float cond1Val;
	public bool greater;
	public ConstFile.ConditionOptions cond2Ind;
	public float cond2Val;
	public ConstFile.Actions action;
	public ConstFile.NoteLen note;
	string sentence;

	public ConditionalItem()
	{
		cond1Val = 0;
		cond2Val = 0;
		cond1Ind = ConstFile.ConditionOptions.ENEMY_DISTANCE;
		greater = true;
		cond2Ind = ConstFile.ConditionOptions.VALUE;
		action = ConstFile.Actions.REST;
		note = ConstFile.NoteLen.QUARTER;
	}
	public string GetSentence()
	{
		string cond1String = ConditionString(cond1Ind, true);
		string cond2String = ConditionString(cond2Ind, false);
		string boolCondString = greater ? ">" : "<";
		string actionString = ActionString(action);
		return string.Format("If {0} {1} {2}, {3} for {4} note", cond1String, boolCondString, cond2String, actionString, note.ToString());
	}


	string ActionString (ConstFile.Actions act)
	{
		string retString = string.Empty;
		switch (act)
		{
			case ConstFile.Actions.ATTACK:
				retString = "attack";
				break;
			case ConstFile.Actions.MOVE_BACK:
				retString = "move back";
				break;
			case ConstFile.Actions.MOVE_ENEMY:
				retString = "move towards enemy";
				break;
			case ConstFile.Actions.MOVE_FORWARD:
				retString = "move forward";
				break;
			case ConstFile.Actions.REST:
				retString = "rest";
				break;
		}

		return retString;
	}


	string ConditionString(ConstFile.ConditionOptions opt, bool one)
	{
		string retString = string.Empty;
		switch (opt)
		{
			case ConstFile.ConditionOptions.ENEMY_DISTANCE:
				retString = "Enemy Distance";
				break;
			case ConstFile.ConditionOptions.ENERGY:
				retString = "Energy";
				break;
			case ConstFile.ConditionOptions.VALUE:
				retString = one ? cond1Val.ToString() : cond2Val.ToString();
				break;
		}

		return retString;
	}
}
