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
        stateMachine.turnIndicator.text = "AI Turn";

        BoardManager bm = stateMachine.boardManager;
        AIAction action = randomAI.SelectAIOption(randomAI.GenerateAIOptions(bm.teamOnePieces, bm.teamTwoPieces));

        Piece aIPiece = action.AIPiece;
        Piece targetPiece = action.targetPiece;

        if (aIPiece != null)
        {
            aIPiece.Move(action.toMove, () =>
                {
                    aIPiece.Attack(aIPiece.GetAttackTiles(targetPiece.currentTile), () =>
                        {
                            Debug.Log("AI turn complete");
                            OnExit();
                        }
                    );
                    aIPiece.SetCurrentCooldown();
                }
            );
        }
        else
        {
            Debug.Log("AI did not have any moves");
            OnExit();
        }
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        stateMachine.SetState(new PlayerTurn(stateMachine));
        return base.OnExit();
    }

}

