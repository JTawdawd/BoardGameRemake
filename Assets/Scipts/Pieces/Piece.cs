using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum ArmorType
    {
        light,
        medium,
        heavy
    }

    public float health;
    public float damage;
    public int currentCooldown;

    public int movementRange;
    public int attackRange;
    public ArmorType armorType;
    public int cooldown;

    public Vector2 pos;

    public BoardManager.Team team;

    public Tile currentTile;
    public void Move(Tile targetTile)
    {
        currentTile.occupier = null;
        transform.position = targetTile.transform.position;
        currentTile = targetTile;
        targetTile.occupier = transform.gameObject;
    }
    public void Attack(List<Tile> targetTiles)
    {
        foreach(Tile targetTile in targetTiles)
        {
            if (targetTile.occupier == null)
                return;

            Piece targetPiece = targetTile.occupier.GetComponent<Piece>();

            if (targetPiece == null)
                return;

            targetPiece.health -= damage;
            targetPiece.CheckHealth();

            Debug.Log("Attacking: " + targetTile.occupier);
        }
    }
    public void Rotate(Tile targetTile)
    {
        Vector3 currPos = transform.position;
        Vector3 rotateTowards = targetTile.transform.position;

        // up
        if (currPos.x == rotateTowards.x && currPos.z < rotateTowards.z)
            transform.eulerAngles = new Vector3(0, 0, 0); 
        // down
        else if (currPos.x == rotateTowards.x && currPos.z > rotateTowards.z)
            transform.eulerAngles = new Vector3(0, 180, 0);
        // left
        else if (currPos.x > rotateTowards.x && currPos.z == rotateTowards.z)
            transform.eulerAngles = new Vector3(0, -90, 0);
        // right
        else if (currPos.x < rotateTowards.x && currPos.z == rotateTowards.z)
            transform.eulerAngles = new Vector3(0, 90, 0);          
    }

    public virtual List<Tile> GetAttackTiles(Tile targetTile)
    {
        return new List<Tile> { targetTile };
    }

    public void CheckHealth()
    {
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
