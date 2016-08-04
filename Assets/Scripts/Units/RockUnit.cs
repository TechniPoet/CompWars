using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class RockUnit : Puppet
{

    protected override void Start()
    {
        base.Start();
        instructions = new Instructions();
        JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine("Assets/Resources/AI", "RockAI.json")), instructions);
    }


    protected override void Attack()
    {
        throw new NotImplementedException();
    }

    protected override void Rest()
    {
        throw new NotImplementedException();
    }


    #region Test methods

    [BitStrap.Button]
    public void MoveRightTest()
    {
        if (currNode.right != null)
        {
            currNode.right.AddPuppet(this);
        }
    }

    [BitStrap.Button]
    public void MoveLeftTest()
    {
        if (currNode.left != null)
        {
            currNode.left.AddPuppet(this);
        }
    }
    #endregion
}
