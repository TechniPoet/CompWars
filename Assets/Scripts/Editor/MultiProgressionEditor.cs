using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MultiProgressionEditor : EditorWindow
{
    int load = 0;
    MultiProgression multiProg;
    protected static GUILayoutOption[] __closeTogglesOptions = new GUILayoutOption[]
    { GUILayout.Width(30f), GUILayout.ExpandWidth(false)};
    protected static GUILayoutOption[] __smallTogglesOptions = new GUILayoutOption[]
    { GUILayout.Width(15f), GUILayout.ExpandWidth(false)};
    static GUIStyle __noteRepFont = new GUIStyle()
    {
        fontSize = 10,
    };
    static float __noteRepWidth = 30f;
    static float __chordBoxMinWidth = 30f;
    static float __chordBoxMaxWidth = 60f;

    static string __editorPrefsSave = "Last MultiProg";
    List<string> fileNames;
    FileInfo[] progFiles;

    Vector2 scrollPos;
    bool shouldAddProgression = false;
    [MenuItem("MusicEditor/Multi Progressions")]
    static void CreateWindow()
    {
        MultiProgressionEditor window = GetWindow<MultiProgressionEditor>();
    }

    void OnGUI()
    {
        if (true)
        {
            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/MultiProgressions");
            FileInfo[] files = dir.GetFiles("*.json");
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                fileNames.Add(files[i].Name);
            }

            EditorGUILayout.Space();
//*
            EditorGUILayout.BeginHorizontal();
            if (files.Length > 0)
            {
                load = EditorGUILayout.Popup(load, fileNames.ToArray(), GUILayout.MaxWidth(200f));
            }

            if (GUILayout.Button("Save") && multiProg != null && !string.IsNullOrEmpty(multiProg.multiProgName))
            {
                string json = JsonUtility.ToJson(multiProg, true);
                File.WriteAllText(Path.Combine("Assets/Resources/MultiProgressions", string.Format("{0}.json", multiProg.multiProgName)), json);
                EditorPrefs.SetString(__editorPrefsSave, Path.Combine("Assets/Resources/MultiProgressions", string.Format("{0}.json", multiProg.multiProgName)));
            }

            if (GUILayout.Button("Load") && files.Length > 0)
            {
                NewMultiProg();
                JsonUtility.FromJsonOverwrite(File.ReadAllText(files[load].ToString()), multiProg);
                EditorPrefs.SetString(__editorPrefsSave, Path.Combine("Assets/Resources/MultiProgressions", string.Format("{0}.json", multiProg.multiProgName)));
            }
//*
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (multiProg == null)
            {
                NewMultiProg();
                if (EditorPrefs.GetString(__editorPrefsSave) != null)
                {
                    if (!string.IsNullOrEmpty(EditorPrefs.GetString(__editorPrefsSave)))
                    {
                        JsonUtility.FromJsonOverwrite(File.ReadAllText(EditorPrefs.GetString(__editorPrefsSave).ToString()), multiProg);
                    }
                }
            }

            MultiProgEditBox();
            
            if (GUILayout.Button("Create New Progression Series"))
            {
                NewMultiProg();
            }
            
        }
    }
    

    void NewMultiProg()
    {
        multiProg = CreateInstance<MultiProgression>() as MultiProgression;
        multiProg.Init();
    }

    void FixFileNums(List<string> fileNames)
    {
            Fix:
            for (int i = 0; i < multiProg.ProgFiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < fileNames.Count; j++)
                {
                    if (multiProg.progFiles[i].FileName == fileNames[j])
                    {
                        multiProg.progFiles[i].currFile = j;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Debug.Log("Could not find: "+ multiProg.progFiles[i].FileName + " " + fileNames[0]);
                    multiProg.progFiles.RemoveAt(i);
                    goto Fix;
                }
            }
        
    }


    void MultiProgEditBox()
    {
        
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Progressions");
        progFiles = dir.GetFiles("*.json");
        fileNames = new List<string>();
        if (progFiles != null)
        {
            for (int i = 0; i < progFiles.Length; i++)
            {
                fileNames.Add(progFiles[i].Name);
            }
            if (multiProg.progFiles.Count > 0)
            {
                int b = 1;
            }
            FixFileNums(fileNames);

            multiProg.multiProgName = EditorGUILayout.TextField("Progression Series Name", multiProg.multiProgName);
            
            EditorGUILayout.BeginVertical("box");
            #region Progression Box
           
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            #region scroll Progression
            for (int i = 0; i < multiProg.ProgFiles.Count; i++)
            {
                
                EditorGUILayout.BeginVertical("box");
                #region vertBox
                EditorGUILayout.BeginHorizontal();
                #region horiz
                MultiProgression.prog temp = multiProg.progFiles[i];
                temp.currFile = EditorGUILayout.Popup(multiProg.progFiles[i].currFile, fileNames.ToArray(), GUILayout.MaxWidth(200f));
                
                temp.FileName = fileNames[temp.currFile];
                multiProg.progFiles[i] = temp;
                
                multiProg.progFiles[i].load();

                #endregion
                EditorGUILayout.EndHorizontal();
                
                if (multiProg.progFiles[i].loaded)
                {
                    
                    EditorGUILayout.LabelField("Loaded: " + multiProg.progFiles[i].p.progName);
                    
                    ShowStaff(multiProg.progFiles[i].p);
                    
                    
                    
                    EditorGUILayout.BeginHorizontal();
                    #region horiz
                    ChordNotation[] chordProgression = multiProg.progFiles[i].p.prog;
                    for (int j = 0; j < chordProgression.Length; j++)
                    {
                        
                        ChordNotation chord = chordProgression[j];
                        
                        EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(__chordBoxMinWidth), GUILayout.MaxWidth(__chordBoxMaxWidth));
                        #region vertBox
                        EditorGUILayout.LabelField(chord.noteLen.ToString(), GUILayout.MinWidth(__chordBoxMinWidth));
                        EditorGUILayout.LabelField(chord.chordType.ToString(), GUILayout.MinWidth(__chordBoxMinWidth));
                        EditorGUILayout.LabelField(chord.chordBase.ToString(), GUILayout.MinWidth(__chordBoxMinWidth));
                        EditorGUILayout.LabelField(chord.playOn.ToString(), GUILayout.MinWidth(__chordBoxMinWidth));
                        
                        EditorGUILayout.BeginHorizontal();
                        #region horiz
                        for (int k = 0; k < chord.Counts.Length; k++)
                        {
                            GUILayout.Toggle(chord.Counts[k], "", __smallTogglesOptions);
                        }
                        #endregion
                        EditorGUILayout.EndHorizontal();
                        #endregion
                        EditorGUILayout.EndVertical();
                    }
                    #endregion
                    EditorGUILayout.EndHorizontal();
                    
                }
                else
                {
                    EditorGUILayout.LabelField("Not Loaded");
                }
                #endregion
                EditorGUILayout.EndVertical();
        
            }
            #endregion
            EditorGUILayout.EndScrollView();
            #endregion
            EditorGUILayout.EndVertical();
        }
        
        if (progFiles.Length > 0)
        {
            if (GUILayout.Button("Add New Progression"))
            {
                multiProg.AddProgression(fileNames[0], 0);
            }
        }
        
        
    }

    void ShowStaff(Progression currProgression)
    {
        EditorGUILayout.BeginHorizontal();
        #region horiz
        for (int m = 1; m < 17; m++)
        {
            int n = m - 1;
            
            EditorGUILayout.BeginVertical();
            #region vert
            GUILayout.Label(m.ToString());
            GUILayout.Toggle(false, "", __closeTogglesOptions);
            if (currProgression != null)
            {
                for (int k = 0; k < currProgression.prog.Length; k++)
                {
                        
                    ChordNotation chord = currProgression.prog[k];
                    switch (chord.playOn)
                    {
                        case ConstFile.NoteLen.WHOLE:
                            if (n % 16 == 0 && chord.Counts[0])
                            {
                                GUILayout.Label(chord.chordType.ToString(), __noteRepFont, GUILayout.Width(__noteRepWidth));
                                GUILayout.Label("W " + chord.chordBase, GUILayout.Width(__noteRepWidth));
                            }
                            break;
                        case ConstFile.NoteLen.HALF:
                            if (n % 8 == 0 && chord.Counts[n / 8])
                            {
                                GUILayout.Label(chord.chordType.ToString(), __noteRepFont, GUILayout.Width(__noteRepWidth));
                                GUILayout.Label("H " + chord.chordBase, GUILayout.Width(__noteRepWidth));
                            }
                            break;
                        case ConstFile.NoteLen.QUARTER:
                            if (n % 4 == 0 && chord.Counts[n / 4])
                            {
                                GUILayout.Label(chord.chordType.ToString(), __noteRepFont, GUILayout.Width(__noteRepWidth));
                                GUILayout.Label("Q " + chord.chordBase, GUILayout.Width(__noteRepWidth));
                            }
                            break;
                        case ConstFile.NoteLen.EIGHTH:
                            if (n % 2 == 0 && chord.Counts[n / 2])
                            {
                                GUILayout.Label(chord.chordType.ToString(), __noteRepFont, GUILayout.Width(__noteRepWidth));
                                GUILayout.Label("E " + chord.chordBase, GUILayout.Width(__noteRepWidth));
                            }
                            break;
                        case ConstFile.NoteLen.SIXTEENTH:
                            if (chord.Counts[n])
                            {
                                GUILayout.Label(chord.chordType.ToString(), __noteRepFont, GUILayout.Width(__noteRepWidth));
                                GUILayout.Label("S " + chord.chordBase, GUILayout.Width(__noteRepWidth));
                            }
                            break;
                    }
                }
            }


                
            
            #endregion
            EditorGUILayout.EndVertical();
        }
        #endregion
        EditorGUILayout.EndHorizontal();
    }
}
