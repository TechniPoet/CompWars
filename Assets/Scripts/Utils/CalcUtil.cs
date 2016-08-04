using UnityEngine;
using System.Collections;

public static class CalcUtil{

	public static int DistCompare(Transform me, Transform x, Transform y)
    {
		if (x == null || y == null)
		{
			return 0;
		}
        float xDist = Vector2.Distance(me.position, x.position);
        float yDist = Vector2.Distance(me.position, y.position);
        return xDist.CompareTo(yDist);
    }
}
