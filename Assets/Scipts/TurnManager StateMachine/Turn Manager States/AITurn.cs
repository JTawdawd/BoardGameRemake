using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : TurnManagerState
{

    RandomAI randomAI = new RandomAI();

    public AITurn(TurnManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator OnEnter()
    {
        BoardManager bm = stateMachine.boardManager;
        AIAction action = randomAI.SelectAIOption(randomAI.GenerateAIOptions(bm.teamOnePieces, bm.teamTwoPieces));

        Piece aIPiece = action.AIPiece;
        Piece targetPiece = action.targetPiece;

        if (aIPiece != null)
        {
            aIPiece.Move(action.toMove, () =>
                {
                    aIPiece.Attack(aIPiece.GetAttackTiles(targetPiece.currentTile), null);
                    aIPiece.currentCooldown = aIPiece.cooldown;
                }
            );
        }
        
        Debug.Log("AI turn complete");
        OnExit();
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        stateMachine.SetState(new PlayerTurn(stateMachine));
        return base.OnExit();
    }

}

