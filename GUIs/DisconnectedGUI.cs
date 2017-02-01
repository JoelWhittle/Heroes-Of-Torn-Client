using UnityEngine;
using System.Collections;

public class DisconnectedGUI : MonoBehaviour {


	public float fWindowPosX;
	public float fWindowPosY;
	public float fWindowSizeX;
	public float fWindowSizeY;
	public float fLabelPosX;
	public float fLabelPosY;
	public float fLabelSizeX;
	public float fLabelSizeY;
	public string sLabel;
	public string[] sDefaultLabels;

	public NetworkManager cNetworkManager;

	void Start()
	{
		cNetworkManager = GameObject.Find ("_NetworkManager").GetComponent<NetworkManager> ();
		sLabel = sDefaultLabels [Random.Range (0, sDefaultLabels.Length)];
	}

	// Update is called once per frame
	void Update () {
	
		if(cNetworkManager.bConnected())
		{
			this.gameObject.SetActive(false);
		}

	}


	public void RandomizeMessage()
	{
		sLabel = sDefaultLabels [Random.Range (0, sDefaultLabels.Length)];

	}
	void OnGUI()
	{
		GUI.Box(new Rect(StaticUtils.fRelativeScreenPos(fWindowPosX, fWindowSizeX, "width"),
		                 StaticUtils.fRelativeScreenPos(fWindowPosY, fWindowSizeY, "height"),
		                 StaticUtils.scrnW(fWindowSizeX),
		                 StaticUtils.scrnH(fWindowSizeY)),
		                 "..Disconnected..");
		
		
		GUI.Label (new Rect (StaticUtils.fRelativeScreenPos(fLabelPosX, fLabelSizeX, "width"),
		                     StaticUtils.fRelativeScreenPos(fLabelPosY, fLabelSizeY, "height"),
		                     StaticUtils.scrnW(fLabelSizeX),
		                     StaticUtils.scrnH(fLabelSizeY)),
		                     sLabel);
		
	

	}
}
