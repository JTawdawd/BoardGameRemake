using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BoardSetUpManager : MonoBehaviour
{
    public NewControls newControls;

    [SerializeField] bool IsCharacterAttached = false;
    [SerializeField] GameObject CharacterAttached = null;
    [SerializeField] BoardManager.PieceType pieceType;

    BoardManager boardManager;
    [SerializeField] PlayerPositionStorage posStorage;

    [SerializeField] GameObject knight;
    [SerializeField] GameObject archer;
    [SerializeField] GameObject mage;

    [SerializeField] GameObject selectableKnight;
    [SerializeField] GameObject selectableArcher;
    [SerializeField] GameObject selectableMage;

    [SerializeField] int maxUsagePoints = 25;
    [SerializeField] int currentUsagePoints = 0;

    private void Awake()
    {
        boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
        posStorage = GameObject.Find("PlayerPositionsStorage").GetComponent<PlayerPositionStorage>();

        newControls = new NewControls();
        newControls.SetUpSelection.Select.started += context => StartSelection();
        newControls.SetUpSelection.Select.canceled += context => EndSelection();

        newControls.SetUpSelection.Remove.performed += context => Remove();
    }

    private void Update()
    {
        if(IsCharacterAttached && CharacterAttached != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            float mag = (0 - mousePosition.y) / ray.direction.y;

            mousePosition += ray.direction * mag;

            CharacterAttached.transform.position = mousePosition;
        }
    }

    void OnEnable()
    {
        newControls.SetUpSelection.Enable();
    }

    void OnDisable()
    {
        newControls.SetUpSelection.Disable();
    }

    void StartSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform == null)
            return;

        Piece piece = hit.transform.GetComponentInParent<Piece>();
        if (piece == null)
            return;

        IsCharacterAttached = true;

        if (piece.GetType() == typeof(Archer))
        {
            selectableArcher.GetComponent<Animator>().SetTrigger("Selected");
            pieceType = BoardManager.PieceType.Archer;
            CharacterAttached = Instantiate(archer, new Vector3(0, 0, 0), Quaternion.identity);
            CharacterAttached.GetComponentInChildren<MeshCollider>().enabled = false;
        }
        else if (piece.GetType() == typeof(Knight))
        {
            selectableKnight.GetComponent<Animator>().SetTrigger("Selected");
            pieceType = BoardManager.PieceType.Knight;
            CharacterAttached = Instantiate(knight, new Vector3(0, 0, 0), Quaternion.identity);
            CharacterAttached.GetComponentInChildren<CapsuleCollider>().enabled = false;
        }
        else
        {
            selectableMage.GetComponent<Animator>().SetTrigger("Selected");
            pieceType = BoardManager.PieceType.Mage;
            CharacterAttached = Instantiate(mage, new Vector3(0, 0, 0), Quaternion.identity);
            CharacterAttached.GetComponentInChildren<MeshCollider>().enabled = false;
        }
    }

    void EndSelection()
    {
        IsCharacterAttached = false;
        Destroy(CharacterAttached);

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform == null)
            return;

        Tile tile = hit.transform.GetComponentInParent<Tile>();
        if (tile == null)
            return;

        if (tile.occupier != null)
            return;

        int pointsToAdd;
        switch (pieceType)
        {
            case BoardManager.PieceType.Knight:
                pointsToAdd = 3;
                break;
            case BoardManager.PieceType.Mage:
                pointsToAdd = 5;
                break;
            default:
                pointsToAdd = 4;
                break;
        }

        if (currentUsagePoints + pointsToAdd > maxUsagePoints)
            return;

        currentUsagePoints += pointsToAdd;
        Piece piece = boardManager.AddPiece(pieceType, tile.pos, BoardManager.Team.TeamOne);
        posStorage.pieces.Add(new PlayerPositionStorage.PieceInfo(piece.pieceType, piece.pos));
    }

    void Remove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform == null)
            return;

        Piece piece = hit.transform.GetComponentInParent<Piece>();
        if (piece == null)
            return;

        if (hit.transform.tag == "SelectionPiece")
            return;

        if (piece.GetType() == typeof(Archer))
        {
            currentUsagePoints -= 4;
        }
        else if (piece.GetType() == typeof(Knight))
        {
            currentUsagePoints -= 3;
        }
        else if (piece.GetType() == typeof(Mage))
        {
            currentUsagePoints -= 5;
        }

        piece.currentTile.occupier = null;
        posStorage.pieces.RemoveAll(p => p.position == piece.pos);
        Destroy(piece.transform.gameObject);
    }

    public void Confirm()
    {   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
