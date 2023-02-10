using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapWaveFunctionSolver : MonoBehaviour
{
    public Tilemap inputTilemap;
    public Tilemap output;

    public List<TileBase> allTiles = new List<TileBase>();
    public List<List<TileBase>[]> possibleTiles = new List<List<TileBase>[]>();
    public List<Vector3Int> tilesToCheck = new List<Vector3Int>();

    public TileBase defaultTile;
    
    public Vector3Int bounds;//TODO Move this to a global settings class

    public Vector3Int debugPos;
    public Vector3Int secondDebugPos;
    public Vector3Int thirdDebugPos;
    public bool debug = false;
    public int debugDelay = 10;

    public void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(debugPos + new Vector3(0.5f, 0.5f), Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(secondDebugPos + new Vector3(0.5f, 0.5f), Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(thirdDebugPos + new Vector3(0.5f, 0.5f), Vector3.one);
    }

    private IEnumerator Start()
    {
        yield return GatherPossibleTiles();
        yield return Solve();
        yield return null;
    }

    public IEnumerator GatherPossibleTiles()
    {
        var bounds = inputTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase tile = inputTilemap.GetTile(new Vector3Int(x, y, 0));

                List<TileBase>[] possibleNeighbors = new List<TileBase>[4];

                if (tile == null)
                {
                    continue;
                }

                if (debug)
                {
                    debugPos = new Vector3Int(x, y, 0);
                    yield return new WaitForSeconds(debugDelay);
                }

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

                    if (debug)
                    {
                        secondDebugPos = pos + offset;
                        yield return new WaitForSeconds(debugDelay);
                    }

                    possibleNeighbors[i].Add(tile2);
                }

                if (!allTiles.Contains(tile))
                {
                    allTiles.Add(tile);
                    possibleTiles.Add(possibleNeighbors);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        possibleTiles[allTiles.IndexOf(tile)][i].AddRange(possibleNeighbors[i]);
                        possibleTiles[allTiles.IndexOf(tile)][i] =
                            new List<TileBase>(new HashSet<TileBase>(possibleTiles[allTiles.IndexOf(tile)][i]));

                    }
                }
            }
        }

        inputTilemap.gameObject.SetActive(false);

    }


    public void DisplayPossibleCombos()
    {
        output.gameObject.SetActive(true);
        output.ClearAllTiles();

        for (var i = 0; i < allTiles.Count; i++)
        {
            output.SetTile(new Vector3Int(i * 10, 0, 0), allTiles[i]);
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
                    output.SetTile(new Vector3Int(i * 10, 0, 0) + direction * (k + 1), possibleTiles[i][j][k]);
                }
            }
        }
    }

    public IEnumerator Solve()
    {
        output.gameObject.SetActive(true);
        output.ClearAllTiles();
        // wallTilemap.ClearAllTiles();

        tilesToCheck.Clear();
        output.SetTile(new Vector3Int(0, 0, 0), allTiles[Random.Range(0, allTiles.Count)]);
        tilesToCheck.Add(new Vector3Int(0, 0, 0));


        while (tilesToCheck.Count > 0)
        {
            Vector3Int pos = GetLowestEntropyTile();

            tilesToCheck.Remove(pos);

            TileBase currentTile = output.GetTile(pos);

            if (debug)
            {
                debugPos = pos;
                yield return new WaitForSeconds(debugDelay);
            }

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

                TileBase neighborTile = output.GetTile(pos + direction);
                if (neighborTile != null)
                {
                    continue;
                }

                if (debug)
                {
                    secondDebugPos = pos + direction;
                    yield return new WaitForSeconds(debugDelay);
                }

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

                    TileBase tempTile = output.GetTile(pos + direction - offset);
                    if (tempTile == null)
                    {
                        continue;
                    }

                    if (debug)
                    {
                        thirdDebugPos = pos + direction - offset;
                        yield return new WaitForSeconds(debugDelay);
                    }

                    int index = allTiles.IndexOf(tempTile);
                    List<TileBase> temp = new List<TileBase>(possibleTiles[index][j]);
                    possibleNeighbors[i].RemoveAll(x => !temp.Contains(x));
                }

                if (possibleNeighbors[i].Count > 0)
                {
                    output.SetTile(pos + direction, possibleNeighbors[i][Random.Range(0, possibleNeighbors[i].Count)]);
                    //if in bounds, add to list of tiles to check
                    if (pos.x + direction.x < bounds.x && pos.x + direction.x > -bounds.x &&
                        pos.y + direction.y < bounds.y && pos.y + direction.y > -bounds.y &&
                        !tilesToCheck.Contains(pos + direction))
                    {
                        tilesToCheck.Add(pos + direction);
                    }

                    if (debug)
                    {
                        yield return new WaitForSeconds(debugDelay);
                    }
                }
                else
                {
                    output.SetTile(pos, defaultTile);
                    break;
                }
            }
        }
    }

    public Vector3Int GetLowestEntropyTile()
    {
        Vector3Int lowestEntropyTile = new Vector3Int(0, 0, 0);
        int lowestEntropy = 100000;

        for (int i = 0; i < tilesToCheck.Count; i++)
        {
            int entropy = 0;
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

                TileBase neighborTile = output.GetTile(tilesToCheck[i] + direction);
                if (neighborTile == null)
                {
                    entropy += possibleTiles[allTiles.IndexOf(output.GetTile(tilesToCheck[i]))][j].Count;
                }
            }

            if (entropy < lowestEntropy)
            {
                lowestEntropy = entropy;
                lowestEntropyTile = tilesToCheck[i];
            }
        }

        return lowestEntropyTile;
    }
}