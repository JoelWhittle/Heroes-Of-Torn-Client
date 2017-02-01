using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MidGameMessageBoxGUI : MonoBehaviour
{

    public static  MidGameMessageBoxGUI Instance;
    public bool bActive = false;
    public string Message = "";

    public GameObject NewTurnMessageBox;
    public GameObject NewTurnMessageBoxTextObject;
    public Color[] Colours;

	// Use this for initialization
	void Start ()
	{

	    Instance = this;

	}
	


    public IEnumerator StartTimer(float t)
    {
        yield return new WaitForSeconds(t);
        bActive = false;
    }

    void OnGUI()
    {
        if (bActive)
        {
            GUI.Box(new Rect(StaticUtils.scrnW(50), StaticUtils.scrnH(50), 100, 50), Message);
 
        }
    }

    public void DisplayMessage(string message, float timer)
    {

        Message = message;
        bActive = true;
        StartCoroutine(StartTimer(timer));
    }

    public void DisplayNewTurnMessage(Agent agent)
    {
        StartCoroutine(HandleNewTurnMessage(agent));
    }

    public IEnumerator HandleNewTurnMessage(Agent currentAgent)
    {
        string message = "";
        float t = 2f;
        Color color = Color.red;
        if (currentAgent.Owner == Player.Instance.sName)
        {
            message = "Your Turn!";
            color = Colours[1];
        }
        else
        {
            message = "Opponents Turn!";
            color = Colours[0];

        }
        NewTurnMessageBox.SetActive(true);
        NewTurnMessageBox.GetComponent<Image>().color = color;
        NewTurnMessageBoxTextObject.GetComponent<Text>().text = message;

        yield return new WaitForSeconds(t);
        NewTurnMessageBox.SetActive(false);
    }
}
