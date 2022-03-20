using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIOption 
{ 
    public AIOption(Piece AIPiece, Piece targetPiece, List<Tile> choiceTiles)
    {
        this.AIPiece = AIPiece;
        this.targetPiece = targetPiece;
        this.choiceTiles = choiceTiles;
    }

    public Piece AIPiece;
    public Piece targetPiece;
    public List<Tile> choiceTiles;
}

public struct AIAction
{
    public AIAction(Piece AIPiece, Piece targetPiece, Tile toMove)
    {
        this.AIPiece = AIPiece;
        this.targetPiece = targetPiece;
        this.toMove = toMove;
    }

    public Piece AIPiece;
    public Piece targetPiece;
    public Tile toMove;
}

public class RandomAI
{
    BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();

    int DistBetweenTiles(Tile tile1, Tile tile2)
    {
        return (int)(Mathf.Abs(tile2.pos.x - tile1.pos.x) + Mathf.Abs(tile2.pos.y - tile1.pos.y));
    }

    public List<AIOption> GenerateAIOptions(List<Piece> teamOnePieces, List<Piece> teamTwoPieces)
    {
        List<AIOption> aIOptions = new List<AIOption>();
        foreach(Piece AIPiece in teamTwoPieces)
        {
            if (AIPiece.currentCooldown > 0)
                continue;

            List<Tile> tiles = new List<Tile>();
            boardManager.ShowTilesMoveableRecAI(AIPiece.currentTile, AIPiece.movementRange, tiles, AIPiece.team);
            foreach(Piece playerPiece in teamOnePieces)
            {
                List<Tile> choiceTiles = new List<Tile>();
                foreach(Tile tile in tiles)
                {
                    if (DistBetweenTiles(playerPiece.currentTile, tile) < AIPiece.attackRange)
                    {
                        choiceTiles.Add(tile);
                    }
                }
                if (choiceTiles.Count > 0)
                {
                    AIOption aIOption = new AIOption(AIPiece, playerPiece, choiceTiles);
                    aIOptions.Add(aIOption);
                }
            }
        }
        return aIOptions;
    }

    public AIAction SelectAIOption(List<AIOption> aIOptions)
    {
        if (aIOptions.Count <= 0)
            return new AIAction();

        AIOption optionChoice = aIOptions[Random.Range(0, aIOptions.Count)];
        Tile tileChoice = optionChoice.choiceTiles[Random.Range(0, optionChoice.choiceTiles.Count)];
        return new AIAction(optionChoice.AIPiece, optionChoice.targetPiece, tileChoice);
    }
}
