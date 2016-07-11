using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ProgressionEditor : EditorWindow
{
	Progression prog;
	int load;
	protected static GUILayoutOption[] __closeTogglesOptions = new GUILayoutOption[]
	{ GUILayout.Width(20f), GUILayout.ExpandWidth(false) };


	[MenuItem("MusicEditor/Progression Editor")]
	static void CreateWindow()
	{
		ProgressionEditor window = GetWindow<ProgressionEditor>();
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Progressions");
		FileInfo[] files = dir.GetFiles("*.json");

		List<string> fileNames = new List<string>();
		for (int i = 0; i < files.Length; i++)
		{
			fileNames.Add(files[i].Name);
		}
		EditorGUILayout.Popup(0, fileNames.ToArray(), GUILayout.MaxWidth(200f));
		
		if (GUILayout.Button("Save") && prog != null && !string.IsNullOrEmpty(prog.progName))
		{
			string json = JsonUtility.ToJson(prog, true);
			File.WriteAllText(string.Format("Assets/Resources/Progressions/{0}.json", prog.progName), json);
			EditorPrefs.SetString("Last Prog", string.Format("Assets/Resources/Progressions/{0}.json", prog.progName));
		}
		if (GUILayout.Button("Load") && files.Length > 0)
		{
			NewProgression();
			JsonUtility.FromJsonOverwrite(File.ReadAllText(files[load].ToString()), prog);
			EditorPrefs.SetString("Last Prog", string.Format("Assets/Resources/Progressions/{0}.json", prog.progName));
		}

		EditorGUILayout.EndHorizontal();


		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (EditorPrefs.GetString("Last Prog") == null && prog == null)
		{
			NewProgression();
			EditorUtility.SetDirty(this);
		}
		else
		{
		}

		ProgressionEditBox();

		if (GUILayout.Button("Create New Progression"))
		{
			NewProgression();
		}


		
	}

	void NewProgression()
	{
		prog = CreateInstance<Progression>() as Progression;
		prog.Init();
	}

	void ProgressionEditBox()
	{
		prog.progName = EditorGUILayout.TextField("Progression Name: ", prog.progName);

		EditorGUILayout.BeginVertical("box");

		ChordEditBox();

		if (GUILayout.Button("Add Chord"))
		{
			prog.AddChord();
		}

		EditorGUILayout.EndVertical();
	}

	void ChordEditBox()
	{
		for (int i = 0; i < prog.prog.Length; i++)
		{
			ChordNotation chord = prog.prog[i];
			EditorGUILayout.BeginVertical("Box");

			

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Chord Base");
				chord.chordBase = (ConstFile.ROMAN_NUMBERAL)EditorGUILayout.EnumPopup(chord.chordBase);
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Chord Type");
				chord.chordType = (ConstFile.CHORD_TYPE)EditorGUILayout.EnumPopup(chord.chordType);
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Chord Length");
				chord.noteLen = (ConstFile.NoteLen)EditorGUILayout.EnumPopup(chord.noteLen);
				EditorGUILayout.EndVertical();

				EditorGUILayout.EndHorizontal();

			chord.PlayOn = (ConstFile.NoteLen)EditorGUILayout.EnumPopup("Note to play on", chord.PlayOn, GUILayout.MaxWidth(400f));

				EditorGUILayout.BeginHorizontal();
				for (int j = 0; j < chord.Counts.Length; j++)
				{
					chord.Counts[j] = GUILayout.Toggle(chord.Counts[j], "", __closeTogglesOptions);
				}
				EditorGUILayout.EndHorizontal();

			Color oldColor = GUI.color;
			GUI.color = Color.red;
			if (GUILayout.Button("X", GUILayout.MaxWidth(30f)))
			{
				prog.RemoveChord(chord);
			}
			GUI.color = oldColor;
			EditorGUILayout.EndVertical();

			prog.prog[i] = chord;
			prog.Clean();
		}
	}
}
