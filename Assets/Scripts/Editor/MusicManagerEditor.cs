using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GAudio;

[CustomEditor(typeof(MusicManager))]
[System.Serializable]
public class MusicManagerEditor : Editor
{
	protected MusicManager m;
    protected ReorderableList progList;
	protected static GUILayoutOption[] __closeTogglesOptions = new GUILayoutOption[] 
	{ GUILayout.Width(20f), GUILayout.ExpandWidth(false) };

	protected static GUIStyle __boxStyle;
	protected static Color __blueColor = new Color(.7f, .7f, 1f);
	protected static Color __purpleColor = new Color(.8f, .6f, 1f);

	void Awake()
    {
        m = (MusicManager)target;
    }


	public override void OnInspectorGUI()
	{
        
		//DrawDefaultInspector();
        
        m.sampleBank = (GATActiveSampleBank)EditorGUILayout.ObjectField("Sample Bank", m.sampleBank, typeof (GATActiveSampleBank), true);
        m.toLoad = (GATSoundBank)EditorGUILayout.ObjectField("Sound Bank to Load", m.toLoad, typeof(GATSoundBank), true);
        m.mainPulse = (MasterPulseModule)EditorGUILayout.ObjectField("Main Pulse", m.mainPulse, typeof(MasterPulseModule), true);
        m.pulser = (PulseScript)EditorGUILayout.ObjectField("Pulser", m.pulser, typeof(PulseScript), true);
        m.playHead = (Transform)EditorGUILayout.ObjectField("Play Head", m.playHead, typeof(Transform), true);
        m.key = (ConstFile.NOTE)EditorGUILayout.EnumPopup("Key", m.key);
        
        EditorGUILayout.LabelField("Chord Progressions");

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/MultiProgressions");
        FileInfo[] files = dir.GetFiles("*.json");
        List<string> fileNames = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            fileNames.Add(files[i].Name);
        }
        m.loadInd = EditorGUILayout.Popup(m.loadInd, fileNames.ToArray());
        m.MultiProgName = fileNames[m.loadInd];

        GUILayout.Label("Curr Progression Index: "+ m.progressionInd);
        GUILayout.Label("Beat: "+ m.currBeat);
        if (m.progressions != null && m.progressions.Count > 0)
        {
            GUILayout.Label("prog list size: "+ m.progressions.Count);
            GUILayout.Label("Curr Progression: " + m.currProgression.progName);

            foreach (Progression p in m.progressions)
            {
                GUILayout.Label(p.progName);
            }
        }

		if (GUI.changed)
		{
			EditorUtility.SetDirty(m);
		}

        /*
		if (__boxStyle == null)
		{
			__boxStyle = new GUIStyle(GUI.skin.box);
			__boxStyle.fontSize = 15;
			__boxStyle.fontStyle = FontStyle.Bold;
		}


		

		//base.OnInspectorGUI();
		m = (MusicManager)target;
		m.sampleBank = (GATActiveSampleBank)EditorGUILayout.ObjectField("Active Sample Bank", m.sampleBank, typeof(GATActiveSampleBank), true);
		m.toLoad = (GATSoundBank)EditorGUILayout.ObjectField("Sound Bank", m.toLoad, typeof(GATSoundBank), true);

		m.mainPulse = (MasterPulseModule)EditorGUILayout.ObjectField("Main Pulse", m.mainPulse, typeof(GATSoundBank), true);
		m.pulser = (PulseScript)EditorGUILayout.ObjectField("Pulser", m.pulser, typeof(PulseScript), true);

		m.key = (ConstFile.NOTE)EditorGUILayout.EnumPopup("Scale Key", m.key);

		EditorGUILayout.LabelField("Progressions");
		EditorGUILayout.BeginVertical("box");
		for (int i = 0; i < m.progressions.Count; i++)
		{
			Progression prog = m.progressions[i];

			EditorGUILayout.BeginVertical("box");
			prog.progName = EditorGUILayout.TextField("Progression name", "");
			
			GUILayout.BeginHorizontal();
			GUI.color = __purpleColor;

			

			ChordNotation[] chords = prog.prog;
			for (int j = 0; j < chords.Length; j++)
			{
				prog[j].isOn = GUILayout.Toggle(prog[j].isOn, "", __closeTogglesOptions);
			}
			
			GUI.color = Color.white;
			GUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.EndVertical();
		*/
    }
}
