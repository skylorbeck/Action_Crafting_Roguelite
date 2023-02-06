using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallGenerator : MonoBehaviour
{
    public Tilemap wallmap;
    public TileBase wallVertical;
    public TileBase wallHorizontal;
    public TileBase wallCornerUpperLeft;
    public TileBase wallCornerUpperRight;
    public TileBase wallCornerLowerLeft;
    public TileBase wallCornerLowerRight;
    public Vector3Int wallmapSize;//TODO Move this to a global settings class
    void Start()
    {
        
        for (int i = -wallmapSize.x; i <= wallmapSize.x; i++)
        {
            wallmap.SetTile(new Vector3Int(i,-wallmapSize.y,0), wallHorizontal);
            wallmap.SetTile(new Vector3Int(i,wallmapSize.y,0), wallHorizontal);
        }
        for (int i = -wallmapSize.y; i <= wallmapSize.y; i++)
        {
            wallmap.SetTile(new Vector3Int(-wallmapSize.x,i,0), wallVertical);
            wallmap.SetTile(new Vector3Int(wallmapSize.x,i,0), wallVertical);
        }
        wallmap.SetTile(new Vector3Int(-wallmapSize.x,-wallmapSize.y,0), wallCornerLowerLeft);
        wallmap.SetTile(new Vector3Int(wallmapSize.x,-wallmapSize.y,0), wallCornerLowerRight);
        wallmap.SetTile(new Vector3Int(-wallmapSize.x,wallmapSize.y,0), wallCornerUpperLeft);
        wallmap.SetTile(new Vector3Int(wallmapSize.x,wallmapSize.y,0), wallCornerUpperRight);
    }

    
}
