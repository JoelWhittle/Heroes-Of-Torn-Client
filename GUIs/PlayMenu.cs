using UnityEngine;
using System.Collections;

public class PlayMenu : MonoBehaviour {

	public string sLabel;
	public string sStatus = "";

	public int iSelectedDeck;
	public GameObject goPlayer;
	private Player cPlayer;
	public UnitCollection cUnitCollection;
	public NetworkManager cNetworkManager;

	void Start()
	{
		goPlayer = GameObject.Find ("Player");
		cPlayer = goPlayer.GetComponent<Player> ();
		cNetworkManager = GameObject.Find ("_NetworkManager").GetComponent<NetworkManager> ();
	}

	void Update()
	{
		if (cNetworkManager.bInGame == true) 
		{
			this.gameObject.SetActive(false);
				}
		}
void OnGUI()
	{
		if(!cPlayer.bInGame)
		{

		cUnitCollection = goPlayer.GetComponent<UnitCollection> ();
		// Make a background box
		GUI.Box(new Rect(scrnW(10),scrnH(10),scrnW(80), scrnH(95)), "Find Game");


		sLabel = "Current Rank: " + GameObject.Find ("Player").GetComponent<Player> ().iRank.ToString () + "   Current Deck: " + cUnitCollection.Decks[iSelectedDeck].sName;
		GUI.Label (new Rect (scrnW(20),scrnH(20),scrnW(50), scrnH(5)), sLabel);

		//Deck buttons
		if(GUI.Button(new Rect(scrnW(20),scrnH(40),scrnW(15), scrnH(20)), cUnitCollection.Decks[0].sName)) {
		
			iSelectedDeck = 0;
		}

		if(GUI.Button(new Rect(scrnW(40),scrnH(40),scrnW(15), scrnH(20)), cUnitCollection.Decks[1].sName)) {
			
			iSelectedDeck = 1;
		}
		if(GUI.Button(new Rect(scrnW(60),scrnH(40),scrnW(15), scrnH(20)), cUnitCollection.Decks[2].sName)) {
			
			iSelectedDeck = 2;
		}

		//
		if(GUI.Button(new Rect(scrnW(20),scrnH(60),scrnW(15), scrnH(20)), cUnitCollection.Decks[3].sName)) {
			
			iSelectedDeck = 3;
		}
		
		if(GUI.Button(new Rect(scrnW(40),scrnH(60),scrnW(15), scrnH(20)), cUnitCollection.Decks[4].sName)) {
			
			iSelectedDeck = 4;
		}
		if(GUI.Button(new Rect(scrnW(60),scrnH(60),scrnW(15), scrnH(20)), cUnitCollection.Decks[5].sName)) {
			
			iSelectedDeck = 5;
		}
		//
		
		if(GUI.Button(new Rect(scrnW(20),scrnH(80),scrnW(15), scrnH(20)), cUnitCollection.Decks[6].sName)) {
			
			iSelectedDeck = 6;
		}
		
		if(GUI.Button(new Rect(scrnW(40),scrnH(80),scrnW(15), scrnH(20)), cUnitCollection.Decks[7].sName)) {
			
			iSelectedDeck = 7;
		}
		if(GUI.Button(new Rect(scrnW(60),scrnH(80),scrnW(15), scrnH(20)), cUnitCollection.Decks[8].sName)) {
			
			iSelectedDeck = 8;
		}

		
		
		//

		GUI.Label (new Rect (scrnW(80),scrnH(25),scrnW(50), scrnH(5)), sStatus);

		
		


		if(GUI.Button(new Rect(scrnW(80),scrnH(20),scrnW(10), scrnH(5)), "Play Ranked")) {
			cNetworkManager = GameObject.Find("_NetworkManager").GetComponent<NetworkManager>();
			 StartCoroutine(cNetworkManager.LookForRankedGame());
			sStatus = "Finding Opponent";
		    Player.Instance.SelectedDeckIndex = iSelectedDeck;
		}
		}
	
		}



	public float scrnH ( float percentage)
	{
		return (Screen.height / 100 )* percentage;
	}
	public float scrnW ( float percentage)
	{
		return (Screen.width / 100 )* percentage;
	}
}




