using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attaches to any object that is selectable
 * i.e. Units, Tiles, etc.
 * 
 * */

public abstract class Selectable : MonoBehaviour
{
    public enum SelectableType { NULL, UNIT, TILE }

    protected SelectableType selectType = SelectableType.NULL;
    public SelectableType SelectType { get { return selectType; } }

    Outline highlightOutline;
    protected bool isHovered;
    protected bool isSelected;

    [SerializeField] protected bool hasSelectIndicator;
    protected GameObject selectIndicator;

    protected virtual void Awake() {
        highlightOutline = this.GetComponent<Outline>();
        if ( highlightOutline == null ) {
            throw new System.Exception( $"{this.name} HAS NO `Outline` COMPONENT" );
        }
        highlightOutline.enabled = false;

        if ( hasSelectIndicator ) {
            foreach ( Transform transform in this.transform ) {
                if ( transform.CompareTag( "SELECT_INDICATOR" ) ) {
                    selectIndicator = transform.gameObject;
                    selectIndicator.SetActive( value: false );
                    break;
                }
            }
            if ( !selectIndicator ) {
                throw new System.Exception( $"{this.name} is Selectable but has no SELECT_INDICATOR in hierarchy!" );
            }
        }
    }


    protected virtual void Update() {
        if ( !isHovered && highlightOutline.enabled == true ) {
            UnHighlight();
        }
    }

    public bool isUnit { get { return selectType == SelectableType.UNIT; } }
    public bool isTile { get { return selectType == SelectableType.TILE; } }



    /* ### HIGHLIGHT && SELECT ###
     */
    public virtual void Highlight() {
        _refreshHighlightIndicator( isHovered = true );
    }
    public virtual void UnHighlight() {
        _refreshHighlightIndicator(isHovered = false);
    }

    public virtual void Select() {
        UIManager.GetInstance.SetSelectText( "Selected:  " + this.gameObject.name );
        _refreshSelectIndicator(isSelected = true);
    }
    public virtual void Deselect() {
        UIManager.GetInstance.SetSelectText( "Selected:  none" );
        _refreshSelectIndicator( isSelected = false);
    }

    private void _refreshHighlightIndicator(bool b) {
        highlightOutline.enabled = b;
    }

    private void _refreshSelectIndicator(bool b) {
        if ( selectIndicator ) {
            selectIndicator.SetActive( b );
        }
    }

}
