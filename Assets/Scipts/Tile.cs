using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public Tile up;
    [SerializeField] public Tile down;
    [SerializeField] public Tile left;
    [SerializeField] public Tile right;

    [SerializeField] public Vector2 pos;

    [SerializeField] public GameObject occupier;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
