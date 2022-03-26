using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPositionStorage : MonoBehaviour
{
    [System.Serializable]
    public struct PieceInfo
    {
        public BoardManager.PieceType pieceType;
        public Vector2 position;

        public PieceInfo(BoardManager.PieceType pieceType, Vector2 position)
        {
            this.pieceType = pieceType;
            this.position = position;
        }
    }

    public List<PieceInfo> pieces;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerPositionsStorage");
        if (objs.Length <= 1)
            DontDestroyOnLoad(transform.gameObject);
        else
            Destroy(this.gameObject);
    }
}
