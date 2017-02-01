using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	public  GameObject ourHitObject;
    public GameObject ourLastHitGameObject;

    public GameObject tstprefab;
    public float FClickRate;
    public float FClickTimer;
    public static MouseManager Instance;

    public event MouseOverHexObjectHandler MouseOverHexObject;
    public delegate void MouseOverHexObjectHandler();
    public event MouseExitHexObjectHandler MouseExitHexObject;
    public delegate void MouseExitHexObjectHandler();


    public event MouseOverAgentObjectHandler MouseOverAgentObject;
    public delegate void MouseOverAgentObjectHandler();
    public event MouseExitAgentObjectHandler MouseExitAgentObject;
    public delegate void MouseExitAgentObjectHandler();


    public event MouseUpAgentObjectHandler MouseUpAgentObject;
    public delegate void MouseUpAgentObjectHandler();
    public event MouseUpHexObjectHandler MouseUpHexObject;
    public delegate void MouseUpHexObjectHandler();

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{

	    if (FClickTimer > 0)
	    {
	        FClickTimer = FClickTimer = - Time.deltaTime;

	        if (FClickTimer < 0)
	        {
	            FClickTimer = 0;
	        }
	    }

		// could also check if game is paused?
		// if main menu is open?
	
	//	Debug.Log( "Mouse Position: " + Input.mousePosition );

		// This only works in orthographic, and only gives us the
		// world position on the same plane as the camera's
		// near clipping play.  (i.e. It's not helpful for our application.)
		//Vector3 worldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		//Debug.Log( "World Point: " + worldPoint );

		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

		RaycastHit hitInfo;

	    if (Physics.Raycast(ray, out hitInfo))
	    {
	        if (hitInfo.collider.gameObject.tag == "Tile")
	        {
	            ourHitObject = hitInfo.collider.transform.parent.gameObject;

	        }
	        if (hitInfo.collider.gameObject.tag == "Agent")
	        {
	            ourHitObject = hitInfo.collider.gameObject;
	        }

	        if (ourHitObject != ourLastHitGameObject)
	        {
	            if (ourLastHitGameObject != null)
	            {
	                switch (ourLastHitGameObject.tag)
	                {
	                    case "Tile":
	                        MouseExit_Hex(ourLastHitGameObject);
	                        break;
                        case "Agent":
                            MouseExit_Agent(ourLastHitGameObject);
	                        break;

	                }
	            }
	            ourLastHitGameObject = ourHitObject;
	     

	            switch (ourHitObject.tag)
	          
	                {
                    case "Tile":
                       
                        MouseOver_Hex(ourHitObject);
	                        break;

                    case "Agent":
                        MouseOver_Agent(ourHitObject);
	                        break;
	                }
	        
	           
	        }

	      // Debug.Log("Hover over: " + ourHitObject.name);





	    }
	    else
	    {
	
	        ourHitObject = null;
	        if (ourLastHitGameObject!=null)
	        {
	            switch (ourLastHitGameObject.tag)
	            {
                    case "Tile":
                        MouseExit_Hex(ourLastHitGameObject);
	                    break;
                    case "Agent":
                        MouseExit_Agent(ourLastHitGameObject);
	                    break;
	            }
	            ourLastHitGameObject = null;
	        }
	    }

	    if (Input.GetMouseButtonUp(0) && FClickTimer == 0)
	    {
	        FClickTimer = FClickRate;
	        if (ourHitObject != null)
	        {
	            if (ourHitObject.tag == "Tile")
	            {
	                Mouse0Up_Hex(ourHitObject);
	            }
	            else
	            {
	                if (ourHitObject.tag == "Agent")
	                {
	                    Mouse0Up_Agent();
	                }
	            }
	        }
	    }
      

	}

	void MouseOver_Hex(GameObject GoHex)
	{
	    if (GoHex != null)
	    {
	        if (MouseOverHexObject != null)
	        {
	            MouseOverHexObject();
	        }
	    }
	}

    public void Mouse0Up_Hex(GameObject GoHex)
    {
        if (GoHex != null)
        {
            if (MouseUpHexObject != null)
            {
                MouseUpHexObject();
            }
        }
    }
    public void Mouse0Up_Agent()
    {

        if (MouseUpAgentObject != null)
        {
            MouseUpAgentObject();
        }
    }
 

    void MouseExit_Hex(GameObject GoHex)
    {
        if (GoHex != null)
        {
            if(MouseExitHexObject != null)
            {

                MouseExitHexObject();
            }
        }
    }


    void MouseOver_Agent(GameObject GoAgent)
    {
        if (GoAgent != null)
        {
            if (MouseOverAgentObject != null)
            {
                MouseOverAgentObject();
            }
        }
    }

    void MouseExit_Agent(GameObject GoAgent)
    {
        if (GoAgent != null)
        {
          if  (MouseExitAgentObject!= null)
            {

                MouseExitAgentObject();
            }
        }

    }

}
