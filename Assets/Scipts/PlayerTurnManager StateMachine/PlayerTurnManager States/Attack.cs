using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : PlayerTurnState
{
    List<Tile> selectedAttackTiles;
    
    public Attack(PlayerTurnManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator OnEnter()
    {
        stateMachine.inputDisabled = false;
        selectedAttackTiles = new List<Tile>();
        if (stateMachine.selectedPiece != null)
        {
            BoardManager bm = stateMachine.boardManager;
            Piece sp = stateMachine.selectedPiece;
            stateMachine.boardManager.ShowTilesAttackableRec(sp.currentTile, sp.attackRange, stateMachine.selectedTiles, sp.team);
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
        if (piece != null && piece == stateMachine.selectedPiece)
            return base.OnClick(hit);

        if (piece != null && piece != stateMachine.selectedPiece && !stateMachine.moved && piece.currentCooldown <= 0 && piece.team == BoardManager.Team.TeamOne)
        {
            selectedAttackTiles.Clear();

            Piece hitPiece = hit.transform.parent.transform.GetComponent<Piece>();
            BoardManager bm = stateMachine.boardManager;

            stateMachine.ClearSelection();
            bm.ShowTilesAttackableRec(hitPiece.currentTile, hitPiece.attackRange, stateMachine.selectedTiles, hitPiece.team);

            stateMachine.selectedPiece = hitPiece;
        }
        else
        {
            if (hit.transform.GetComponent<Tile>() == null && piece == null)
                return base.OnClick(hit);

            Tile hitTile;

            if (hit.transform.GetComponent<Tile>() != null)
                hitTile = hit.transform.GetComponent<Tile>();
            else
                hitTile = piece.currentTile;

            if (selectedAttackTiles.Contains(hitTile))
            {
                Debug.Log("Confirmed Selection");
                stateMachine.inputDisabled = true;

                stateMachine.selectedTiles.AddRange(selectedAttackTiles);
                OnExit();

                stateMachine.selectedPiece.Attack(selectedAttackTiles, () =>
                    {
                        stateMachine.attacked = true;

                        if (stateMachine.moved)
                            stateMachine.SetState(new Rotate(stateMachine));
                        else
                            stateMachine.SetState(new Move(stateMachine));
                    }
                );

                return base.OnClick(hit);
            }

            if (!stateMachine.selectedTiles.Contains(hitTile))
                return base.OnClick(hit);

            if (selectedAttackTiles.Count > 0)
            {
                List<Tile> toRemove = new List<Tile>();
                foreach(Tile selectedAttackTile in selectedAttackTiles)
                {
                    if (!stateMachine.selectedTiles.Contains(selectedAttackTile))
                    {
                        toRemove.Add(selectedAttackTile);
                    }
                }

                selectedAttackTiles.RemoveAll(item => toRemove.Contains(item));

                stateMachine.boardManager.UpdateTileMaterial(toRemove, stateMachine.boardManager.mat_default);

                stateMachine.boardManager.UpdateTileMaterial(selectedAttackTiles, stateMachine.boardManager.mat_attackable);
            }
                

            selectedAttackTiles = stateMachine.selectedPiece.GetAttackTiles(hitTile);
            stateMachine.boardManager.UpdateTileMaterial(selectedAttackTiles, stateMachine.boardManager.mat_selectedAttack);
        }
        return base.OnClick(hit);
    }

    public override IEnumerator OnExit()
    {
        stateMachine.ClearSelection();
        return base.OnExit();
    }
}
