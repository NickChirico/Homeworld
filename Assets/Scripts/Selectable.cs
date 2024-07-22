using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attaches to any object that is selectable
 * i.e. Units, Tiles, etc.
 * 
 * */

public class Selectable : MonoBehaviour
{
    Outline highlightOutline;

    private void Awake() {
        highlightOutline = GetComponent<Outline>();
        if(highlightOutline == null ) {
            throw new System.Exception( $"{this.name} HAS NO `Outline` COMPONENT" );
        }
        highlightOutline.enabled = false;
    }

    public void Highlight() {
        highlightOutline.enabled = true;
    }
    public void UnHighlight() {
        highlightOutline.enabled = false;
    }

    public void Select() {
        highlightOutline.enabled = true;

    }
    public void Deselect() {
        highlightOutline.enabled = false;

    }



}
