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
    Vector2 scrollPos;
    Instructions instructs;

    List<swap> swaps = new List<swap>();
    List<int> removes = new List<int>();


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
        }
        if (GUILayout.Button("Save"))
        {
            string json = JsonUtility.ToJson(instructs, true);
            File.WriteAllText(Path.Combine("Assets/Resources/AI", "RockAI.json"), json);
        }

        EditorGUILayout.BeginVertical();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < instructs.instructs.Count; i++)
        {

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

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
            instructs.instructs[i].cond1 = (ConstFile.ConditionOptions)EditorGUILayout.EnumPopup(instructs.instructs[i].cond1);
            if (instructs.instructs[i].cond1 == ConstFile.ConditionOptions.VALUE)
            {
                instructs.instructs[i].cond1Val = EditorGUILayout.FloatField(instructs.instructs[i].cond1Val);
            }
            EditorGUILayout.EndVertical();

            instructs.instructs[i].comp = (ConstFile.BOOLEAN)EditorGUILayout.EnumPopup(instructs.instructs[i].comp);

            EditorGUILayout.BeginVertical();
            instructs.instructs[i].cond2 = (ConstFile.ConditionOptions)EditorGUILayout.EnumPopup(instructs.instructs[i].cond2);
            if (instructs.instructs[i].cond2 == ConstFile.ConditionOptions.VALUE)
            {
                instructs.instructs[i].cond2Val = EditorGUILayout.FloatField(instructs.instructs[i].cond2Val);
            }
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



            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            instructs.instructs[i].action = (ConstFile.Actions)EditorGUILayout.EnumPopup(instructs.instructs[i].action);

            instructs.instructs[i].length = (ConstFile.NoteLen)EditorGUILayout.EnumPopup(instructs.instructs[i].length);
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
