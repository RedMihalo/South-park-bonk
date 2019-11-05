﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GameObject TilePrefab;
    public Vector2Int GridSize;
    public Vector2 TilesMargin;
    List<Tile> tiles = new List<Tile>();

    public GameObject MovedUnit;

    // Start is called before the first frame update
    private void Awake()
    {
        Vector3 spawnPosition = transform.position;
        Tile lastSpawnedTile = null;

        for(int y = 0; y < GridSize.y; ++y)
        {
            spawnPosition.x = transform.position.x;
            for(int x = 0; x < GridSize.x; ++x)
            {
                GameObject newTile = Instantiate(TilePrefab, spawnPosition, Quaternion.identity, transform);
                newTile.transform.localScale = TilePrefab.transform.localScale;
                lastSpawnedTile = newTile.GetComponent<Tile>();
                lastSpawnedTile.ParentController = this;
                tiles.Add(lastSpawnedTile);

                spawnPosition.x += lastSpawnedTile.GetWorldBounds().size.x + TilesMargin.x;
            }
            spawnPosition.y -= lastSpawnedTile.GetWorldBounds().size.y + TilesMargin.y;
        }
        foreach(Tile t in tiles)
            t.transform.Translate(new Vector3(t.GetWorldBounds().size.x / 2, -t.GetWorldBounds().size.y / 2, 0));
    }


    public void SetGridEnabled(bool bEnabled)
    {
        foreach(var T in tiles)
            T.SetTileTargetable(bEnabled);
    }

}