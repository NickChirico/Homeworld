using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager GetGridManager {  get { return _instance; } }
    [SerializeField] private bool DEBUG_VISUALS = false;

    [Header( "Grid Properties" )]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int gridTransformSize;

    public int GridTransformSize { get { return gridTransformSize; } }

    Dictionary<Vector2Int, GridNode> grid = new();
    Dictionary<Vector2Int, GridNode> Grid {  get { return grid; } }

    [Header("Generate Grid On Start")]
    [SerializeField] private bool generateSquareGridOnStart = false;
    [SerializeField] private WorldTile TilePrefab; // get from constant pool of tile options 


    private void Awake() {
        _instance = this;

        if(generateSquareGridOnStart)
            GenerateSquareGrid(gridSize.x, gridSize.y);
    }


    private void GenerateSquareGrid(int length, int width) {
        for ( int x = 0; x < length; x++ ) {
            for ( int y = 0; y < width; y++ ) {
                // init grid axiis, and instantiate tile
                Vector2Int coords = new Vector2Int( x, y );
                grid.Add( coords, new GridNode( coords ) );
                var spawnedTile = Instantiate( TilePrefab, new Vector3( x, 0f, y ), Quaternion.identity );
                // Tile Object is renamed by its GridLabeler component to its coords: i.e. "(0,1)"

                if ( DEBUG_VISUALS ) { // debug visualization
                    GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube );
                    Vector3 pos = new Vector3( coords.x * gridTransformSize, 0f, coords.y * gridTransformSize );
                    cube.transform.position = pos;
                    cube.transform.localScale = Vector3.one * 0.45f;
                    cube.transform.SetParent( this.transform );
                }
            }
        }
    }

}
