using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightSelector : MonoBehaviour
{
    private Transform highlight;
    private Selectable hovered;
    private Selectable selection;
    private RaycastHit raycasthit;

    private void Update() {
        if (highlight != null) {
            if(highlight.gameObject.GetComponent<Selectable>() != null ) {
                highlight.gameObject.GetComponent<Selectable>().UnHighlight();
            }
            highlight = null;
        }

        // TODO: UPDATE input action listener
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if ( !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycasthit) ) {

            highlight = raycasthit.transform;
            if (highlight != selection && ( hovered = highlight.GetComponent<Selectable>()) != null ) {
                hovered.Highlight();
            } else {
                Outline outline = highlight.gameObject.AddComponent<Outline>();
                outline.enabled = true;
                highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                highlight.gameObject.GetComponent<Outline>().OutlineWidth = 5f;
            }
        } else {
            highlight = null;
        }

        // Selection
        if ( Input.GetMouseButtonDown( 0 ) ) {
            if ( highlight ) {
                if ( selection != null ) {
                    selection.Deselect();
                    selection = highlight.GetComponent<Selectable>();
                    selection.Select();
                    highlight = null;
                } else {
                    if ( selection ) {
                        selection.Deselect();
                        selection = null;
                    }
                }
            }
        }


    }

}
