using UnityEngine;
using System.Collections;

[System.Serializable]
public static class ConstFile
{
	public enum PuppetType
	{
		BASS,
		TREBLE,
		TOWER,
	}

	public enum Direction : int
	{
		UP = 0,
		DOWN = 1,
		LEFT = 2,
		RIGHT = 3,
		UP_RIGHT = 4,
		UP_LEFT = 5,
		DOWN_RIGHT = 6,
		DOWN_LEFT = 7,
	}

	#region Direction Vectors

	public static Vector2 UPVec = new Vector2(0,1);
	public static Vector2 DOWNVec = new Vector2(0, -1);
	public static Vector2 LEFTVec = new Vector2(-1, 0);
	public static Vector2 RIGHTVec = new Vector2(1, 0);
	public static Vector2 UP_LEFTVec = new Vector2(-1, 1);
	public static Vector2 UP_RIGHTVec = new Vector2(1, 1);
	public static Vector2 DOWN_LEFTVec = new Vector2(-1, -1);
	public static Vector2 DOWN_RIGHTVec = new Vector2(1, -1);
	public static Vector2[] DirectionVectors = new Vector2[] {
		UPVec, DOWNVec, LEFTVec, RIGHTVec, UP_RIGHTVec, UP_LEFTVec, DOWN_RIGHTVec, DOWN_LEFTVec };


	#endregion


	public enum AIModes
	{
		AGGRESSIVE,
		DEFENSIVE,
		NEUTRAL,
	}

	public enum ConditionOptions : int
	{
		ENERGY = 0,
		ENEMY_DISTANCE = 1,
		VALUE = 2,
	}

	public static string[] ConditionOptionsText = new string[] { "Energy", "Enemy Distance", "Value" };

	public enum Actions
	{
		ATTACK = 0,
		MOVE_FORWARD = 1,
		MOVE_ENEMY = 2,
		MOVE_BACK = 3,
		REST = 4,
	}

	public static string[] ActionsText = new string[] { "Attack", "Move Forward", "Move to Enemy", "Move Back", "Rest" };

	public static float BPM = 30;
	public enum NoteLen : int
	{
		WHOLE = 0,
		HALF = 1,
		QUARTER = 2,
		EIGHTH = 3,
		SIXTEENTH = 4,
		/*
		DOTTED_QUARTER = 5,
		DOTTED_EIGTH = 6,
		DOTTED_SIXTEENTH = 7,
		*/
	}
	// Based on data from http://bradthemad.org/guitar/tempo_explanation.php 
	public static float[] NoteBPMCalcs = new float[] { 240, 120, 60, 30, 15, 90, 45, 22.5f};


	public enum CHORD_TYPE
	{
		TRIAD,
		MINOR,
	}

	public enum ROMAN_NUMBERAL : int
	{
		I = 0,
		II = 1,
		III = 2,
		IV = 3,
		V = 4,
		VI = 5,
		VII = 6,
	}


	public enum NOTE : int
	{
		A,
		A_SHARP,
		B,
		C,
		C_SHARP,
		D,
		D_SHARP,
		E,
		F,
		F_SHARP,
		G,
		G_SHARP,
	}

	public static string[] PIANO_NOTES = new string[]
	{
		"piano_{0}_A",
		"piano_{0}_As",
		"piano_{0}_B",
		"piano_{0}_C",
		"piano_{0}_Cs",
		"piano_{0}_D",
		"piano_{0}_Ds",
		"piano_{0}_E",
		"piano_{0}_F",
		"piano_{0}_Fs",
		"piano_{0}_G",
		"piano_{0}_Gs",
	};
}
