using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class MultiProgression : ScriptableObject
{
    [System.Serializable]
    public class prog
    {
        [System.NonSerialized]
        const string loadPath = "Assets/Resources/Progressions";
        public string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        [System.NonSerialized]
        public bool loaded;

        [System.NonSerialized]
        public Progression p;
        [System.NonSerialized]
        public int currFile;

        public prog(string file, int fileNum)
        {
            FileName = file;
            loaded = false;
            currFile = fileNum;
            
        }


        public void load()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                p = CreateInstance<Progression>();
                p.Init();
                JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(loadPath, FileName)), p);
                this.loaded = true;
            }
            else
            {

            }
            
        }
    }

    public string multiProgName;
    public List<prog> progFiles;
    public List<prog> ProgFiles
    {
        get
        {
            if (progFiles == null)
            {
                progFiles = new List<prog>();
            }
            return progFiles;
        }
        set
        {
            progFiles = value;
        }
    }
    
	public void Init()
    {
        multiProgName = "Progression Series";
        progFiles = new List<prog>();
    }

    public void Load()
    {
        // Load xml files
        for(int i = 0; i < progFiles.Count; i++)
        {
            progFiles[i].load();
        }
    }

    public void AddProgression(string p, int i, bool load = false)
    {
        prog newProg = new prog(p, i);
        if (load)
        {
            newProg.load();
        }
        progFiles.Add(newProg);
    }
}
