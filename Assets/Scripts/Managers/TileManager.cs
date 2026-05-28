using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    [Header("Particules")]
    [SerializeField] private GameObject particule;
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
    
    public TileBase GetTileType(Vector2 hitPosition, Tilemap hitTilemap) //check position et tilemap touchée
    {
        if (hitTilemap == null) return null;
        
        Vector3Int cellPosition = hitTilemap.WorldToCell(hitPosition);
        TileBase hitTile = hitTilemap.GetTile(cellPosition);

        if (hitTile != null)
        {
            //Debug.Log($"tile {hitTile.name} touchée");
        }

        return hitTile;
    }
    public void SpawnParticleForTile(TileBase hitTile, Vector3 position)
    {
        if (hitTile == null) return;
        
        switch (hitTile.name) //faudrait check le nom des tiles
        {
            /*
            case "GrassTile": //exemple prcq on a pas de nom actuellement
                Instantiate(particule, position, Quaternion.identity);
                break;
            */
            default:
                //Debug.Log("pas de particule pour cette tile");
                break;
        }
    }
    
}