using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Instructions : ScriptableObject
{
    [SerializeField]
    public List<Instruct> instructs = new List<Instruct>();
    

    public void AddInstruct()
    {
        instructs.Add(new Instruct());
    }
}
