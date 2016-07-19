using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ProgressionEditor : EditorWindow
{
	Progression prog;
	int load = 0;
    Vector2 scrollPos;
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
        if (files.Length > 0)
        {
            load = EditorGUILayout.Popup(load, fileNames.ToArray(), GUILayout.MaxWidth(200f));
        }
		
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
		if (EditorPrefs.GetString("Last Prog") == null)
		{
            if (prog == null)
            {
                NewProgression();
            }
		}
		else
		{
            if (prog == null)
            {
                NewProgression();
                JsonUtility.FromJsonOverwrite(File.ReadAllText(EditorPrefs.GetString("Last Prog").ToString()), prog);
            }
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
		prog.progName = EditorGUILayout.TextField("Progression Name:", prog.progName);

		EditorGUILayout.BeginVertical("box");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        ChordEditBox();
        EditorGUILayout.EndScrollView();
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
				EditorGUILayout.LabelField("Chord Base", GUILayout.MinWidth(80));
				chord.chordBase = (ConstFile.ROMAN_NUMBERAL)EditorGUILayout.EnumPopup(chord.chordBase, GUILayout.MinWidth(80));
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Chord Type", GUILayout.MinWidth(80));
				chord.chordType = (ConstFile.CHORD_TYPE)EditorGUILayout.EnumPopup(chord.chordType, GUILayout.MinWidth(80));
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Chord Length", GUILayout.MinWidth(90));
				chord.noteLen = (ConstFile.NoteLen)EditorGUILayout.EnumPopup(chord.noteLen, GUILayout.MinWidth(90));
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
