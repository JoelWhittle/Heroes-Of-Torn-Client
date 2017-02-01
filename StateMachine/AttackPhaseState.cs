using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AttackPhaseState : IState
{

    public List<GameObject> AttackableTargets; 
    public string Name()
    {
        return "AttackPhaseState";

    }

    public void Enter()
    {
        GamePlayGUI.Instance.EnableEndTurnButton(true);

        Debug.Log("reached attack phase");
        //Subscribe to inputs

      MouseManager.Instance.MouseOverHexObject += new MouseManager.MouseOverHexObjectHandler(MouseOverHexObject);
        MouseManager.Instance.MouseExitHexObject += new MouseManager.MouseExitHexObjectHandler(MouseExitHexObject);



       MouseManager.Instance.MouseOverAgentObject += new MouseManager.MouseOverAgentObjectHandler(MouseOverAgentObject);
        MouseManager.Instance.MouseExitAgentObject += new MouseManager.MouseExitAgentObjectHandler(MouseExitAgentObject);


        MouseManager.Instance.MouseUpAgentObject += new MouseManager.MouseUpAgentObjectHandler(MouseUpAgentObject);
        MouseManager.Instance.MouseUpHexObject += new MouseManager.MouseUpHexObjectHandler(MouseUpHexObject);

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
     }

    public void UpdateState()
    {

    }


    public void MouseOverHexObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;
        receivedObject.GetComponent<Hex>().h.FlashingOn(Color.blue, Color.red, 1f);
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

    }


    public void MouseUpAgentObject()
    {
        AttackableTargets = GameManager.Instance.CurrentAgent.GetValidAttackableTargets();

        GameObject receivedObject = MouseManager.Instance.ourHitObject;

        Debug.Log("DISTANCE: " + Pathfinding.Instance.GetDistanceAsCrowFlys(GameManager.Instance.CurrentAgent.ParentHex, receivedObject.GetComponent<Agent>().ParentHex).ToString());

        if (receivedObject.GetComponent<Agent>().Owner == Player.Instance.sName)
        {
            Debug.Log("cant attack our player");

            //ToDo Play an invalid move sound or something
        }
        else
        {
            //We clicked on an enemy so check to see if enemy is a valid attackable agent, ie is in range and isnt obstructed

            if (AttackableTargets.Contains(receivedObject))
            {
                Debug.Log("valid target");

                //Valid target, so ask the CurrentAgent to calculate the attack results
            Debug.Log("LOOKING FOR ABILITY BY NAME" + AbilityManager.Instance.FindAbilityByName("Attack").GetName());
                AbilityManager.Instance.FindAbilityByName("Attack")
                    .CastRequest(GameManager.Instance.CurrentAgent, receivedObject.GetComponent<Agent>());

                GameManager.Instance.EndTurn();
            }
            else
            {
                Debug.Log("invalid target");

            }


        }

    }
}
