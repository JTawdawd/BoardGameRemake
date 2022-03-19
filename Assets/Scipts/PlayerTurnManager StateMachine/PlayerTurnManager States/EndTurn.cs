using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : PlayerTurnState
{

    public EndTurn(PlayerTurnManager stateMachine) : base(stateMachine)
    {
        
    }

    public override IEnumerator OnEnter()
    {
        List<Piece> playerPieces = stateMachine.boardManager.teamOnePieces;
        List<Piece> aiPieces = stateMachine.boardManager.teamTwoPieces;

        foreach(Piece piece in playerPieces)
            if (piece.currentCooldown > 0)
                piece.currentCooldown--;

        foreach (Piece piece in aiPieces)
            if (piece.currentCooldown > 0)
                piece.currentCooldown--;

        if (stateMachine.moved || stateMachine.attacked)
            stateMachine.selectedPiece.currentCooldown = stateMachine.selectedPiece.cooldown;
        stateMachine.selectedPiece = null;
        stateMachine.ClearSelection();

        stateMachine.transform.gameObject.SetActive(false);
        
        OnExit();

        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        stateMachine.playerTurn.OnExit();   
        return base.OnExit();
    }
}