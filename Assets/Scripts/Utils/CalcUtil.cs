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


    public static float GridDist(Vector2 x1, Vector2 x2)
    {
        float xd = x1.x - x2.x;
        float yd = x1.y - x2.y;
        return Mathf.Sqrt((xd * xd) + (yd * yd));
    }
}
