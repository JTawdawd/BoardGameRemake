using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWrapper
{
    public TileWrapper(Tile tile, int g, int h, int f)
    {
        this.tile = tile;
        this.g = g;
        this.h = h;
        this.f = f;
        this.parent = null;
    }

    public Tile tile;
    public int g;
    public int f;
    public int h;
    public TileWrapper parent;
}

public class PathFinder
{
    public List<TileWrapper> AStarAlgorthim(Tile start, Tile end)
    {
        if (start.occupier == null)
            return null;
        BoardManager.Team team = start.occupier.GetComponent<Piece>().team;

        // initialise lists
        List<TileWrapper> openSet = new List<TileWrapper>();
        List<TileWrapper> closedSet = new List<TileWrapper>();

        // add starting node to open list
        openSet.Add(new TileWrapper(start, 0, 0, 0));

        // while the open list is not empty
        while (openSet.Count > 0)
        {
            // find the tile with the smallest f on the open list
            int winner = 0;
            for (int i = 0; i < openSet.Count; i++)
                if (openSet[i].f < openSet[winner].f)
                    winner = i;

            TileWrapper currentTile = openSet[winner];
            if (currentTile.tile == end)
            {
                closedSet.Add(new TileWrapper(end, 0, 0, 0));
                break;
            }

            // remove the winner from the open list
            openSet.Remove(currentTile);

            List<Tile> adjTiles = new List<Tile> { currentTile.tile.up, currentTile.tile.down, currentTile.tile.left, currentTile.tile.right };

            foreach (Tile tile in adjTiles)
            {
                Predicate<TileWrapper> neighborFinder = (TileWrapper t) => { return t.tile == tile; };
                TileWrapper neighbor;
                if (openSet.Find(neighborFinder) == null)
                    neighbor = new TileWrapper(tile, 0, 0, 0);
                else
                    neighbor = openSet.Find(neighborFinder);

                if (neighbor.tile == null || (neighbor.tile.occupier != null && 
                    neighbor.tile.occupier.transform.GetComponent<Piece>().team != team))
                    continue;

                neighbor.g = currentTile.g + 1;
                neighbor.h = CalcH(neighbor.tile, end);

                int tempf = neighbor.g + neighbor.h;

                // if the node is already in open list and its f is lower then temp f skip
                
                if (openSet.Find(neighborFinder) != null && openSet.Find(neighborFinder).f < tempf)
                    continue;

                // if the node is already in the closed list and its f is lower than temp f skip
                if (closedSet.Find(neighborFinder) != null && closedSet.Find(neighborFinder).f < tempf)
                    continue;

                neighbor.f = tempf;
                neighbor.parent = currentTile;
                if (openSet.Find(neighborFinder) == null)
                    openSet.Add(neighbor);
            }
            closedSet.Add(currentTile);
        }

        // if we havent found end return null
        Predicate<TileWrapper> endFinder = (TileWrapper t) => { return t.tile == end; };
        if (closedSet.Find(endFinder) == null)
            return null;

        // create your new path and return
        List<TileWrapper> path = new List<TileWrapper>();
        TileWrapper tempTile = closedSet.Find(endFinder);
        path.Add(tempTile);

        tempTile.parent = closedSet[closedSet.Count - 2];

        while (tempTile.tile != start)
        {
            if (tempTile.parent == null)
                break;
            tempTile = tempTile.parent;
            path.Add(tempTile);
        }
        path.Reverse();
        return path;
    }

    int CalcH(Tile tile, Tile end)
    {

        int dx = (int)Mathf.Abs(tile.pos.x - end.pos.x);
        int dy = (int)Mathf.Abs(tile.pos.y- end.pos.y);

        int D = 1;

        return D * dx + dy;
    }
}
