using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapWaveFunctionSolver : MonoBehaviour
{
    public Tilemap inputTilemap;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    
    //list of arrays of valid tiles for each tile
    public List<TileBase> allTiles = new List<TileBase>();
    public List<List<TileBase>[]> possibleTiles = new List<List<TileBase>[]>();
    
    public List<Vector3Int> tilesToCheck = new List<Vector3Int>();

    public Vector3Int bounds;
    
    
    public Vector3Int debugPos;
    public Vector3Int secondDebugPos;
    public Vector3Int thirdDebugPos;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(debugPos+new Vector3(0.5f,0.5f), Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(secondDebugPos+new Vector3(0.5f,0.5f), Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(thirdDebugPos+new Vector3(0.5f,0.5f), Vector3.one);
    }

    public async void GatherPossibleTiles()
    {
        var bounds = inputTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y =  bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase tile = inputTilemap.GetTile(new Vector3Int(x, y, 0));
                
                List<TileBase>[] possibleNeighbors = new List<TileBase>[4];
    
                if (tile == null)
                {
                    continue;
                }
                debugPos = new Vector3Int(x, y, 0);
                await Task.Delay(10);
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    Vector3Int offset = new Vector3Int(0, 0, 0);
                    possibleNeighbors[i] = new List<TileBase>();
                    switch (i)
                    {
                        case 0:
                            offset = new Vector3Int(1, 0, 0);
                            break;
                        case 1:
                            offset = new Vector3Int(0, 1, 0);
                            break;
                        case 2:
                            offset = new Vector3Int(-1, 0, 0);
                            break;
                        case 3:
                            offset = new Vector3Int(0, -1, 0);
                            break;
                    }
                    
                    TileBase tile2 = inputTilemap.GetTile(pos + offset);
                    
                    if (tile2 == null || possibleNeighbors[i].Contains(tile2))
                    {
                       continue;
                    }
                    secondDebugPos = pos + offset;
                    await Task.Delay(10);
                    possibleNeighbors[i].Add(tile2);
                }
                if (!allTiles.Contains(tile))
                {
                    allTiles.Add(tile);
                    possibleTiles.Add(possibleNeighbors);
                } else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        possibleTiles[allTiles.IndexOf(tile)][i].AddRange(possibleNeighbors[i]);
                        possibleTiles[allTiles.IndexOf(tile)][i] = new List<TileBase>(new HashSet<TileBase>(possibleTiles[allTiles.IndexOf(tile)][i]));
                        
                    }
                }
            }
        }
        inputTilemap.gameObject.SetActive(false);

    }

    public void DisplayPossibleCombos()
    {
        floorTilemap.gameObject.SetActive(true);
        floorTilemap.ClearAllTiles();
        
        for (var i = 0; i < allTiles.Count; i++)
        {
            floorTilemap.SetTile(new Vector3Int(i*10, 0, 0), allTiles[i]);
            for (int j = 0; j < 4; j++)
            {
                Vector3Int direction = new Vector3Int(0, 0, 0);
                    
                switch (j)
                {
                    case 0:
                        direction = new Vector3Int(1, 0, 0);
                        break;
                    case 1:
                        direction = new Vector3Int(0, 1, 0);
                        break;
                    case 2:
                        direction = new Vector3Int(-1, 0, 0);
                        break;
                    case 3:
                        direction = new Vector3Int(0, -1, 0);
                        break;
                }
                for (int k = 0; k < possibleTiles[i][j].Count; k++)
                {
                    floorTilemap.SetTile(new Vector3Int(i*10, 0, 0)+direction*(k+1), possibleTiles[i][j][k]);
                }
            }
        }
    }

    public async void Solve()
    {
        floorTilemap.gameObject.SetActive(true);
        floorTilemap.ClearAllTiles();
        // wallTilemap.ClearAllTiles();
     
        tilesToCheck.Clear();
        floorTilemap.SetTile(new Vector3Int(0, 0, 0), allTiles[Random.Range(0, allTiles.Count)]);
        tilesToCheck.Add(new Vector3Int(0, 0, 0));

        while (tilesToCheck.Count > 0)
        {
            Vector3Int pos = tilesToCheck[0];
            
            tilesToCheck.RemoveAt(0);
            TileBase currentTile = floorTilemap.GetTile(pos);
            debugPos = pos;
            await Task.Delay(10);
            if (currentTile == null)
            {
                continue;
            }
            int currentIndex = allTiles.IndexOf(currentTile);
            List<TileBase>[] possibleNeighbors = new List<TileBase>[4];
            for (int i = 0; i < 4; i++)
            {
                possibleNeighbors[i] = new List<TileBase>(possibleTiles[currentIndex][i]);
                Vector3Int direction = new Vector3Int(0, 0, 0);
                    
                switch (i)
                {
                    case 0:
                        direction = new Vector3Int(1, 0, 0);
                        break;
                    case 1:
                        direction = new Vector3Int(0, 1, 0);
                        break;
                    case 2:
                        direction = new Vector3Int(-1, 0, 0);
                        break;
                    case 3:
                        direction = new Vector3Int(0, -1, 0);
                        break;
                }
                TileBase neighborTile = floorTilemap.GetTile(pos + direction);
                if (neighborTile != null)
                {
                    continue;
                }
                secondDebugPos = pos + direction;
                await Task.Delay(10);
                for (int j = 3; j >= 0; j--)
                {
                    Vector3Int offset = new Vector3Int(0, 0, 0);
                    
                    switch (j)
                    {
                        case 0:
                            offset = new Vector3Int(1, 0, 0);
                            break;
                        case 1:
                            offset = new Vector3Int(0, 1, 0);
                            break;
                        case 2:
                            offset = new Vector3Int(-1, 0, 0);
                            break;
                        case 3:
                            offset = new Vector3Int(0, -1, 0);
                            break;
                    }
                    
                    TileBase tempTile = floorTilemap.GetTile(pos + direction - offset);
                    if (tempTile == null)
                    {
                        continue;
                    }

                    thirdDebugPos = pos + direction - offset;
                    await Task.Delay(10);
                    int index = allTiles.IndexOf(tempTile);
                    List<TileBase> temp = new List<TileBase>(possibleTiles[index][j]);
                    possibleNeighbors[i].RemoveAll(x => !temp.Contains(x));
                }
                
                if (possibleNeighbors[i].Count > 0)
                {
                    floorTilemap.SetTile(pos + direction, possibleNeighbors[i][Random.Range(0, possibleNeighbors[i].Count)]);
                    //if in bounds, add to list of tiles to check
                    if (pos.x + direction.x < bounds.x && pos.x + direction.x > -bounds.x && pos.y + direction.y < bounds.y && pos.y + direction.y > -bounds.y && !tilesToCheck.Contains(pos + direction))
                    {
                        tilesToCheck.Add(pos + direction);
                    }
                    await Task.Delay(10);
                }
            }
        }
    }

    public Vector3Int GetLowestEntropyTile()
    {
        Vector3Int lowestEntropyTile = new Vector3Int(0, 0, 0);
        int lowestEntropy = 100000;
        for (int x = -bounds.x; x < bounds.x; x++)
        {
            for (int y = -bounds.y; y < bounds.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = floorTilemap.GetTile(pos);
                if (tile == null)
                {
                    continue;
                }
                int index = allTiles.IndexOf(tile);
                List<TileBase>[] possibleNeighbors = possibleTiles[index];
                int entropy = 0;
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int offset = new Vector3Int(0, 0, 0);
                    
                    switch (i)
                    {
                        case 0:
                            offset = new Vector3Int(1, 0, 0);
                            break;
                        case 1:
                            offset = new Vector3Int(0, 1, 0);
                            break;
                        case 2:
                            offset = new Vector3Int(-1, 0, 0);
                            break;
                        case 3:
                            offset = new Vector3Int(0, -1, 0);
                            break;
                    }
                    
                    TileBase tile2 = floorTilemap.GetTile(pos + offset);
                    if (tile2 == null)
                    {
                        entropy += possibleNeighbors[i].Count;
                    }
                }

                if (entropy < lowestEntropy)
                {
                    lowestEntropy = entropy;
                    lowestEntropyTile = pos;
                }
            }
        }
        return lowestEntropyTile;
    }
}
