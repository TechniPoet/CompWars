using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class IntructionsEditorWindow : EditorWindow
{
    struct swap
    {
        public int x2;
        public int x1;
        public swap(int newX1, int newX2)
        {
            x1 = newX1;
            x2 = newX2;
        }
    }

    static GUILayoutOption[] __controlButtonOptions = new GUILayoutOption[]
    {
        GUILayout.Width(30f)
    };

	static GUILayoutOption[] __enumPopupOptions = new GUILayoutOption[]
	{
		GUILayout.MinWidth(60f),
		GUILayout.MaxWidth(400f),
	};
    Vector2 scrollPos;
    Instructions instructs;

    List<swap> swaps = new List<swap>();
    List<int> removes = new List<int>();
	const string aiDir = "Assets/Resources/AI";

	[MenuItem("AI/RockAI")]
    static void CreateWindow()
    {
        IntructionsEditorWindow window = GetWindow<IntructionsEditorWindow>();
    }


	public void OnGUI()
	{
        GUILayout.Label("Rock Instructions");
        if (instructs == null)
        {
            instructs = CreateInstance<Instructions>();
			if (File.Exists(Path.Combine(aiDir, "RockAI.json")))
			{
				JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(aiDir, "RockAI.json")), instructs);
			}
        }
		EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            string json = JsonUtility.ToJson(instructs, true);
            File.WriteAllText(Path.Combine(aiDir, "RockAI.json"), json);
        }
		if (GUILayout.Button("Reset"))
		{
			if (File.Exists(Path.Combine(aiDir, "RockAI.json")))
			{
				JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(aiDir, "RockAI.json")), instructs);
			}
		}
		EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < instructs.instructs.Count; i++)
        {

            EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			#region instruction Box
					#region Nav Buttons

					EditorGUILayout.BeginVertical();
					if (i != 0)
					{
						if (GUILayout.Button("/\\", __controlButtonOptions))
						{
							swaps.Add(new swap(i, i - 1));
						}
					}
					if (i != instructs.instructs.Count - 1)
					{
						if (GUILayout.Button("\\/", __controlButtonOptions))
						{
							swaps.Add(new swap(i, i + 1));
						}
					}


					EditorGUILayout.EndVertical();

			#endregion
				EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
					#region Top Row
						EditorGUILayout.BeginVertical();
						#region Cond 1
						GUILayout.Label("Condition 1");
						instructs.instructs[i].cond1 = (ConstFile.ConditionOptions)EditorGUILayout.EnumPopup(instructs.instructs[i].cond1, __enumPopupOptions);
						if (instructs.instructs[i].cond1 == ConstFile.ConditionOptions.VALUE)
						{
							instructs.instructs[i].cond1Val = EditorGUILayout.FloatField(instructs.instructs[i].cond1Val);
						}
						#endregion
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical();
						GUILayout.Label("Operator");
						instructs.instructs[i].comp = (ConstFile.BOOLEAN)EditorGUILayout.EnumPopup(instructs.instructs[i].comp, __enumPopupOptions);
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical();
						#region Cond 2
						GUILayout.Label("Condition 2");
						instructs.instructs[i].cond2 = (ConstFile.ConditionOptions)EditorGUILayout.EnumPopup(instructs.instructs[i].cond2, __enumPopupOptions);
						if (instructs.instructs[i].cond2 == ConstFile.ConditionOptions.VALUE)
						{
							instructs.instructs[i].cond2Val = EditorGUILayout.FloatField(instructs.instructs[i].cond2Val);
						}
						#endregion
						EditorGUILayout.EndVertical();

					#endregion
					EditorGUILayout.EndHorizontal();

				
				EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					#region Actions
					GUILayout.Label("Do");
					instructs.instructs[i].action = (ConstFile.Actions)EditorGUILayout.EnumPopup(instructs.instructs[i].action, __enumPopupOptions);
					GUILayout.Label("On");
					instructs.instructs[i].length = (ConstFile.NoteLen)EditorGUILayout.EnumPopup(instructs.instructs[i].length, __enumPopupOptions);
				GUILayout.Label(" Notes");
				#endregion
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();

			#region Control buttons


			Color preColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.red;
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("X", __controlButtonOptions))
				{
					removes.Add(i);
				}
				GUI.backgroundColor = preColor;
				#endregion


			#endregion
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }



        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Condition"))
        {
            instructs.AddInstruct();
        }
        EditorGUILayout.EndVertical();



        if (swaps.Count > 0)
        {
            foreach (swap s in swaps)
            {

                Instruct i = instructs.instructs[s.x1];
                instructs.instructs[s.x1] = instructs.instructs[s.x2];
                instructs.instructs[s.x2] = i;
            }
            swaps = new List<swap>();
        }
        if (removes.Count > 0)
        {
            foreach (int r in removes)
            {
                instructs.instructs.RemoveAt(r);
            }
            removes = new List<int>();
        }
    }
}
