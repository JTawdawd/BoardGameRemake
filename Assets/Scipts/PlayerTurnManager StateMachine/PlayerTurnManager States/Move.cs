using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : PlayerTurnState
{

    Tile selectedTile;

    public Move(PlayerTurnManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator OnEnter()
    {
        if (stateMachine.selectedPiece != null)
        {
            BoardManager bm = stateMachine.boardManager;
            Piece sp = stateMachine.selectedPiece;
            stateMachine.boardManager.ShowTilesMoveableRec(sp.currentTile, sp.movementRange, stateMachine.selectedTiles, sp.team);
        }
        return base.OnEnter();
    }

    public override IEnumerator OnUpdate()
    {
        return base.OnUpdate();
    }

    public override IEnumerator OnClick(RaycastHit hit)
    {
        Piece piece = hit.transform.parent.transform.GetComponent<Piece>();
        if (piece != null && piece != stateMachine.selectedPiece && !stateMachine.attacked && piece.currentCooldown <= 0)
        {
            selectedTile = null;

            Piece hitPiece = hit.transform.parent.transform.GetComponent<Piece>();
            BoardManager bm = stateMachine.boardManager;

            stateMachine.ClearSelection();
            bm.ShowTilesMoveableRec(hitPiece.currentTile, hitPiece.movementRange, stateMachine.selectedTiles, hitPiece.team);

            stateMachine.selectedPiece = hitPiece;
        }
        else
        {
            if (hit.transform.GetComponent<Tile>() == null)
                return base.OnClick(hit);

            Tile hitTile = hit.transform.GetComponent<Tile>();

            if (hitTile == selectedTile)
            {
                Debug.Log("Confirmed Selection");
                stateMachine.selectedPiece.Move(selectedTile);
                stateMachine.moved = true;
                OnExit();
                if (stateMachine.attacked)
                    stateMachine.SetState(new Rotate(stateMachine));
                else
                    stateMachine.SetState(new Attack(stateMachine));

                return base.OnClick(hit);
            }
                
            if (!stateMachine.selectedTiles.Contains(hitTile))
                return base.OnClick(hit);

            if (selectedTile != null)
                stateMachine.boardManager.UpdateTileMaterial(new List<Tile> { selectedTile }, stateMachine.boardManager.mat_moveable);

            stateMachine.boardManager.UpdateTileMaterial(new List<Tile> { hitTile }, stateMachine.boardManager.mat_selected);
            selectedTile = hitTile;
        }

        return base.OnClick(hit);
    }

    public override IEnumerator OnExit()
    {
        stateMachine.ClearSelection();
        return base.OnExit();
    }
}

