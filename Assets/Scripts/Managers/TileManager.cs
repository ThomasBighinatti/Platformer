using System;
using Arrows;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.parent);
    }
    
    public void GetTileType(Vector2 arrowCoords)
    {
        Tilemap.GetTile(arrowCoords);
    }
}
