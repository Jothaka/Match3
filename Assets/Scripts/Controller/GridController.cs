using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityRandom = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public event Action OnFallingTilesCompleted;

    [Header("Grid settings")]
    public Vector2Int GridStartAnchor;
    public Vector2Int GridSize;
    [Tooltip("How fast does each row of the blocks spawn")]
    public float SpawnInterval = 0.25f;

    [SerializeField]
    private Grid grid;

    [Header("Tilemap references")]
    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private Tile[] colorTiles;

    void Start()
    {
        List<GridTileData> currentTiles = new List<GridTileData>();

        for (int k = 0; k < GridSize.x; k++)
        {
            for (int i = 0; i < GridSize.y; i++)
            {
                currentTiles.Add(new GridTileData(new Vector3Int(GridStartAnchor.x - k, GridStartAnchor.y + i, 0), null));
            }
        }
        //Create the game level
        StartCoroutine(ProcessTilesFalling(currentTiles));
    }

    public GridTileData GetTileDataFromPosition(Vector3 mouseWorldPosition)
    {
        Vector3Int tileCoordinate = grid.WorldToCell(mouseWorldPosition);
        Tile selectedTile = map.GetTile<Tile>(tileCoordinate);

        GridTileData tileData = null;
        if (selectedTile != null)
            tileData = new GridTileData(tileCoordinate, selectedTile);

        return tileData;
    }

    public void RefillLevel(List<GridTileData> tilesToReplace)
    {
        StartCoroutine(ProcessTilesFalling(tilesToReplace));
    }

    private void SpawnNewRandomTile(Vector3Int spawnPosition)
    {
        Tile tile = colorTiles[UnityRandom.Range(0, colorTiles.Length)];
        map.SetTile(spawnPosition, tile);
    }

    private IEnumerator ProcessTilesFalling(List<GridTileData> tilesToReplace)
    {
        List<Vector3Int> emptyGridPositions = RemoveTilesFromTheGrid(tilesToReplace);

        List<Vector3Int> removedGridPositions = new List<Vector3Int>();

        //sort so the tiles at the top are processed first.
        emptyGridPositions.Sort(new CustomVector3IntComparer());

        while (emptyGridPositions.Count > 0)
        {
            yield return new WaitForSeconds(SpawnInterval);
            for (int i = 0; i < emptyGridPositions.Count; i++)
            {
                var gridPosition = emptyGridPositions[i];
                //spawn a new tile when the processed Position is at the top of the grid
                if (gridPosition.x == GridStartAnchor.x)
                {
                    SpawnNewRandomTile(gridPosition);

                    removedGridPositions.Add(gridPosition);
                }
                else
                {
                    //if there is a tile in the above gridposition put it in the place of the currently empty gridposition and empty its former position
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
            //remove all gridpositions that have managed to advance to the top of the grid.
            for (int i = 0; i < removedGridPositions.Count; i++)
            {
                emptyGridPositions.Remove(removedGridPositions[i]);
            }
            removedGridPositions.Clear();
        }

        OnFallingTilesCompleted?.Invoke();
    }

    private List<Vector3Int> RemoveTilesFromTheGrid(List<GridTileData> tilesToReplace)
    {
        List<Vector3Int> emptyGridPositions = new List<Vector3Int>();
        for (int i = 0; i < tilesToReplace.Count; i++)
        {
            var tilePosition = tilesToReplace[i].GridPosition;

            map.SetTile(tilePosition, null);
            emptyGridPositions.Add(tilePosition);
        }

        return emptyGridPositions;
    }
}