using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Selectable
{
    [SerializeField] private float moveSpeed;

    protected WorldTile currentSpot;
    protected WorldTile lastSpot;

    public WorldTile CurrentSpot { get { return currentSpot; } }
    public float MoveSpeed { get { return moveSpeed; } }

    protected int visionRange;
    // TODO: MOVE TO SUBCLASS!!



    protected override void Awake() {
        base.Awake();
        this.selectType = SelectableType.UNIT;
    }

    protected override void Start() {
        base.Start();

        // temp, get currentSpot on Spawn
        //Vector2Int startingCoords = GridManager.GetGridManager.GetCoordsFromPosition( this.transform.position );
        //this.currentSpot = GridManager.GetGridManager.GetTileAtCoords( startingCoords );
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

        // TODO: Should there be one SINGLETON UnitGridControl? Or one one each Unit running simul
        Debug.Log( $"MOVING TO ({target.name}), from  SPOT {CurrentSpot.name}" );
        UnitGridController.GetUnitController.MoveUnitToSpace(this, target);
    }

    /*public Vector2Int[] GetAvailableMoves() {

        int numLegalSpaces = 0;

        Vector2Int[] allTilesInRange = GridManager.GetAllTilesInRange( currentSpot.GridCoords, visionRange );

        Debug.Log( $"GOT ({allTilesInRange.Length}) Tiles within {visionRange} !" );




        return null;
    }*/


}
