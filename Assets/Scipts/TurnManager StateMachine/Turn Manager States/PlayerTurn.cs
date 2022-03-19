using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : TurnManagerState
{
    public PlayerTurn(TurnManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator OnEnter()
    {
        stateMachine.playerTurnManager.transform.gameObject.SetActive(true);
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        stateMachine.SetState(new AITurn(stateMachine));
        return base.OnExit();
    }

}
