using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerState : MonoBehaviour
{
    protected TurnManager stateMachine;

    public TurnManagerState(TurnManager stateMachine)
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
