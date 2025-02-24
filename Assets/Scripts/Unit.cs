using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : Selectable
{
    GridManager gridManager;
    GridPathFinding pathFinder;

    [SerializeField] private float moveSpeed;

    protected WorldTile currentSpot;
    protected WorldTile lastSpot;

    public WorldTile CurrentSpot { get { return currentSpot; } }
    public float MoveSpeed { get { return moveSpeed; } }

    protected int visionRange;
    // TODO: MOVE TO SUBCLASS!!


    // GRID PATH 
    private List<GridNode> path = new();


    protected override void Awake() {
        base.Awake();
        this.selectType = SelectableType.UNIT;
    }

    protected override void Start() {
        base.Start();

        gridManager = GridManager.GetGridManager;
        pathFinder = GridPathFinding.GetPathFinding;
    }

    public override void Highlight() {
        base.Highlight();
    }
    public override void UnHighlight() {
        base.UnHighlight();
    }
    public override void Select() {
        base.Select();
    }
    public override void Deselect() {
        base.Deselect();
    }

    private void _leaveCurrentSpot() {
        lastSpot = currentSpot.Leave();
        currentSpot = null;
    }

    public void SetCurrentSpot( WorldTile worldTile ) {
        if(currentSpot) {
            _leaveCurrentSpot();
        }
        this.currentSpot = worldTile;
    }

    public void MoveTo( WorldTile target ) {

        // Debug.Log( $"MOVING TO ({target.name}), from  SPOT {CurrentSpot.name}" );

        pathFinder.SetNewDestination( this.CurrentSpot.GridCoords, target.GridCoords );
        this.SetCurrentSpot( target.Occupy() );
        this.RecalculatePath( true, this.MoveSpeed );

    }

    //public Vector2Int[] GetAvailableMoves() {
    //    
    // }

    void RecalculatePath( bool resetPath, float speed = 3f ) {
        Vector2Int coords = new();
        if ( resetPath ) {
            coords = pathFinder.StartCoords;
        } else {
            coords = gridManager.GetCoordsFromPosition( transform.position );
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath( coords );
        StartCoroutine( FollowPath( speed ) );
    }

    private IEnumerator FollowPath( float moveSpeed ) {
        for ( int i = 0; i < path.Count; i++ ) {
            Vector3 startPos = this.transform.position;
            Vector3 endPos = gridManager.GetWorldPosFromCoords( path[ i ].Coords );
            float travelPercent = 0f;

            // TODO: TURN to face destination
            //selectedUnit.LookAt( endPos );

            while ( travelPercent < 1f ) {
                travelPercent += Time.deltaTime * moveSpeed;
                this.transform.position = Vector3.Lerp( startPos, endPos, travelPercent );
                yield return new WaitForEndOfFrame();
            }
        }
    }


}