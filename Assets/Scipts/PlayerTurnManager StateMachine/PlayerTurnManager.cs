using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerTurnManager : MonoBehaviour
{
    public NewControls newControls;
    public BoardManager boardManager;

    public Piece selectedPiece;

    public List<Tile> selectedTiles;

    PlayerTurnState state;

    TurnManager turnManager;
    public PlayerTurn playerTurn;

    public bool moved, attacked;

    public Button b_move;
    public Button b_attack;
    public Button b_rotate;
    public Button b_end;

    public bool inputDisabled = false;

    public void SetState(PlayerTurnState state)
    {
        this.state = state;
        this.state.OnEnter();
        if (state.GetType() != typeof(EndTurn))
            UpdateButtons(state);
    }

    void Awake()
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        newControls = new NewControls();

        newControls.Player.Click.performed += context => Click();

        selectedTiles = new List<Tile>();

        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }

    private void Start()
    {
    }

    public void ClearSelection()
    {
        foreach (Tile tile in selectedTiles)
        {
            tile.GetComponentInChildren<MeshRenderer>().material = boardManager.mat_default;
        }
        selectedTiles.Clear();
    }

    void Click()
    {
        //Debug.Log("click!");

        if (inputDisabled)
            return;

        // calculate what has been clicked on and store in hit
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform == null)
            return;

        Piece p = hit.transform.parent.transform.GetComponent<Piece>();
        if (p != null && p.team != BoardManager.Team.TeamOne && state.GetType() != typeof(Attack))
            return;
            
        state.OnClick(hit);
    }

    void OnEnable()
    {
        newControls.Player.Enable();
    }

    void OnDisable()
    {
        newControls.Player.Disable();
    }

    public void StartNewPlayerTurn()
    {
        playerTurn = (PlayerTurn)turnManager.state;

        moved = false;
        attacked = false;

        SetState(new Move(this));
    }

    void UpdateButtons(PlayerTurnState state)
    {
        if (moved)
            b_move.interactable = false;
        else
            b_move.interactable = true;

        if (attacked)
            b_attack.interactable = false;
        else
            b_attack.interactable = true;

        b_rotate.interactable = true;
        b_end.interactable = true;

        if (state is Move)
            b_move.interactable = false;
        else if (state is Attack)
            b_attack.interactable = false;
        else if (state is Rotate)
            b_rotate.interactable = false;
        else if (state is EndTurn)
            b_end.interactable = false;
    }

    public void ChangeToMove()
    {
        state.OnExit();
        SetState(new Move(this));
    }
    public void ChangeToAttack()
    {
        state.OnExit();
        SetState(new Attack(this));
    }
    public void ChangeToRotate()
    {
        state.OnExit();
        SetState(new Rotate(this));
    }
    public void ChangeToEnd()
    {
        state.OnExit();
        SetState(new EndTurn(this));
    }

}
