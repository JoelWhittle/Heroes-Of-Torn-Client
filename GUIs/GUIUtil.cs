using UnityEngine;
using System.Collections;

public static class GUIUtil {

	public static float ScrnPercent(string sDimension, float fPercent)
	{
		if(sDimension == "x")
		{
			return (Screen.width / 100) * fPercent;
		}
		else if(sDimension == "y")
		{
			return (Screen.height / 100) * fPercent;

		}
		else
		{
			return 0;
		}

	}
}
