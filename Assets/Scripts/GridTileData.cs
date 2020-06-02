using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTileData 
{
    public Vector3Int GridPosition;
    public Tile Tile;

    public GridTileData(Vector3Int positiion, Tile tile)
    {
        GridPosition = positiion;
        Tile = tile;
    }

    public override bool Equals(object obj)
    {
        return obj is GridTileData data &&
               GridPosition.Equals(data.GridPosition);
    }

    public override int GetHashCode()
    {
        return -172710810 + GridPosition.GetHashCode();
    }

    public bool IsTileValidNeighbor(GridTileData other)
    {
        bool result = false;

        //are the tiles sharing the same tileasset
        if (other.Tile == Tile)
        {
            //is the other tile not this tile
            if (other.GridPosition != GridPosition)
            {
                //is the other left or right of this tile
                if (other.GridPosition.y <= GridPosition.y + 1 && other.GridPosition.y >= GridPosition.y - 1)
                {
                    //due to the nature of the hexagongrid the coordinates may only change on the y-coordinates depending on the positioning of the origin-tile 
                    //(on even y-positions the top-left and top-right and on odd y-positions the bottom-left and bottom-right)
                    if((GridPosition.y % 2 == 0 && other.GridPosition.x > GridPosition.x) ||
                        (GridPosition.y % 2 != 0 && other.GridPosition.x < GridPosition.x))
                    {
                        if(other.GridPosition.y == GridPosition.y)
                        {
                            result = true;
                        }
                    }
                    else if (other.GridPosition.x <= GridPosition.x + 1 && other.GridPosition.x >= GridPosition.x - 1)
                    {
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    public static bool operator ==(GridTileData left, GridTileData right)
    {
        return EqualityComparer<GridTileData>.Default.Equals(left, right);
    }

    public static bool operator !=(GridTileData left, GridTileData right)
    {
        return !(left == right);
    }
}
