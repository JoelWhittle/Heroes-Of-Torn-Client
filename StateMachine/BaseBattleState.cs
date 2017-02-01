using UnityEngine;
using System.Collections;

public class BaseBattleState : IState {

    public string Name()
    {
        return "BaseBattleState";

    }

    public void Enter()
    {
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
     
    }



    public void MouseUpAgentObject()
    {
        GameObject receivedObject = MouseManager.Instance.ourHitObject;

    }
}
