using UnityEngine;
using System.Collections;

public class UnitAnimationHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {

		gameObject.GetComponent<Animation> ().Play ("die 2");
	
	}
	

}
