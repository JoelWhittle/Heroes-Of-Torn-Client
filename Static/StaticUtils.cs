using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StaticUtils {


	public static float scrnH ( float percentage)
	{
		return (Screen.height / 100 )* percentage;
	}
	public static float scrnW ( float percentage)
	{
		return (Screen.width / 100 )* percentage;
	}

	public static float fRelativeScreenPos ( float fPos, float fObjectDimension, string sScreenDimension)
	{
		float f = 0f;

		switch (sScreenDimension) 
		{

		case "width":
			 
			f =((Screen.width / 100 )* fPos ) - ((Screen.width / 100 * fObjectDimension) / 2);
			break;
		case "height":

			f = ((Screen.height / 100 )* fPos ) - ((Screen.height / 100 * fObjectDimension) / 2);
			break;
		default:

			f = 0;
			Debug.Log("Invalid Screen dimension from StaticUtils.fRelativeScreenPos");
			break;

		}
		return f;
	}

		public static int RollDieWithLuck(int iDieCount, int iDieSize, int iLuck)
	{

		List<int> iRolls = new List<int> ();
		int iNoOfExtraDice = Mathf.Abs (0 - iLuck);
		for(int i = 0; i < iDieCount + iNoOfExtraDice ; i++)
		{
			iRolls.Add(Random.Range(1,iDieSize + 1));
		}

		iRolls.Sort((x,y)=> y.CompareTo(x));  

		if(iLuck < -0.1f)
		{
			iRolls.Reverse ();
		}


		int iResult = 0;

		for(int n = 0; n < iDieCount; n++)
		{
			iResult = iResult + iRolls[n];
		}
		Debug.Log (iResult.ToString());

		return iResult;
	}


}
