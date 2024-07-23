using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Selectable
{
    protected WorldTile currentSpot;
    protected WorldTile lastSpot;
    public WorldTile CurrentSpot { get { return currentSpot; } }

    protected override void Awake() {
        base.Awake();
        this.selectType = SelectableType.UNIT;
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

    public void LeaveCurrentSpot() {
        if (currentSpot != null) {
            lastSpot = currentSpot.Leave();
            currentSpot = null;
        }

        
    }

    public void MoveTo( WorldTile target ) {
        // TODO: lol
        this.transform.position = target.WorldPosition;
        currentSpot = target.Occupy();
    }
}
