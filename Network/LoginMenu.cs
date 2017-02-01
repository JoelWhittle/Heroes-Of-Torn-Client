using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;


//Handles the aesthetics and functionality of the LoginMenu..
//logic that is shared and would be called again during gameplay is found in SendRecievePlayerData
//Attatched to the _NetworkManager GameObject.
public class LoginMenu : MonoBehaviour {

	

	string username = "";
	string password = "";

	public string label = "";
	bool bSignUp = false;


	public GameObject goMainMenu;
	public SendRecievePlayerData cSendRecievePlayerData;

	public GUISkin myGUI;

	public NetworkManager cNetworkManager;


	public float fWindowPosX;
	public float fWindowPosY;
	public float fWindowSizeX;
	public float fWindowSizeY;
	public float fUsernameLabelPosX;
	public float fUsernameLabelPosY;
	public float fUsernameLabelSizeX;
	public float fUsernameLabelSizeY;

	//Find references on class initialisation
	void Start()
	{
	
		cSendRecievePlayerData = gameObject.GetComponent<SendRecievePlayerData> ();

	}

	//Show the Login/SignUp menu
	void OnGUI()
	{
				if (cNetworkManager.bShowLoginMenu())
		{


			GUI.Window (0, new Rect (StaticUtils.fRelativeScreenPos(fWindowPosX, fWindowSizeX, "width"),
			                                  StaticUtils.fRelativeScreenPos(fWindowPosY, fWindowSizeY, "height"),
			                                  StaticUtils.scrnW(fWindowSizeX),
			                                  StaticUtils.scrnH(fWindowSizeY)),
                                              LoginWindow,
			                                  "..Please Login..");

		}

		}

	//Creates the aesthetcs for the Login/Signup Menu, is called via the OnGUI
	void LoginWindow(int windowID)
	{
		GUI.Label ( new Rect( StaticUtils.fRelativeScreenPos(fUsernameLabelPosX, fUsernameLabelSizeX, "width"),
		                     StaticUtils.fRelativeScreenPos(fUsernameLabelPosY, fUsernameLabelSizeY, "height"),
		                     StaticUtils.scrnW(fUsernameLabelSizeX),
		                     StaticUtils.scrnH(fUsernameLabelSizeY)),
		           		     "..Username..");



		username = GUI.TextField (new Rect (25,60,375,30), username);
		GUI.Label ( new Rect( 140,92,130,100), "----Password----");
		password = GUI.TextField (new Rect (25,115,375,30), password);

		if (GUI.Button (new Rect (25, 160, 375, 50), "Login"))
		{
		StartCoroutine(HandleLogin(username,password));
				}
		if (GUI.Button (new Rect (25, 220, 375, 50), "Sign Up"))
		{
			StartCoroutine(HandleSignUp(username,password));
		}

		GUI.Label (new Rect (55, 282, 250, 100), label);

	}

//Sends the players entered Username to the database and if it has found a match returns the password
	//then we check the entered PAssword against the stored password for a match, if so sucessfull login, and requests the 
	//players data. Once the data has been recieved, we remove the login menu and go to the main menu.
	//Todo improve this and the signup system by hashing passwords and using a salt
	IEnumerator HandleLogin(string username, string password)
	{
		label = "Checking username and password";

	    string s = AsynchronousClient.Instance.Send("RequestLogin:" + username + ">" + password);

	    if (s == "Success")
	    {

            label = "SuccessfullLogin";
            Player.Instance.sName = username;


            yield return StartCoroutine(SendRecievePlayerData.Instance.HandleGetPlayerData(username));

            yield return StartCoroutine(SignInAndLaunchMainMenu());
	        
	    }

	    else
	    {
	        label = s;
	    }
	            
	    yield return new WaitForSeconds(0);
	}

	// Creates a new user in the database,  and hands over to cSendReicievePlayerData.ReadInBeginnersCollection to handle setting 
	//up new units. decks, and saving the data
	//Todo,, Improve this system by first making sure the entered Username doesnt already exist inside the database,
	//then by using a salt system to make players accounts safer.
	IEnumerator HandleSignUp(string username, string password)
	{
        if (username != "" && password != "")
	    {
	        label = "Checking username and password";

	        string s = AsynchronousClient.Instance.Send("RequestSignup:" + username + ">" + password);

	        if (s.Contains("Success"))
	        {
	            label = "Account created, hiring mercenaries";
	            string[] args = s.Split(">".ToCharArray());

	            Player cPlayer = GameObject.Find("Player").GetComponent<Player>();
	            cPlayer.sName = username;
	            cPlayer.iGold = int.Parse(args[1]);
	            cPlayer.iDust = int.Parse(args[2]);


	            //Bring in the UnitCatalogue db 
	            yield return StartCoroutine(cSendRecievePlayerData.HandleReadInUnitCatalogue());
	            //Bring in the BeginnersCollection.txt
	            yield return StartCoroutine(cSendRecievePlayerData.HandleReadInBeginnersCollectionData());



	            //soft login
	            StartCoroutine(SignInAndLaunchMainMenu());
	        }
	        else
	        {
	            label = s;
	        }
	    }
        else
        {
            label = "Please fill out Username and Password";
        }

	}

	//Simply removes the Login Window by setting the Player as logged in, and activates the main menu
	//is sgared logic between the HandleLogin and HandleSignup methods
	public IEnumerator SignInAndLaunchMainMenu()
	{
		//read in the map data to be made by the server on request
	yield return 	StartCoroutine(cSendRecievePlayerData.DownloadMapMetaData ());
    yield return StartCoroutine(cSendRecievePlayerData.DownloadRaceData());


		cNetworkManager.bLoggedIn = true;
		goMainMenu.SetActive(true);
	}
}
