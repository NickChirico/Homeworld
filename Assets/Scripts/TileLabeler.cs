using TMPro;
using UnityEngine;

/** Attaches to Tile, Always updates its own label **/
public class TileLabeler : MonoBehaviour
{
    TextMeshPro gridLabel;

    private void Awake() { 
        gridLabel = GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateLabel(Vector2Int coords) {
        gridLabel.text = $"{coords.x},{coords.y}";
        //Debug.Log( "coordinates: " + coords.ToString() );
        this.transform.name = coords.ToString();
    }
}
