using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridNode
{
    private Vector2Int coords;

    private bool explored;
    private bool walkable;
    private bool path;
    private GridNode connectedTo;
    private WorldTile correspondingTile;

    public Vector2Int Coords { get => coords; }
    public WorldTile GetWorldTile { get => correspondingTile; }

    public bool Walkable { get => walkable; set => walkable = value; }
    public bool Explored { get => explored; set => explored =  value ; }
    public bool Path { get => path; set => path =  value ; }
    public GridNode ConnectedTo { get => connectedTo; set => connectedTo =  value ; }

    public GridNode( Vector2Int coords, bool isWalkable, WorldTile correspondingTile) { 
        this.coords = coords; 
        this.walkable = isWalkable;
        this.correspondingTile = correspondingTile;
    }

}
