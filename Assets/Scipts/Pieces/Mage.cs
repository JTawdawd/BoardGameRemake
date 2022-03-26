using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Piece
{
    public override float health { get; set; }
    public override int movementRange { get; protected set; }
    public override int attackRange { get; protected set; }
    public override ArmorType armorType { get; protected set; }
    public override int cooldown { get; protected set; }
    public override float damage { get; set; }
    public override int currentCooldown { get; set; }
    public override Vector2 pos { get; set; }
    public override BoardManager.Team team { get; set; }
    public override Tile currentTile { get; set; }
    public override Animator animator { get; set; }

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
