using UnityEngine;
using System.Collections;

public interface   IState
{
    string Name();
    void Enter();
    void Exit();
    void UpdateState();

}
