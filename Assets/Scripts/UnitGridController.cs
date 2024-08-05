using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridController : MonoBehaviour
{
    static UnitGridController _instance;
    public static UnitGridController GetUnitController { get { return _instance; } }

    GridManager gridManager;
    GridPathFinding pathFinder;
    List<GridNode> path = new();

    Transform selectedUnit;

    private void Awake() {
        _instance = this;
    }

    void Start()
    {
        gridManager = GridManager.GetGridManager;
        pathFinder = GridPathFinding.GetPathFinding;
    }

    public void MoveUnitToSpace(Unit unit, WorldTile dest) {
        selectedUnit = unit.transform;
        pathFinder.SetNewDestination( unit.CurrentSpot.GridCoords, dest.GridCoords );
        unit.SetCurrentSpot(dest.Occupy());
        RecalculatePath( true, unit.MoveSpeed );
    }

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
        StartCoroutine( FollowPath(speed) );
    }

    private IEnumerator FollowPath(float moveSpeed) {
        for ( int i = 0; i < path.Count; i++ ) {
            Vector3 startPos = selectedUnit.position;
            Vector3 endPos = gridManager.GetWorldPosFromCoords( path[i].Coords );
            float travelPercent = 0f;

            // TODO: TURN to face destination
            //selectedUnit.LookAt( endPos );

            while ( travelPercent < 1f ) {
                travelPercent += Time.deltaTime * moveSpeed;
                selectedUnit.position = Vector3.Lerp(startPos, endPos, travelPercent );
                yield return new WaitForEndOfFrame();
            }
        }
    }


}
