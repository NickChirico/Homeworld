using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : Selectable
{
    public float transformY_Height;
    private Vector2Int gridCoords;
    private Vector3 worldPosition;
    public Vector3 WorldPosition { get { return worldPosition; } }

    private bool isOccupied = false;
    public bool IsOccupied { get { return isOccupied; } }


    protected override void Awake() {
        base.Awake();
        this.selectType = SelectableType.TILE;
        gridCoords = this.GetComponent<TileLabeler>().GetCoords;
        worldPosition = new Vector3( gridCoords.x, transformY_Height, gridCoords.y );
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
}
