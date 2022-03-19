using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Piece
{
    private void Awake()
    {
        health = 100;
        damage = 40;
        currentCooldown = 0;

        movementRange = 6;
        attackRange = 7;
        armorType = ArmorType.medium;
        cooldown = 3;
    }
}
