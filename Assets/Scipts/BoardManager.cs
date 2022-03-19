using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public int boardSize;
    [SerializeField] public int cullDepth;

    [SerializeField] public Material mat_default;
    [SerializeField] public Material mat_moveable;
    [SerializeField] public Material mat_attackable;
    [SerializeField] public Material mat_rotatable;
    [SerializeField] public Material mat_selected;
    [SerializeField] public Material mat_selectedAttack;

    [SerializeField] public Material mat_teamOne;
    [SerializeField] public Material mat_teamTwo;

    [SerializeField] public GameObject archer;
    [SerializeField] public GameObject knight;
    [SerializeField] public GameObject mage;


    [SerializeField] public List<Piece> teamOnePieces;
    [SerializeField] public List<Piece> teamTwoPieces;

    enum PieceType
    {
        Archer,
        Knight,
        Mage
    }

    public enum Team
    {
        TeamOne,
        TeamTwo
    }

    // Start is called before the first frame update
    void Start()
    {
        BoardGeneration(boardSize);
        LinkTiles();
        CullEdges(cullDepth);

        //UpdateTileMaterialRec(GetTileByPos(new Vector2(4, 4)), 4, mat_moveable);
        //List<Tile> tiles = new List<Tile> { GetTileByPos(new Vector2(4,4)), GetTileByPos(new Vector2(3, 4)), GetTileByPos(new Vector2(5, 4)) };
        //UpdateTileMaterial(tiles, mat_attackable);

        AddPiece(PieceType.Knight, new Vector2(4,4), Team.TeamOne);
        AddPiece(PieceType.Knight, new Vector2(6, 4), Team.TeamOne);
        AddPiece(PieceType.Knight, new Vector2(8, 4), Team.TeamOne);

        AddPiece(PieceType.Archer, new Vector2(3, 2), Team.TeamOne);
        AddPiece(PieceType.Archer, new Vector2(9, 2), Team.TeamOne);

        AddPiece(PieceType.Mage, new Vector2(6, 1), Team.TeamOne);


        AddPiece(PieceType.Knight, new Vector2(4, 9), Team.TeamTwo);
        AddPiece(PieceType.Knight, new Vector2(6, 9), Team.TeamTwo);
        AddPiece(PieceType.Knight, new Vector2(8, 9), Team.TeamTwo);

        AddPiece(PieceType.Archer, new Vector2(9, 10), Team.TeamTwo);
        AddPiece(PieceType.Archer, new Vector2(3, 10), Team.TeamTwo);

        AddPiece(PieceType.Mage, new Vector2(6, 11), Team.TeamTwo);
    }

    #region generation

    void BoardGeneration(int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i * 2.1f - size, 0, j * 2.1f - size), Quaternion.identity, this.gameObject.transform);
                tile.name = "tile(" + i + ", " + j + ")";
                Tile t = tile.GetComponent<Tile>();
                t.pos = new Vector2(i, j);
            }
        }
    }

    void LinkTiles()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            Tile tile = child.GetComponent<Tile>();

            if (tile.pos.y + 1 < boardSize)
                tile.up = GetTileByPos(new Vector2(tile.pos.x, tile.pos.y + 1));
            if (tile.pos.y - 1 >= 0)
                tile.down = GetTileByPos(new Vector2(tile.pos.x, tile.pos.y - 1));
            if (tile.pos.x + 1 < boardSize)
                tile.right = GetTileByPos(new Vector2(tile.pos.x + 1, tile.pos.y));
            if (tile.pos.x - 1 >= 0)
                tile.left = GetTileByPos(new Vector2(tile.pos.x - 1, tile.pos.y));
        }
    }

    void CullEdges(int cullDepth)
    {
        Tile bottomLeft = GetTileByPos(new Vector2(0, 0));
        Tile bottomRight = GetTileByPos(new Vector2(0, boardSize-1));
        Tile topLeft = GetTileByPos(new Vector2(boardSize-1, 0));
        Tile topRight = GetTileByPos(new Vector2(boardSize-1, boardSize-1));

        CullEdgesRec(bottomLeft, cullDepth);
        CullEdgesRec(bottomRight, cullDepth);
        CullEdgesRec(topLeft, cullDepth);
        CullEdgesRec(topRight, cullDepth);
    }

    void CullEdgesRec(Tile currTile, int cullDepth)
    {
        // base case
        if (cullDepth <= 0 || currTile == null) 
            return;

        CullEdgesRec(currTile.up, cullDepth - 1);
        CullEdgesRec(currTile.down, cullDepth - 1);
        CullEdgesRec(currTile.left, cullDepth - 1);
        CullEdgesRec(currTile.right, cullDepth - 1);

        Destroy(currTile.gameObject);      
    }

    #endregion

    #region material_changes

    public void ShowTilesMoveableRec(Tile origin, int range, List<Tile> selectedTiles, Team team)
    {
        // base case
        if (range <= 0)
            return;

        List<Tile> adjTiles = new List<Tile> { origin.up, origin.down, origin.left, origin.right };

        foreach (Tile tile in adjTiles)
            if (tile != null && (tile.occupier == null || (tile.occupier != null && tile.occupier.GetComponent<Piece>().team == team)))
                ShowTilesMoveableRec(tile, range - 1, selectedTiles, team);

        if (origin.occupier == null)
        {
            origin.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_moveable;
            selectedTiles.Add(origin);
        }
    }

    public void ShowTilesAttackableRec(Tile origin, int range, List<Tile> selectedTiles, Team team)
    {
        // base case
        if (range <= 0)
            return;

        if (origin.up != null)
            ShowTilesAttackableRec(origin.up, range - 1, selectedTiles, team);
        if (origin.down != null)
            ShowTilesAttackableRec(origin.down, range - 1, selectedTiles, team);
        if (origin.left != null)
            ShowTilesAttackableRec(origin.left, range - 1, selectedTiles, team);
        if (origin.right != null)
            ShowTilesAttackableRec(origin.right, range - 1, selectedTiles, team);

        if (origin.occupier == null || (origin.occupier != null && origin.occupier.GetComponent<Piece>().team != team))
        {
            origin.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_attackable;
            selectedTiles.Add(origin);
        }
    }

    public void ShowTilesRotatableRec(Tile origin, int range, List<Tile> selectedTiles)
    {
        // base case
        if (range <= 0)
            return;

        if (origin.up != null)
            ShowTilesRotatableRec(origin.up, range - 1, selectedTiles);
        if (origin.down != null)
            ShowTilesRotatableRec(origin.down, range - 1, selectedTiles);
        if (origin.left != null)
            ShowTilesRotatableRec(origin.left, range - 1, selectedTiles);
        if (origin.right != null)
            ShowTilesRotatableRec(origin.right, range - 1, selectedTiles);


        origin.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_attackable;
        selectedTiles.Add(origin);
    }

    public void UpdateTileMaterial(List<Tile> tiles, Material material)
    {
        foreach(Tile tile in tiles)
        {
            tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
        }
    }

    public void ShowTilesMoveableRecAI(Tile origin, int range, List<Tile> selectedTiles, Team team)
    {
        // base case
        if (range <= 0)
            return;

        List<Tile> adjTiles = new List<Tile> { origin.up, origin.down, origin.left, origin.right };

        foreach (Tile tile in adjTiles)
            if (tile != null && (tile.occupier == null || (tile.occupier != null && tile.occupier.GetComponent<Piece>().team == team)))
                ShowTilesMoveableRecAI(tile, range - 1, selectedTiles, team);

        if (origin.occupier == null)
        {
            selectedTiles.Add(origin);
        }
    }

    #endregion 

    void AddPiece(PieceType pieceType, Vector2 position, Team team)
    {
        Tile tile = GetTileByPos(position);
        GameObject temp;

        Quaternion rotation;
        if (team == Team.TeamOne)
            rotation = Quaternion.identity;
        else
            rotation = Quaternion.Euler(0, 180, 0);

        switch(pieceType)
        {
            default:
                temp = Instantiate(archer, tile.transform.position, rotation);
                break;
            case PieceType.Knight:
                temp = Instantiate(knight, tile.transform.position, rotation);
                break;
            case PieceType.Mage:
                temp = Instantiate(mage, tile.transform.position, rotation);
                break;
        }

        tile.occupier = temp;
        Piece piece = temp.GetComponent<Piece>();
        piece.currentTile = tile;
        piece.team = team;
        piece.pos = position;

        if (team == Team.TeamOne)
        {
            temp.GetComponentInChildren<SkinnedMeshRenderer>().material = mat_teamOne;
            teamOnePieces.Add(piece);
        }
        else
        {
            temp.GetComponentInChildren<SkinnedMeshRenderer>().material = mat_teamTwo;
            teamTwoPieces.Add(piece);
        }
            
    }

    public Tile GetTileByPos(Vector2 pos)
    {
        return GameObject.Find("tile(" + pos.x + ", " + pos.y + ")").GetComponent<Tile>();
    }

}
