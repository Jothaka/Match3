using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<int> OnBlocksRemoved;
    
    public GridController GridController;
    public HighlightView Highlight;

    private List<GridTileData> currentTiles = new List<GridTileData>();

    private bool interactionAllowed = false;

    void Start()
    {
        GridController.OnFallingTilesCompleted += OnStartInteraction;
    }
   
    void Update()
    {
        if (interactionAllowed)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                SelectTiles();
            }
            else
            {
                if (currentTiles.Count >= 1)
                {
                    Highlight.ClearAllHighlights();

                    if (currentTiles.Count >= GameController.MINBLOCKSREMOVED)
                    {
                        OnBlocksRemoved?.Invoke(currentTiles.Count);
                        GridController.RefillLevel(currentTiles);
                        interactionAllowed = false;
                    }

                    currentTiles.Clear();
                }
            }
        }
    }

    private void SelectTiles()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GridTileData selectedTileData = GridController.GetTileDataFromPosition(mouseWorldPos);
        if (selectedTileData != null)
        {
            if (currentTiles.Count == 0)
            {
                currentTiles.Add(selectedTileData);

                Highlight.SetHighlight(selectedTileData.GridPosition);
            }
            else
            {
                var lastTileData = currentTiles[currentTiles.Count - 1];
                if (!currentTiles.Contains(selectedTileData))
                {
                    if (lastTileData.IsTileValidNeighbor(selectedTileData))
                    {
                        currentTiles.Add(selectedTileData);
                        Highlight.SetHighlight(selectedTileData.GridPosition);
                    }
                }
                else
                {
                    if (lastTileData != selectedTileData)
                    {
                        if (selectedTileData == currentTiles[currentTiles.Count - 2])
                        {
                            Highlight.RemoveHighlight(lastTileData.GridPosition);
                            currentTiles.Remove(lastTileData);
                        }
                    }
                }
            }
        }
    }
    private void OnStartInteraction()
    {
        interactionAllowed = true;
    }
}
