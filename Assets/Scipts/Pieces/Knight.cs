using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
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
