using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using GAudio;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor
{
	protected MusicManager m;

	protected static GUILayoutOption[] __closeTogglesOptions = new GUILayoutOption[] 
	{ GUILayout.Width(20f), GUILayout.ExpandWidth(false) };

	protected static GUIStyle __boxStyle;
	protected static Color __blueColor = new Color(.7f, .7f, 1f);
	protected static Color __purpleColor = new Color(.8f, .6f, 1f);

	protected void OnEnable()
	{
		
	}


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
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
