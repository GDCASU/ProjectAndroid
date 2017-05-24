using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject tilePrefab;
    public int mapWidth;
    public int mapHeight;
    [Header("Prototype Settings")]
    public int numTargetsCase1;
    public int numTargetsCase2;
    public int numBlocksCase2;
    public int numTargetsCase3;
    public int numTargetsCase4;
    public int numBlocksCase4;

    public Tile[,] map;

    private int protoTaskIndex;
    private int protoTargetCounter = 5;

    private void Start()
    {
        SetupMap();
    }

    public void SetupMap()
    {
        map = new Tile[mapWidth, mapHeight];
        Vector3 offset = new Vector3(-mapWidth / 2, 0, -mapHeight / 2);
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
                map[i, j] = Instantiate(tilePrefab, new Vector3(i, 0, j) + offset, Quaternion.identity, transform).GetComponent<Tile>();

        SetupTestCase();
    }

    public void CleanMap()
    {
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
                Destroy(map[i, j].gameObject);
    }

    public void SetupTestCase()
    {
        switch (protoTaskIndex)
        {
            case 1:
                Tile target1 = RandomTile();
                target1.protoTarget = true;
                target1.SetColor(Color.yellow);
                protoTargetCounter = numTargetsCase1;
                break;
            case 2:
                for (int i = 0; i < numBlocksCase2; i++)
                {
                    Tile block = RandomTile();
                    block.impassible = true;
                    block.SetColor(Color.black);
                }
                Tile target2 = RandomTile();
                target2.protoTarget = true;
                target2.SetColor(Color.yellow);
                protoTargetCounter = numTargetsCase2;
                break;
            case 3:
                Tile target3 = RandomTile();
                //spawn enemy on target3
                protoTargetCounter = numTargetsCase3;
                break;
            case 4:
                for (int i = 0; i < numBlocksCase4; i++)
                {
                    Tile block = RandomTile();
                    block.impassible = true;
                    block.SetColor(Color.black);
                }
                Tile target4 = RandomTile();
                //spawn enemy on target4
                protoTargetCounter = numTargetsCase4;
                break;
            default:
                break;
        }
    }

    public void ChangeTestCase(int newCase)
    {
        protoTaskIndex = newCase;
        CleanMap();
        SetupMap();
    }

    public bool MoveUnit(Tile oldTile, Tile newTile, Tile unit) //change unit type to Unit when possible
    {
        if (newTile.unit || newTile.impassible) return false;
        oldTile.unit = null;
        newTile.unit = unit;
        if(newTile.protoTarget)
        {
            newTile.protoTarget = false;
            newTile.SetColor(Color.white);
            if(protoTargetCounter > 0)
            {
                Tile target = RandomTile();
                target.protoTarget = true;
                target.SetColor(Color.yellow);
                protoTargetCounter--;
            }
        }
        return true;
    }

    public Tile RandomTile()
    {
        return map[Random.Range(0, mapWidth), Random.Range(0, mapHeight)];
    }
}
