using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathFinding : MonoBehaviour
{
    static GridPathFinding _instance;
    public static GridPathFinding GetPathFinding {  get { return _instance; } }
    [SerializeField] private float transformY_Height;
    public static float TransformY { get { return _instance.transformY_Height; } }

    [SerializeField] Vector2Int startCoords;
    [SerializeField] Vector2Int targetCoords;

    public Vector2Int StartCoords { get { return startCoords; } }
    public Vector2Int TargetCoords { get { return targetCoords; } }


    GridNode startNode;
    GridNode targetNode;
    GridNode currentNode;

    Queue<GridNode> bestpath = new Queue<GridNode>();
    Dictionary<Vector2Int, GridNode> reached = new();

    GridManager gridManager;
    Dictionary<Vector2Int, GridNode> grid = new();

    Vector2Int[] searchOrder = { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down };

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        gridManager = GridManager.GetGridManager;
        if ( gridManager ) {
            grid = gridManager.Grid;
        } else {
            throw new System.Exception( "Couldnt get GridManager!! Failed to Start `GridPathFinding`" );
        }
    }

    public List<GridNode> GetNewPath() {
        return GetNewPath( startCoords );
    }

    public List<GridNode> GetNewPath( Vector2Int coords ) {
        gridManager.ResetNodes();
        BreadthFirstSearch(coords);
        return BuildPath();
    }

    private void BreadthFirstSearch( Vector2Int coords ) {
        startNode.Walkable = true;
        targetNode.Walkable = true;
        bestpath.Clear();
        reached.Clear();
        bool isRunning = true;

        bestpath.Enqueue( grid[coords] );
        reached.Add(coords, grid[coords]);

        while ( bestpath.Count > 0 && isRunning ) { 
            currentNode = bestpath.Dequeue();
            currentNode.Explored = true;
            ExploreAdjacent();
            if(currentNode.Coords == targetCoords ) {
                isRunning = false;
                currentNode.Walkable = false;
            }
        }
    }

    private void ExploreAdjacent() {
        List<GridNode> adjacents = new();
        
        foreach ( Vector2Int dir in searchOrder ) {
            Vector2Int adjCoords = currentNode.Coords + dir;
            if(grid.ContainsKey(adjCoords) ) {
                adjacents.Add( grid[adjCoords]);
            }
        }

        foreach( GridNode adj in adjacents ) {
            if ( !reached.ContainsKey( adj.Coords ) && adj.Walkable ) {
                adj.ConnectedTo = currentNode;
                reached.Add( adj.Coords, adj );
                bestpath.Enqueue( adj );
            }
        }
    }

    private List<GridNode> BuildPath() {
        List<GridNode> path = new();
        GridNode currentNode = targetNode;
        path.Add( currentNode );
        currentNode.Path = true;

        while ( currentNode.ConnectedTo != null ) {
            currentNode = currentNode.ConnectedTo;
            path.Add( currentNode );
            currentNode.Path = true;
        }

        path.Reverse();
        return path;
    }

    public void NotifyReceievers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

    public void SetNewDestination(Vector2Int startCoords, Vector2Int targetCoords) {
        this.startCoords = startCoords;
        this.targetCoords = targetCoords;
        startNode = grid[this.startCoords];
        targetNode = grid[this.targetCoords];
        GetNewPath();
    }


}
