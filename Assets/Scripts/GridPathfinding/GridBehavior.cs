using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    // init grid
    public bool SettingDistance = false;
    public int rows = 10;
    public int columns = 10;
    public float scale = 1f;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = Vector3.zero;
    public List<GameObject> path = new();

    //
    public GameObject[,] gridArray;
    public int startX = 0, startY = 0, endX = 2, endY = 2;

    //
    const int UP = 1, RIGHT = 2, DOWN = 3, LEFT = 4, LAST = -1;

    private void Awake() {
        gridArray = new GameObject[columns, rows];
        if(gridPrefab != null ) {
            _generateGrid();
        } else {
            throw new UnityException( "GridBehavior.Awake() : Missing gridPrefab!" );
        }
    }

    private void Update() {
        if(SettingDistance) {
            _setDistance();
            _setBestPath();
            SettingDistance = false;
        }
    }

    private void _generateGrid() {
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) { 
                GameObject obj = Instantiate(gridPrefab,
                    new Vector3(
                    leftBottomLocation.x + scale * x, 
                    leftBottomLocation.y, 
                    leftBottomLocation.z + scale * y),
                    Quaternion.identity);
                obj.transform.SetParent( gameObject.transform );
                obj.GetComponent<GridStat>().InitCoords( x, y );
                gridArray[x, y] = obj;
            }
        }
    }

    private void _setDistance() {
        _initSetup();
        //int x = startX, y = startY;
        //int[] testArr = new int[ rows * columns ];
        for(int step = 1; step < rows * columns; step++ ) {
            foreach(GameObject obj in gridArray ) {
                if(obj != null && obj.GetComponent<GridStat>().Visited == step-1 ) {
                    _testFourDirections( obj.GetComponent<GridStat>().GetCoords(), step );
                }
            }
        }
    }

    private void _setBestPath() {
        int step;
        int x = endX, y = endY;
        List<GameObject> tempList = new();
        path.Clear();
        if ( gridArray[endX, endY] && gridArray[endX,endY].GetComponent<GridStat>().Visited > 0 ) {
            path.Add( gridArray[ x, y ] );
            step = gridArray[ x, y ].GetComponent<GridStat>().Visited - 1;
        } else {
            // CANNOT REACH LOCATION!
            throw new System.Exception( $"Cant reach location : endLoc=({endX},{endY})" );
        }

        for(int i = step; step > -1; step-- ) {
            if ( _testDirection( x, y, step, UP ) )
                tempList.Add( gridArray[ x, y + 1 ] );
            if ( _testDirection( x, y, step, RIGHT ) )
                tempList.Add( gridArray[ x + 1, y ] );
            if ( _testDirection( x, y, step, DOWN ) )
                tempList.Add( gridArray[ x, y - 1 ] );
            if ( _testDirection( x, y, step, LEFT ) )
                tempList.Add( gridArray[ x - 1, y ] );

            GameObject tempObj = _findClosest( gridArray[ endX, endY ].transform, tempList );
            path.Add( tempObj );
            (int x, int y) coords = tempObj.GetComponent<GridStat>().GetCoords();
            x = coords.x; y = coords.y;
            tempList.Clear();
        }
    }

    private GameObject _findClosest(Transform targetLoc, List<GameObject> list ) {
        float currentDistance = scale * rows * columns;
        int index = 0;
        for(int i = 0; i < list.Count; i++ ) {
            float dist = Vector3.Distance( targetLoc.position, list[ i ].transform.position);
            if ( dist < currentDistance ) {
                currentDistance = dist;
                index = i;
            }
        }
        return list[index];
    }


    private void _initSetup() {
        foreach(GameObject obj in gridArray) {
            obj.GetComponent<GridStat>().Visited = -1;
        }
        gridArray[ startX, startY ].GetComponent<GridStat>().Visited = 0;
    }

    private bool _testDirection( int x, int y, int step, int dir ) {
        // dir tells which case to use
        switch ( dir ) {
            case LEFT:
                return x - 1 > -1 && gridArray[ x - 1, y ] && gridArray[ x - 1, y ].GetComponent<GridStat>().Visited == step;
            case DOWN:
                return y - 1 > -1 && gridArray[ x, y - 1 ] && gridArray[ x, y - 1 ].GetComponent<GridStat>().Visited == step;
            case RIGHT:
                return x + 1 < columns && gridArray[ x + 1, y ] && gridArray[ x + 1, y ].GetComponent<GridStat>().Visited == step;
            case UP:
                return y + 1 < rows && gridArray[ x, y + 1 ] && gridArray[ x, y + 1 ].GetComponent<GridStat>().Visited == step;
            default:
                return false;
        }
    }

    private void _testFourDirections( (int x, int y) coords, int step ) {
        int x = coords.x, y = coords.y;
        if ( _testDirection( x, y, LAST, UP ) )
            _setVisited( x, y + 1, step );
        if ( _testDirection( x, y, LAST, RIGHT ) )
            _setVisited( x + 1, y, step );
        if ( _testDirection( x, y, LAST, DOWN ) )
            _setVisited( x, y - 1, step );
        if ( _testDirection( x, y, LAST, LEFT ) )
            _setVisited( x - 1, y, step );
    }

    private void _setVisited( int x, int y, int step ) {
        if ( gridArray[ x, y ] )
            gridArray[ x, y ].GetComponent<GridStat>().Visited = step;
    }


}
