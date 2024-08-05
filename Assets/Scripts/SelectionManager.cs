using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectionManager : MonoBehaviour
{
    private static SelectionManager _instance;
    public static SelectionManager GetSelectionManager { get { return _instance; } }
    public static Selectable GetSelected { get { return _instance.selected; } }

    private Transform current;
    private Selectable hovered;
    private Selectable selected;
    private RaycastHit raycasthit;

    private void Awake() {
        _instance = this;
    }

    private void Update() {

        // TODO: UPDATE input action listener, improve overhead on this
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        if ( !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast( ray, out raycasthit ) ) {

            current = raycasthit.transform;
            if ( current != selected && current != hovered ) {
                if ( hovered != null ) {
                    hovered.UnHighlight();
                }
                if ( ( hovered = current.GetComponent<Selectable>() ) != null ) {
                    hovered.Highlight();
                }
            }


            if ( current != selected && ( hovered = current.GetComponent<Selectable>() ) != null ) {
                hovered.Highlight();
            } else {
                /* IF GAMEOBJECT IS NOT `SELECTABLE`
                Outline outline = current.gameObject.AddComponent<Outline>();
                outline.enabled = true;
                current.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                current.gameObject.GetComponent<Outline>().OutlineWidth = 5f;
                */
            }
        } else {
            current = null;
        }

        // Selection
        // TODO: Update with better input action listener
        if ( Input.GetMouseButtonDown( 0 ) ) {
            Selectable current_select;
            if ( current != null ) {
                current_select = current.GetComponent<Selectable>();
                if(selected && selected.Equals( current_select ) ) {
                    selected.Deselect();
                    selected = null;
                    return;
                }
            } else {
                current = null;
                return;
            }

            // ###
            if ( current_select.isUnit ) {
                // IF CLICK ON UNIT..
                // # DESELECT CURRENT SELECTION
                // # SELECT UNIT

                if ( selected ) {
                    selected.Deselect();
                }
                if ( ( selected = current.GetComponent<Selectable>() ) != null ) {
                    selected.Select();
                }
            } else if ( current_select.isTile ) {
                // IF CLICK ON TILE...
                // # IF UNIT SELECTED, MOVE SELECTED UNIT TO TARGET AND KEEP IT SELECTED
                // # IF TILE/NOTHING SELECTED, SELECT NEW TILE 

                if ( selected ) {
                    if ( selected.isUnit && ( (WorldTile) current_select ).IsAvailable() ) {
                        ( (Unit) selected ).MoveTo( (WorldTile) current_select );
                    } else {
                        // replace selection
                        selected.Deselect();
                        selected = current_select;
                        selected.Select();
                    }
                } else {
                    // select GameTile if nothing else selected
                    current_select.Select();
                }
            }
        }
    }
}
