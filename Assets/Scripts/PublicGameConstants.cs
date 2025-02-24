using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicGameConstants : MonoBehaviour
{
    private static PublicGameConstants _inst;
    public static PublicGameConstants constants { get { return _inst; } }

    private void Awake() {
        _inst = this;
    }


    // ## Tile Highlight Colors - not being used - just set in Tile's prefab for now
    //public Color highlight_color_tile_normal;
    //public Color highlight_color_tile_hazard;
}
