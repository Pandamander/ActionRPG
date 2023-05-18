using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileBehavior
{
    public bool IsShaded;
}

public class TileMapManager : MonoBehaviour
{
    private Tilemap tileMap;

    [SerializeField]
    private List<TileData> tileDataList;

    private Dictionary<TileBase, TileData> tileBaseDataMap;

    private void Awake()
    {
        tileMap = GetComponent<Tilemap>();

        tileBaseDataMap = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDataList)
        {
            foreach (var tile in tileData.tiles)
            {
                tileBaseDataMap.Add(tile, tileData);
            }
        }
    }

    public TileBehavior GetBehavoirForCurrentTile(Vector2 worldPosition)
    {
        Vector3Int gridPosition = tileMap.WorldToCell(worldPosition);
        TileBase tile = tileMap.GetTile(gridPosition);

        if (tile == null) return new TileBehavior { IsShaded = false };

        if (!tileBaseDataMap.ContainsKey(tile)) return new TileBehavior { IsShaded = false };
        TileData tileData = tileBaseDataMap[tile];

        return new TileBehavior { IsShaded = tileData.shaded };
    }
}
