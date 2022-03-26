using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    public Button b_move;
    public Button b_attack;
    public Button b_rotate;
    public Button b_end;

    public PlayerTurnManager playerTurnManager;
    public BoardManager boardManager;

    public TurnManagerState state;

    public Text turnIndicator;

    public void SetState(TurnManagerState state)
    {
        this.state = state;
        this.state.OnEnter();
    }

    void Start()
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();

        playerTurnManager = GameObject.Find("PlayerTurnManager").GetComponent<PlayerTurnManager>();

        int choice = Random.Range(1, 3);
        if (choice == 1)
        {
            Debug.Log("state set to player turn");
            SetState(new PlayerTurn(this));
        }
        else
        { 
            Debug.Log("state set to ai turn");
            SetState(new AITurn(this));
        }
    }

    public void EnableButtons()
    {
        b_move.interactable = true;
        b_attack.interactable = true;
        b_rotate.interactable = true;
        b_end.interactable = true;
    }
    public void DisableButtons()
    {
        b_move.interactable = false;
        b_attack.interactable = false;
        b_rotate.interactable = false;
        b_end.interactable = false;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
