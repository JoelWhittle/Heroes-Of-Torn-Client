using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NewMoveState : IState
{

    public bool IsMyAgent = false;
    private Agent CurrentAgent;
    private List<GameObject> CurrentAgentMovementRadius = new List<GameObject>();

    public string Name()
    {
        return "NewMoveState";

    }



    public void Enter()
    {
        CurrentAgent = GameManager.Instance.CurrentAgent;
        //Display message box
        MidGameMessageBoxGUI.Instance.DisplayNewTurnMessage(CurrentAgent.GetComponent<Agent>());

        //Check that this is our turn, if it isnt, break to oppoenents turn state
        if (GameManager.Instance.CurrentAgent.Owner != Player.Instance.sName)
        {
            GameManager.Instance.GamePlayStateMachine.ChangeState("OpponentsTurnState");


        }
        else
        {
            Init();
        }
    }
    public void Init()
    {

    //Subscribe to inputs

        MouseManager.Instance.MouseOverHexObject += new MouseManager.MouseOverHexObjectHandler(MouseOverHexObject);
        MouseManager.Instance.MouseExitHexObject += new MouseManager.MouseExitHexObjectHandler(MouseExitHexObject);



        MouseManager.Instance.MouseOverAgentObject += new MouseManager.MouseOverAgentObjectHandler(MouseOverAgentObject);
        MouseManager.Instance.MouseExitAgentObject += new MouseManager.MouseExitAgentObjectHandler(MouseExitAgentObject);


        MouseManager.Instance.MouseUpAgentObject += new MouseManager.MouseUpAgentObjectHandler(MouseUpAgentObject);
        MouseManager.Instance.MouseUpHexObject += new MouseManager.MouseUpHexObjectHandler(MouseUpHexObject);


       

            IsMyAgent = true;
            CurrentAgent.Highlighter.ConstantOn(Color.blue);


   

        //highlight movement radius

        CurrentAgentMovementRadius = Pathfinding.Instance.FindMovementRadius(CurrentAgent.ParentHex.gameObject,
            CurrentAgent.GetMovementSpeed());
        foreach (GameObject go in CurrentAgentMovementRadius)
        {
            go.GetComponent<Hex>().h.ConstantOn(Color.blue);
        }

            GamePlayGUI.Instance.EnableEndTurnButton(true);
        GamePlayGUI.Instance.EnableAbilitySelectButton(true);
        
    }



    public void Exit()
    {

        GamePlayGUI.Instance.EnableEndTurnButton(false);

        //unsubscribe from inputs
        MouseManager.Instance.MouseOverHexObject -= new MouseManager.MouseOverHexObjectHandler(MouseOverHexObject);
        MouseManager.Instance.MouseExitHexObject -= new MouseManager.MouseExitHexObjectHandler(MouseExitHexObject);


        MouseManager.Instance.MouseOverAgentObject -= new MouseManager.MouseOverAgentObjectHandler(MouseOverAgentObject);
        MouseManager.Instance.MouseExitAgentObject -= new MouseManager.MouseExitAgentObjectHandler(MouseExitAgentObject);


        MouseManager.Instance.MouseUpAgentObject -= new MouseManager.MouseUpAgentObjectHandler(MouseUpAgentObject);
        MouseManager.Instance.MouseUpHexObject -= new MouseManager.MouseUpHexObjectHandler(MouseUpHexObject);

        //reset state for next use
        IsMyAgent = false;
        //stop highlighting
        CurrentAgent.Highlighter.ConstantOff();
        foreach (GameObject go in CurrentAgentMovementRadius)
        {
            go.GetComponent<Hex>().h.ConstantOff();
        }
        CurrentAgentMovementRadius.Clear();


    }

        public void UpdateState()
    {
                 
   
    }

    public void MouseOverHexObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;
receivedObject.GetComponent<Hex>().h.FlashingOn(Color.blue, Color.red , 1f);
    }

    public void MouseExitHexObject()
    {
       GameObject receivedObject = MouseManager.Instance.ourLastHitGameObject;

    
            receivedObject.GetComponent<Hex>().h.FlashingOff();
    }

    public void MouseOverAgentObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;
        if (receivedObject.GetComponent<Agent>().Owner == Player.Instance.sName)
        {
            receivedObject.GetComponent<Agent>().Highlighter.FlashingOn(Color.blue, Color.clear, 1f);
        }
        else
        {
            receivedObject.GetComponent<Agent>().Highlighter.FlashingOn(Color.red, Color.clear, 1f);

        }
    }

    public void MouseExitAgentObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourLastHitGameObject;


        receivedObject.GetComponent<Agent>().Highlighter.FlashingOff();
    }

    public void MouseUpHexObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;

        if (CurrentAgent.Owner == Player.Instance.sName)
        {
            if (CurrentAgentMovementRadius.Contains(receivedObject))

            {
                Debug.Log("i can move");
                if (receivedObject== CurrentAgent.ParentHex)
                {
                    GameManager.Instance.GamePlayStateMachine.ChangeState("AttackPhaseState");
                }
                else
                {
                    List<GameObject> pathList = Pathfinding.Instance.FindNEWPath(CurrentAgent.ParentHex, receivedObject);
                    ExchangeGameData.Instance.SendAgentMoveRequest(CurrentAgent.ID, pathList);
                    Agent a = GameManager.Instance.CalculateNextAgent();
                    GameManager.Instance.GamePlayStateMachine.ChangeState("AttackPhaseState");

                }
           
            }
            else
            {
                Debug.Log("too far away");

            }
        }
        else
        {
            Debug.Log("not my turn");
        }

    }

   

    public void MouseUpAgentObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;

        
        if (receivedObject.GetComponent<Agent>().Owner == Player.Instance.sName)
        {
            //ToDo Play an invalid move sound or something
        }
        else
        {
            //We clicked on an enemy so check to see if enemy is a valid attackable agent, ie is in range and isnt obstructed

            if (GameManager.Instance.CurrentAgent.GetValidAttackableTargets().Contains(receivedObject))
            {
                //Valid target, so ask the CurrentAgent to calculate the attack results
                AbilityManager.Instance.FindAbilityByName("Attack")
                        .CastRequest(GameManager.Instance.CurrentAgent, receivedObject.GetComponent<Agent>());


                GameManager.Instance.EndTurn();
            }


        }

    }

    

}

