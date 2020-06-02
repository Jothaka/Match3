using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    [Header("Tilemap references")]
    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private Tile[] colorTiles;

    [Header("Highlightmap references")]
    [SerializeField]
    private Tilemap highlightMap;
    [SerializeField]
    private Tile highlightTile;

    private List<GridTileData> currentTiles = new List<GridTileData>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            SelectTiles();
        }
        else
        {
            if(currentTiles.Count >= 3)
            {
                highlightMap.ClearAllTiles();
                //maybe add small effect on highlight map here?!


            }
        }
    }

    private void SelectTiles()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tileCoordinate = grid.WorldToCell(mouseWorldPos);

        Tile selectedTile = map.GetTile<Tile>(tileCoordinate);
        if (selectedTile != null)
        {
            GridTileData selectedTileData = new GridTileData(tileCoordinate, selectedTile);

            if (currentTiles.Count == 0)
            {
                currentTiles.Add(selectedTileData);

                highlightMap.SetTile(selectedTileData.GridPosition, highlightTile);
            }
            else
            {
                var currentTileData = currentTiles[currentTiles.Count - 1];
                if (!currentTiles.Contains(selectedTileData))
                {
                    if (currentTileData.IsTileValidNeighbor(selectedTileData))
                    {
                        currentTiles.Add(selectedTileData);
                        highlightMap.SetTile(selectedTileData.GridPosition, highlightTile);
                    }
                }
            }
        }
    }

    private IEnumerator ProcessTilesFalling()
    {

        yield return new WaitForSeconds(0.5f);
    }
}
