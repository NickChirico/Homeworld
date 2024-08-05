using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager GetGridManager { get { return _instance; } }
    //[SerializeField] private bool DEBUG_VISUALS = false;

    [Header( "Grid Properties" )]
    [SerializeField] private Vector2Int gridSize;
    private const int gridTransformSize = 1;

    public static int GridTransformSize => gridTransformSize;

    //Dictionary<Vector2Int, WorldTile> gridTiles = new();
    Dictionary<Vector2Int, GridNode> gridNodes = new();
    public Dictionary<Vector2Int, GridNode> Grid { get { return gridNodes; } }

    [Header( "Generate Grid On Start" )]
    [SerializeField] private bool generateSquareGridOnStart = false;
    [SerializeField] private Vector2Int generationOffset;
    [SerializeField] private WorldTile TilePrefab; // TODO: Choose from constant pool of tile options 
    [SerializeField] private WorldTile TilePrefab_2; // TODO: Choose from constant pool of tile options 
    [SerializeField][Range( 0, 0.99f )] private float alt_chance = 0.35f;

    private void Awake() {
        _instance = this;

        if ( generateSquareGridOnStart ) {
            _generateSquareGrid( gridSize.x, gridSize.y );
            return;
        }



    }

    private void Start() {
        // TEMPORARY:: Just add all WorldTiles on start to clickable grid.
        // Once we're doing multiple rooms, update this

        if ( !generateSquareGridOnStart ) {
            foreach ( WorldTile tile in FindObjectsOfType<WorldTile>() ) {
                _addTileToGrid( tile );
            }
        }

        foreach ( Unit unit in FindObjectsOfType<Unit>() ) {
            WorldTile tile = GetTileAtCoords( GetCoordsFromPosition( unit.transform.position ) );
            unit.SetCurrentSpot( tile );
            // TODO: Broken - move unit to current spot on spawn
            // UnitGridController.GetUnitController.MoveUnitToSpace( unit, tile );
        }
    }

    private void _generateSquareGrid( int length, int width ) {
        for ( int x = generationOffset.x; x < length + generationOffset.x; x++ ) {
            for ( int y = generationOffset.y; y < width + generationOffset.y; y++ ) {
                Vector2Int coords = new Vector2Int( x, y );

                // TODO: Improve generation spawning chance
                if ( Random.value < alt_chance ) {
                    WorldTile tile = Instantiate( TilePrefab_2, position: new Vector3( x, 0f, y ), Quaternion.identity );
                    tile.GetComponent<WorldTile>().SetGridCoords( coords );
                    gridNodes.Add( coords, new GridNode( coords, false, tile ) );

                } else {
                    WorldTile tile = Instantiate( TilePrefab, position: new Vector3( x, 0f, y ), Quaternion.identity );
                    tile.GetComponent<WorldTile>().SetGridCoords( coords );
                    gridNodes.Add( coords, new GridNode( coords, true, tile ) );
                }
            }
        }
    }

    private void _addTileToGrid( WorldTile tile ) {
        gridNodes.Add( tile.GridCoords, new GridNode( tile.GridCoords, tile.isWalkable, tile ) );
        if ( tile.isBlocked ) {
            BlockNode( tile.GridCoords );
        }

        //Debug.Log( $"added {tile.name}, coords:{tile.GridCoords} to the grid" ); 
        //Debug.Log("Size: " + grid.Count + " ... " + grid.ToCommaSeparatedString() );
    }

    public WorldTile GetTileAtCoords( Vector2Int coords ) {

        return gridNodes[ coords ].GetWorldTile;
    }


    /*
     * Called by Unit.GetAvailableMoves()
     */
    internal static Vector2Int[] GetAllTilesInRange( Vector2Int startCoord, int range, bool roundCorners = true ) {

        //int numSpaces;

        int minX = startCoord.x - range;
        int maxX = startCoord.x + range;
        int minY = startCoord.y - range;
        int maxY = startCoord.y + range;

       /* foreach( WorldTile tile in _instance.gridTiles.Values  ) {
            //
        }*/
        


        return null;
    }




    /*** 
     * ########################
     * # Breadth First Search #
     * ########################
     * */
    public GridNode GetNode( Vector2Int coords) {
        if ( gridNodes.ContainsKey( coords ) ) {
            return gridNodes[coords];
        }
        return null;
    }

    public void BlockNode(Vector2Int coords) {
        if(gridNodes.ContainsKey( coords ) ) {
            gridNodes[ coords ].Walkable = false;
        }
    }

    public void ResetNodes() {
        foreach(KeyValuePair<Vector2Int, GridNode> pair in gridNodes ) {
            pair.Value.ConnectedTo = null;
            pair.Value.Explored = false;
            pair.Value.Path = false;
        }
    }

    public Vector2Int GetCoordsFromPosition(Vector3 worldPos) {
        Vector2Int coords = new();
        coords.x = Mathf.RoundToInt( worldPos.x / gridTransformSize );
        coords.y = Mathf.RoundToInt( worldPos.y / gridTransformSize );
        return coords;
    }

    public Vector3 GetWorldPosFromCoords(Vector2Int coords) {
        Vector3 pos = new();
        pos.x = coords.x * gridTransformSize;
        pos.y = GridPathFinding.TransformY;
        pos.z = coords.y * gridTransformSize;
        return pos;
    }
}
