
using UnityEngine;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
    public List<IState> StatesList = new List<IState>();
    public IState CurrentState;

    public InitGamePlayState initGamePlayState = new InitGamePlayState();
    public IdleState idleState = new IdleState();
    public  NewMoveState newMoveState = new NewMoveState();
    public AttackPhaseState attackPhaseState = new AttackPhaseState();
    public OpponentsTurnState opponentsTurnState = new OpponentsTurnState();

	// Use this for initialization
	void Start () {

        StatesList.Add(idleState);
        StatesList.Add(initGamePlayState);
        StatesList.Add(newMoveState);
        StatesList.Add(attackPhaseState);
        StatesList.Add(opponentsTurnState);
	    CurrentState = idleState;
        CurrentState.Enter();
	    
         
	}


   public  void ChangeState(string StateName)
    {
        IState newState = null;
        foreach (IState state in StatesList)
        {
            if (state.Name() == StateName)
            {
                newState = state;
            }
        }

        CurrentState.Exit();
        if (newState != null)
        {
            CurrentState = newState;
            CurrentState.Enter();
            
        }
    }
	// Update is called once per frame
	void Update () {
	    if (CurrentState != null)
	    {
	        CurrentState.UpdateState();
	    }

	}
}
