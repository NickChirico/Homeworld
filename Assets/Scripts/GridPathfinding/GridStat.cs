using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    private int visited = -1;
    private int x = 0;
    private int y = 0;

    public int Visited { get => visited; set => visited =  value ; }

    public void InitCoords(int x, int y ) {
        this.x = x; 
        this.y = y;
    }

    public (int, int) GetCoords() {
        return (x, y);
    }
}
