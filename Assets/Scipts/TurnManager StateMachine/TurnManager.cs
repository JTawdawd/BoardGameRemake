using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public PlayerTurnManager playerTurnManager;
    public BoardManager boardManager;

    public TurnManagerState state;
    public void SetState(TurnManagerState state)
    {
        this.state = state;
        this.state.OnEnter();
    }

    void Start()
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();

        playerTurnManager = GameObject.Find("PlayerTurnManager").GetComponent<PlayerTurnManager>();
        playerTurnManager.transform.gameObject.SetActive(false);

        int choice = Random.Range(1, 3);
        if (choice == 1)
        {
            Debug.Log("state set to player turn");
            SetState(new PlayerTurn(this));
        }
        if (choice == 2)
        { 
            Debug.Log("state set to ai turn");
            SetState(new AITurn(this));
        }
    }
}
