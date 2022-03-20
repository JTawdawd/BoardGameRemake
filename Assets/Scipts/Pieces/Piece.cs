using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    private Animator animator;

    [SerializeField] private int moveSpeed = 5;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Tile targetTile, Action OnCompleted)
    {
        PathFinder pathFinder = new PathFinder();
        List<TileWrapper> path = pathFinder.AStarAlgorthim(currentTile, targetTile);

        StartCoroutine(MoveAlongPath(path, () =>
            {
                currentTile.occupier = null;
                transform.position = targetTile.transform.position + new Vector3(0, 0.1f, 0);
                currentTile = targetTile;
                targetTile.occupier = transform.gameObject;
                OnCompleted?.Invoke();
            }
        ));
    }

    IEnumerator MoveAlongPath(List<TileWrapper> path, Action OnCompleted)
    {
        animator.SetBool("IsWalking", true);

        foreach(TileWrapper tileWrapper in path)
        {
            Rotate(tileWrapper.tile);
            Vector3 targetPos = tileWrapper.tile.transform.position + new Vector3(0, 0.1f, 0);

            while(transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        animator.SetBool("IsWalking", false);

        OnCompleted?.Invoke();
    }

    public void Attack(List<Tile> targetTiles, Action OnCompleted)
    {

        Rotate(targetTiles[0]);

        animator.SetTrigger("Attack");

        StartCoroutine(IsAnimPlaying("Attack", () =>
            {
                foreach (Tile targetTile in targetTiles)
                {
                    if (targetTile.occupier == null)
                        continue;

                    Piece targetPiece = targetTile.occupier.GetComponent<Piece>();

                    if (targetPiece == null)
                        continue;

                    targetPiece.animator.SetTrigger("Damaged");
                    targetPiece.health -= damage;
                    targetPiece.CheckHealth();

                    Debug.Log("Attacking: " + targetTile.occupier);
                }
                OnCompleted?.Invoke();
            }
        ));
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
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        animator.SetTrigger("Die");

        StartCoroutine(IsAnimPlaying("Death", () =>
            {
                this.currentTile.occupier = null;
                Destroy(this.gameObject);
            }
        ));

        yield return null;
    }

    IEnumerator IsAnimPlaying(string currentAnim, Action OnCompleted)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(currentAnim))
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //Debug.Log(currentAnim + " : " + !stateInfo.IsName(currentAnim) + " | " + stateInfo.normalizedTime);
            yield return null;
        }
        OnCompleted?.Invoke();
    }

}
