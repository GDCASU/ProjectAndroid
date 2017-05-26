using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMap : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    [Header("Prototype Settings")]
    public int numTargetsCase1;
    public int numTargetsCase2;
    public int numBlocksCase2;
    public int numTargetsCase3;
    public int numTargetsCase4;
    public int numBlocksCase4;
    public Text progressText;
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject player;
    public GameObject enemy;

    public Tile[,] map;

    private int protoTaskIndex;
    private int protoTargetCounter = 0;
    private DemoEnemy[] enemies;

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
            {
                map[i, j] = Instantiate(tilePrefab, new Vector3(i, 0, j) + offset, Quaternion.identity, transform).GetComponent<Tile>();
                map[i, j].mapPos.x = i; 
                map[i, j].mapPos.y = j;
            }
                

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
        //spawn player in center
        SpawnUnit(map[mapWidth/2, mapHeight/2], player);

        switch (protoTaskIndex)
        {
            case 1:
                Tile target1 = RandomTile(true);
                target1.protoTarget = true;
                target1.SetColor(Color.yellow);
                protoTargetCounter = numTargetsCase1;
                break;
            case 2:
                for (int i = 0; i < numBlocksCase2; i++)
                    RandomTile(true).MakeBlock();
                Tile target2 = RandomTile(true);
                target2.protoTarget = true;
                target2.SetColor(Color.yellow);
                protoTargetCounter = numTargetsCase2;
                break;
            case 3:
                enemies = new DemoEnemy[numTargetsCase3];
                for (int i = 0; i < numTargetsCase3; i++)
                    enemies[i] = (DemoEnemy)SpawnUnit(RandomTile(true), enemy);

                protoTargetCounter = numTargetsCase3;
                enemies[protoTargetCounter - 1].SetActive();
                break;
            case 4:
                for (int i = 0; i < numBlocksCase4; i++)
                    RandomTile(true).MakeBlock();
                enemies = new DemoEnemy[numTargetsCase4];
                for (int i = 0; i < numTargetsCase4; i++)
                    enemies[i] = (DemoEnemy)SpawnUnit(RandomTile(true), enemy);

                protoTargetCounter = numTargetsCase4;
                enemies[protoTargetCounter - 1].SetActive();
                break;
            default:
                break;
        }
        progressText.text = protoTargetCounter.ToString();
    }

    public void ChangeTestCase(int newCase)
    {
        protoTaskIndex = newCase;
        CleanMap();
        SetupMap();
    }

    public bool MoveUnit(Tile oldTile, Tile newTile, Unit unit) //change unit type to Unit when possible
    {
        if (newTile.unit || newTile.impassible) return false;

        oldTile.unit = null;
        newTile.unit = unit;
        unit.transform.SetParent(newTile.transform, true);
        unit.occupiedTile = newTile;
        unit.transform.position = newTile.transform.position;
        
        if (newTile.protoTarget)
        {
            newTile.protoTarget = false;
            newTile.SetColor(Color.white);
            protoTargetCounter--;
            if (protoTargetCounter > 0)
            {
                Tile target = RandomTile(true);
                target.protoTarget = true;
                target.SetColor(Color.yellow);
            }
            progressText.text = protoTargetCounter.ToString();
        }
        return true;
    }

    public void DamageTile(Tile tile)
    {
        if (tile == null || tile.unit == null || !(tile.unit is DemoEnemy)) return;

        
        if (((DemoEnemy)tile.unit).target)
        {
            tile.unit.Damaged();
            protoTargetCounter--;
            if (protoTargetCounter > 0)
                enemies[protoTargetCounter - 1].SetActive();
            progressText.text = protoTargetCounter.ToString();
        }
    }

    public Unit SpawnUnit(Tile tile, GameObject unit)
    {
        GameObject spawn = Instantiate(unit, tile.transform.position, Quaternion.Euler(0,270,0));
        tile.unit = spawn.GetComponent<Unit>();
        tile.unit.transform.SetParent(tile.transform, true);
        tile.unit.occupiedTile = tile;
        tile.unit.tileMap = this;
        tile.unit.Rotate(1);
        return tile.unit;
    }

    public bool ValidPos(int x, int y)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return false;
        return true;
    }

    public Tile RandomTile(bool empty)
    {
        Tile res = map[Random.Range(0, mapWidth), Random.Range(0, mapHeight)];
        while (empty && (res.unit != null || res.impassible))
            res = map[Random.Range(0, mapWidth), Random.Range(0, mapHeight)];
        return res;
    }

    public Tile[] GetNeighbors(Tile tile)
    {
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
                if (map[i, j] == tile)
                    return GetNeighbors(i, j);
        return null;
    }

    public Tile[] GetNeighbors(int xPos, int yPos)
    {
        Tile[] res = new Tile[4]; // 0 = right, 1 = down, 2 = left, 3 = up
        Vector2 pos = new Vector2(xPos, yPos);
        Vector2[] dirMap = { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
        for(int i=0; i<4; i++)
        {
            Vector2 target = pos + dirMap[i];
            if (ValidPos((int)target.x, (int)target.y))
                res[i] = map[(int)target.x, (int)target.y];
            else
                res[i] = null;
        }
        return res;
    }
}
