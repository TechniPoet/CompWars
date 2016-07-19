using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using GAudio;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor
{
	protected MusicManager m;
    protected ReorderableList progList;
	protected static GUILayoutOption[] __closeTogglesOptions = new GUILayoutOption[] 
	{ GUILayout.Width(20f), GUILayout.ExpandWidth(false) };

	protected static GUIStyle __boxStyle;
	protected static Color __blueColor = new Color(.7f, .7f, 1f);
	protected static Color __purpleColor = new Color(.8f, .6f, 1f);

	protected void OnEnable()
	{

        progList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("progName"),
                true, true, true, true);
        
        progList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = progList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("progName"));
            };
	}


	public override void OnInspectorGUI()
	{
        m = (MusicManager)target;
		//DrawDefaultInspector();
        
        m.sampleBank = (GATActiveSampleBank)EditorGUILayout.ObjectField("Sample Bank", m.sampleBank, typeof (GATActiveSampleBank), true);
        m.toLoad = (GATSoundBank)EditorGUILayout.ObjectField("Sound Bank to Load", m.toLoad, typeof(GATSoundBank), true);
        m.mainPulse = (MasterPulseModule)EditorGUILayout.ObjectField("Main Pulse", m.mainPulse, typeof(MasterPulseModule), true);
        m.pulser = (PulseScript)EditorGUILayout.ObjectField("Pulser", m.pulser, typeof(PulseScript), true);
        m.key = (ConstFile.NOTE)EditorGUILayout.EnumPopup("Key", m.key);

        EditorGUILayout.LabelField("Chord Progressions");
        

        
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
