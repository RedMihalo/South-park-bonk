﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GameObject TilePrefab;
    public Vector2Int GridSize;
    public Vector2 TilesMargin;
    List<Tile> tiles = new List<Tile>();

    private static GridController _controller;
    public static GridController GetGridController()
    {
        return _controller;
    }

    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public static int ManhattanDistance(Tile a, Tile b)
    {
        return ManhattanDistance(a.PositionInGrid, b.PositionInGrid);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _controller = this;
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
                lastSpawnedTile.PositionInGrid = new Vector2Int(x, y);
                tiles.Add(lastSpawnedTile);

                spawnPosition.x += lastSpawnedTile.GetWorldBounds().size.x + TilesMargin.x;
            }
            spawnPosition.y -= lastSpawnedTile.GetWorldBounds().size.y + TilesMargin.y;
        }
        foreach(Tile t in tiles)
            t.transform.Translate(new Vector3(t.GetWorldBounds().size.x / 2, -t.GetWorldBounds().size.y / 2, 0));
    }

    public List<Tile> GetTiles()
    {
        List<Tile> toReturn = new List<Tile>();
        tiles.ForEach((Tile t) => toReturn.Add(t));
        return toReturn;
    }

    public Tile GetTile(Vector2Int positionInGrid)
    {
        return tiles.Find((Tile t) => { return t.PositionInGrid == positionInGrid; });
    }

    public void MoveUnit(GameObject unit, Vector2Int positionInGrid)
    {
        MoveUnit(unit, GetTile(positionInGrid));
    }

    public void MoveUnit(GameObject unit, Tile target)
    {
        if(unit.GetComponent<BattleUnit>().CurrentTile != null)
            unit.GetComponent<BattleUnit>().CurrentTile.CurrentUnit = null;

        unit.GetComponent<BattleUnit>().CurrentTile = target;
        unit.GetComponent<ObjectMover>().SetCurrentTarget(target.UnitPosition);
        target.CurrentUnit = unit;
    }

    public void EnableValidTiles(Predicate<Tile> Predicate)
    {
        foreach(var T in tiles)
            T.SetTileTargetable(Predicate(T));
    }

    public void SetGridEnabled(bool bEnabled)
    {
        foreach(var T in tiles)
            T.SetTileTargetable(bEnabled);
    }

    public static Tile GetDestinationTileInRange(Tile start, Tile dest, int range)
    {
        if(ManhattanDistance(start, dest) <= range)
            return dest;
        List<Tile> open = GridController.GetGridController().GetTiles();
        List<Tile> toVisit = new List<Tile>();
        Stack<Tile> visited = new Stack<Tile>();

        Tile current = start;
        while(current != null)
        {
            visited.Push(current);
            open.Remove(current);
            List<Tile> neighbours = current.GetNeighbors();
            while(neighbours.Count == 0 && current != null)
            {
                current = visited.Pop();
                neighbours = current.GetNeighbors();
            }

            foreach(Tile t in current.GetNeighbors())
            {
                if(!visited.Contains(t) && open.Contains(t))
                    toVisit.Add(t);
            }
            if(visited.Count == range || current == null || open.Count == 0)
                break;
            toVisit.Sort((Tile a, Tile b) => ManhattanDistance(a, dest) - ManhattanDistance(b, dest));
            current = toVisit[0];
        }

        return current;
    }


}
