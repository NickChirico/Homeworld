using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldTile : Selectable
{
    public bool isWalkable;
    public bool isBlocked;

    private Vector2Int gridCoords;
    private Vector3 centerPosition;
    private bool isOccupied = false;
    private TileLabeler labeler;

    public Vector2Int GridCoords { get { return gridCoords; } }
    public Vector3 CenterPosition { get { return centerPosition; } }
    public bool IsOccupied { get { return isOccupied; } }

    public void SetGridCoords(Vector2Int coor ) { gridCoords = coor; }

    protected override void Awake() {
        base.Awake();
        this.selectType = SelectableType.TILE;
        labeler = this.GetComponent<TileLabeler>();
    }

    protected override void Start() {
        this.UpdateGridCoords();
        centerPosition = new Vector3( GridCoords.x + ( GridManager.GridTransformSize * 0.5f ), 0f, GridCoords.y + ( GridManager.GridTransformSize * 0.5f ) );
    }

    // ### Select / Highlight
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

    // ### Occupy 
    public WorldTile Occupy() {
        isOccupied = true;

        // ## Do any additional stuff when this tile is occupied...
        // ## i.e. Trigger any traps that were previously left on this space
        // foreach (trap in trapsSetOnThisSpace)
        //      trap.Trigger() !
        // 
        // if (onFire)
        //    BurnOccupants();
        return this;
    }

    public WorldTile Leave() {
        isOccupied = false;
        return this;
    }

    public bool IsAvailable() {
        return !isOccupied;
    }

    // #################### TILE LABELER

    private void UpdateGridCoords() {
        // TODO: Dont hardcode around (0,0) transform!
        gridCoords.x = Mathf.RoundToInt( transform.position.x / GridManager.GridTransformSize );
        gridCoords.y = Mathf.RoundToInt( transform.position.z / GridManager.GridTransformSize );

        if ( labeler ) {
            labeler.UpdateLabel( gridCoords );
        }
    }
}


