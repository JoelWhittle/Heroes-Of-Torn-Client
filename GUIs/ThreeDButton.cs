using UnityEngine;
using System.Collections;

public class ThreeDButton : MonoBehaviour {

	public bool bIsVisible = true;
	public bool bIsClickable = true;
	public GameObject goMenuToOpen;

	void OnMouseUp()
	{
		if(bIsClickable)
		{
			goMenuToOpen.SetActive(true);
		}
	}
}
