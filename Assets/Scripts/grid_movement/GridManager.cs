using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager GetGridManager {  get { return _instance; } }
    [SerializeField] bool DEBUG = false;


    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unitGridSize;

    public int UnityGridSize { get { return unitGridSize; } }

    Dictionary<Vector2Int, GridNode> grid = new();
    Dictionary<Vector2Int, GridNode> Grid {  get { return grid; } }

    private void Awake() {
        _instance = this;
        // init grid axiis
        for ( int x = 0; x < gridSize.x; x++ ) {
            for ( int y = 0; y < gridSize.y; y++ ) {
                Vector2Int coords = new Vector2Int( x, y );
                grid.Add( coords, new GridNode( coords ) );

                if ( DEBUG ) { // debug visualization
                    GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube );
                    Vector3 pos = new Vector3( coords.x * unitGridSize, 0f, coords.y * unitGridSize );
                    cube.transform.position = pos;
                    cube.transform.localScale = Vector3.one * 0.45f;
                    cube.transform.SetParent( this.transform );
                }
            }

        }
    }

}
