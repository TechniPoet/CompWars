using UnityEngine;
using System.Collections;

public class UnitTestScene : MonoBehaviour {

	public void CreateUnit()
    {
        UnitManager.Instance.CreateNewUnit(ConstFile.PuppetType.ROCK, ConstFile.Team.LEFT, new Vector2(5,6));
    }
}
