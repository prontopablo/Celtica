using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateId{
    ChasePlayer
}
public interface AIState 
{
    AIStateId GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
