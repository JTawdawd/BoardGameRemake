using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : PlayerTurnState
{

    Tile selectedTileRotate;

    public Rotate(PlayerTurnManager stateMachine) : base(stateMachine)
    {
        
    }

    public override IEnumerator OnEnter()
    {
        if (stateMachine.selectedPiece != null)
        {
            BoardManager bm = stateMachine.boardManager;
            Piece sp = stateMachine.selectedPiece;
            bm.ShowTilesRotatableRec(sp.currentTile, 2, stateMachine.selectedTiles);
;
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
        if (piece != null && piece != stateMachine.selectedPiece && (!stateMachine.moved && !stateMachine.attacked) && piece.currentCooldown <= 0)
        {
            selectedTileRotate = null;

            Piece hitPiece = hit.transform.parent.transform.GetComponent<Piece>();
            BoardManager bm = stateMachine.boardManager;

            stateMachine.ClearSelection();
            bm.ShowTilesRotatableRec(hitPiece.currentTile, 2, stateMachine.selectedTiles);

            stateMachine.selectedPiece = hitPiece;
        }
        else
        {
            if (hit.transform.GetComponent<Tile>() == null)
                return base.OnClick(hit);

            Tile hitTile = hit.transform.GetComponent<Tile>();

            if (hitTile == selectedTileRotate)
            {
                Debug.Log("Confirmed Selection");
                stateMachine.selectedPiece.Rotate(selectedTileRotate);
                OnExit();
                stateMachine.SetState(new EndTurn(stateMachine));
                return base.OnClick(hit);
            }

            if (!stateMachine.selectedTiles.Contains(hitTile))
                return base.OnClick(hit);

            if (selectedTileRotate != null)
                stateMachine.boardManager.UpdateTileMaterial(new List<Tile> { selectedTileRotate }, stateMachine.boardManager.mat_attackable);

            stateMachine.boardManager.UpdateTileMaterial(new List<Tile> { hitTile }, stateMachine.boardManager.mat_selectedAttack);
            selectedTileRotate = hitTile;
        }
        return base.OnClick(hit);
    }

    public override IEnumerator OnExit()
    {
        stateMachine.ClearSelection();
        return base.OnExit();
    }
}

