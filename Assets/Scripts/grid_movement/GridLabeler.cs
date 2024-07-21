using TMPro;
using UnityEngine;

[ExecuteAlways]
/** Attaches to Tile, Always updates its own label **/
public class GridLabeler : MonoBehaviour
{
    TextMeshPro gridLabel;
    Vector2Int coordinates = new();
    public Vector2Int GetCoords {  get { return coordinates; } }
    GridManager gridManager;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        gridLabel = GetComponentInChildren<TextMeshPro>();

        UpdateCoordsLabel();
    }

    private void Update() {
        UpdateCoordsLabel();
        // rename tile Object to coordinates
        this.transform.name = coordinates.ToString();
    }

    private void UpdateCoordsLabel() {
        if ( !gridManager ) return;
        // TODO: Dont hardcode around (0,0) transform!
        coordinates.x = Mathf.RoundToInt( transform.position.x / gridManager.UnityGridSize );
        coordinates.y = Mathf.RoundToInt( transform.position.z / gridManager.UnityGridSize );
        gridLabel.text = $"{coordinates.x},{coordinates.y}";
    }
}
