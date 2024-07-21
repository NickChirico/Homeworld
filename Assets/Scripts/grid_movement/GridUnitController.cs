using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnitController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    bool unitIsSelected = false;
    Transform selectedUnit;
    GridManager gridManager;

    void Start() {
        gridManager = this.GetComponent<GridManager>();

    }

    void Update() {


        // TODO :: Must be updated to new player input actions

        if ( Input.GetMouseButtonDown( 0 ) ) {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            bool hasHit = Physics.Raycast( ray, out hit );

            if ( hasHit ) {
                if ( hit.transform.CompareTag( "TILE" ) ) {
                    if(unitIsSelected ) {
                        Vector2Int targetCoords = hit.transform.GetComponent<GridLabeler>().GetCoords;
                        Vector2Int startCoords = new Vector2Int((int) selectedUnit.position.x, (int) selectedUnit.position.y / gridManager.UnityGridSize );
                        // TODO: move unit
                        selectedUnit.position = new Vector3( targetCoords.x, selectedUnit.position.y, targetCoords.y );
                        unitIsSelected = false;
                    }

                } else if ( hit.transform.CompareTag( "UNIT" ) ) {
                    selectedUnit = hit.transform;
                    unitIsSelected = true;
                }
            }
        }
    }
}
