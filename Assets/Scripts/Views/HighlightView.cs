using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightView : MonoBehaviour
{
    [Header("Highlightmap references")]
    [SerializeField]
    private Tilemap highlightMap;
    [SerializeField]
    private Tile highlightTile;

    public void SetHighlight(Vector3Int mapCoordinates)
    {
        highlightMap.SetTile(mapCoordinates, highlightTile);
    }

    public void RemoveHighlight(Vector3Int mapCoordinates)
    {
        highlightMap.SetTile(mapCoordinates, null);
    }

    public void ClearAllHighlights()
    {
        highlightMap.ClearAllTiles();
    }
}
