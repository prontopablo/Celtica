using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId intitialState;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AIStateMachine(this);  
        stateMachine.ChangeState(intitialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
