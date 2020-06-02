using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [Header("Grid settings")]
    public Vector2Int GridStartAnchor;
    public Vector2Int GridSize;

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

    private Coroutine fallCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(5);
        for (int k = 0; k < GridSize.x; k++)
        {
            for (int i = 0; i < GridSize.y; i++)
            {
                currentTiles.Add(new GridTileData(new Vector3Int(GridStartAnchor.x - k, GridStartAnchor.y + i, 0), null));
            }
        }
        fallCoroutine = StartCoroutine(ProcessTilesFalling(currentTiles));
    }

    // Update is called once per frame
    void Update()
    {
        if (fallCoroutine == null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                SelectTiles();
            }
            else
            {
                if (currentTiles.Count >= 1)
                {
                    highlightMap.ClearAllTiles();
                    if (currentTiles.Count >= 3)
                    {
                        fallCoroutine = StartCoroutine(ProcessTilesFalling(currentTiles));
                        //maybe add small effect on highlight map here?!
                    }
                    currentTiles.Clear();
                }
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
                else
                {
                    if (currentTileData.GridPosition != selectedTileData.GridPosition)
                    {
                        if (selectedTileData.GridPosition == currentTiles[currentTiles.Count - 2].GridPosition)
                        {
                            highlightMap.SetTile(currentTileData.GridPosition, null);
                            currentTiles.Remove(currentTileData);
                        }
                    }
                }
            }
        }
    }

    private void SpawnNewRandomTile(Vector3Int spawnPosition)
    {
        Tile tile = colorTiles[Random.Range(0, colorTiles.Length)];
        map.SetTile(spawnPosition, tile);
    }

    private IEnumerator ProcessTilesFalling(List<GridTileData> tilesToReplace)
    {
        List<Vector3Int> emptyGridPositions = new List<Vector3Int>();
        for (int i = 0; i < tilesToReplace.Count; i++)
        {
            var tilePosition = tilesToReplace[i].GridPosition;

            map.SetTile(tilePosition, null);
            emptyGridPositions.Add(tilePosition);
        }
        tilesToReplace.Clear();
        List<Vector3Int> removedGridPositions = new List<Vector3Int>();

        emptyGridPositions.Sort(new CustomVector3IntComparer());

        while (emptyGridPositions.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < emptyGridPositions.Count; i++)
            {
                var gridPosition = emptyGridPositions[i];
                if (gridPosition.x == GridStartAnchor.x)
                {
                    SpawnNewRandomTile(gridPosition);

                    removedGridPositions.Add(gridPosition);
                }
                else
                {
                    gridPosition.x++;
                    Tile fallingTile = map.GetTile<Tile>(gridPosition);
                    if (fallingTile != null)
                    {
                        map.SetTile(gridPosition, null);
                        map.SetTile(emptyGridPositions[i], fallingTile);
                    }
                    emptyGridPositions[i] = gridPosition;
                }
            }
            for (int i = 0; i < removedGridPositions.Count; i++)
            {
                emptyGridPositions.Remove(removedGridPositions[i]);
            }
            removedGridPositions.Clear();
        }
        fallCoroutine = null;
    }
}
