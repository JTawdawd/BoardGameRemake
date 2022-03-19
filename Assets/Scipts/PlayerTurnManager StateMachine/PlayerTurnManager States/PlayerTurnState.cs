using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTurnState : MonoBehaviour
{

    protected PlayerTurnManager stateMachine;

    public PlayerTurnState(PlayerTurnManager stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual IEnumerator OnEnter()
    {
        yield break;
    }

    public virtual IEnumerator OnClick(RaycastHit hit)
    {
        yield break;
    }

    public virtual IEnumerator OnUpdate()
    {
        yield break;
    }

    public virtual IEnumerator OnExit()
    {
        yield break;
    }
}
