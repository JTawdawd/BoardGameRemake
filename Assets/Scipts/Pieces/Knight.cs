using UnityEngine;

public class Knight : Piece
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
        damage = 50;
        currentCooldown = 0;

        movementRange = 4;
        attackRange = 2;
        armorType = ArmorType.heavy;
        cooldown = 2;
    }

}
