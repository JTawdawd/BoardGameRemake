using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Piece
{
    private void Awake()
    {
        health = 100;
        damage = 30;
        currentCooldown = 0;

        movementRange = 5;
        attackRange = 5;
        armorType = ArmorType.light;
        cooldown = 5;
    }

    public override List<Tile> GetAttackTiles(Tile targetTile)
    {
        List<Tile> attackTiles = new List<Tile> { targetTile };

        List<Tile> neighbours = new List<Tile> { targetTile.up, targetTile.down, targetTile.left, targetTile.right };

        foreach (Tile neighbour in neighbours)
        {
            if (neighbour != null)
                attackTiles.Add(neighbour);
        }

        return attackTiles;
    }
}
