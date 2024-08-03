using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager GetGridManager {  get { return _instance; } }
    //[SerializeField] private bool DEBUG_VISUALS = false;

    [Header( "Grid Properties" )]
    [SerializeField] private Vector2Int gridSize;
    private const int gridTransformSize = 1;

    public static int GridTransformSize => gridTransformSize;

    Dictionary<Vector2Int, GridNode> grid = new();
    //Dictionary<Vector2Int, GridNode> Grid {  get { return grid; } }

    [Header("Generate Grid On Start")]
    [SerializeField] private bool generateSquareGridOnStart = false;
    [SerializeField] private Vector2Int generationOffset;
    [SerializeField] private WorldTile TilePrefab; // TODO: Choose from constant pool of tile options 
    [SerializeField] private WorldTile TilePrefab_2; // TODO: Choose from constant pool of tile options 
    [SerializeField] [Range(0, 0.99f)] private float alt_chance = 0.35f;


    private void Awake() {
        _instance = this;

        if ( generateSquareGridOnStart ) {
            GenerateSquareGrid( gridSize.x, gridSize.y );
            return;
        }
    }

    private void Start() {
        // TEMPORARY:: Just add all WorldTiles on start to clickable grid.
        // Once we're doing multiple rooms, update this
        if ( !generateSquareGridOnStart ) {
            foreach ( WorldTile tile in FindObjectsOfType<WorldTile>() ) {
                AddTileToGrid( tile );
            }
        }
    }

    private void GenerateSquareGrid(int length, int width) {
        for ( int x = generationOffset.x; x < length + generationOffset.x; x++ ) {
            for ( int y = generationOffset.y; y < width + generationOffset.y; y++ ) {
                // init grid axiis, and instantiate tile
                Vector2Int coords = new Vector2Int( x, y );
                grid.Add( coords, new GridNode( coords ) );

                if(Random.value < alt_chance ) {
                    var spawnedTile = Instantiate( TilePrefab_2, position: new Vector3( x, 0f, y ), Quaternion.identity );
                    spawnedTile.GetComponent<WorldTile>().SetGridCoords( coords );
                } else {
                    var spawnedTile = Instantiate( TilePrefab, position: new Vector3( x, 0f, y ), Quaternion.identity );
                    spawnedTile.GetComponent<WorldTile>().SetGridCoords( coords );
                }

                // Tile Object is renamed by its TileLabeler component to its coords: i.e. "(0,1)"

                /**
                if ( DEBUG_VISUALS ) { // debug visualization
                    GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube );
                    Vector3 pos = new Vector3( coords.x * gridTransformSize, 0f, coords.y * gridTransformSize );
                    cube.transform.position = pos;
                    cube.transform.localScale = Vector3.one * 0.45f;
                    cube.transform.SetParent( this.transform );
                }
                **/
            }
        }
    }

    private void AddTileToGrid(WorldTile tile) {
        grid.Add( tile.GridCoords, new GridNode( tile.GridCoords ) );
        //Debug.Log( $"added {tile.name}, coords:{tile.GridCoords} to the grid" ); 
        //Debug.Log("Size: " + grid.Count + " ... " + grid.ToCommaSeparatedString() );
    }

}
