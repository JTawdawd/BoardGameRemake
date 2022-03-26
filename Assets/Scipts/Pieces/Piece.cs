using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Piece : MonoBehaviour
{
    public enum ArmorType
    {
        light,
        medium,
        heavy
    }

    public BoardManager.PieceType pieceType;

    public abstract float health { get; set; }
    public abstract float damage { get; set; }
    public abstract int currentCooldown { get; set; }

    public abstract int movementRange { get; protected set; }
    public abstract int attackRange { get; protected set; }
    public abstract ArmorType armorType { get; protected set; }
    public abstract int cooldown { get; protected set; }

    public abstract Vector2 pos { get; set; }

    public abstract BoardManager.Team team { get; set; }

    public abstract Tile currentTile { get; set; }

    public abstract Animator animator { get; set; }

    [SerializeField] private int moveSpeed = 5;
    BoardManager boardManager;

    public void SetCurrentCooldown()
    {
        currentCooldown = cooldown;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
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
                }
                OnCompleted?.Invoke();
            }
        ));
    }
    public void Rotate(Tile targetTile)
    {
        Vector3 currPos = transform.position;
        Vector3 rotateTowards = targetTile.transform.position;
        float angle = Mathf.Atan2(rotateTowards.z - currPos.z, rotateTowards.x - currPos.x) * Mathf.Rad2Deg;

        // up
        if (angle >= 45 && angle < 135)
            transform.eulerAngles = new Vector3(0, 0, 0); 
        // down
        else if (angle >= -135 && angle < -45)
            transform.eulerAngles = new Vector3(0, 180, 0);
        // left
        else if (angle >= 135 || angle < -135)
            transform.eulerAngles = new Vector3(0, -90, 0);
        // right
        else if (angle >= -45 && angle < 45)
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

        if (team == BoardManager.Team.TeamOne)
            boardManager.teamOnePieces.Remove(this);
        else if (team == BoardManager.Team.TeamTwo)
            boardManager.teamTwoPieces.Remove(this);

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
            //Debug.Log(currentAnim + " : " + !stateInfo.IsName(currentAnim) + " | " + stateInfo.normalizedTime);
            yield return null;
        }
        if (currentAnim == "Death")
            yield return new WaitForSeconds(2);
        OnCompleted?.Invoke();
    }

}
