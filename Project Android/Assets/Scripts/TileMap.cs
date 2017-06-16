﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class TileMap : MonoBehaviour
{

    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    [Header("Prototype Settings")]
    public int numTasks;
    public Text progressText;
    public Text instructionText;
    public TaskTimer timer;
    public bool turnBased;
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Tile[,] map;

    private int protoTaskIndex;
    private int protoTargetMax = 0;
    private int protoTargetCounter = 0;
    private DemoEnemy[] enemies;
    private Player activePlayer;
    private Tile[] targetTiles;

    private void Start()
    {
        if(map == null)
            SetupMap();
    }

    void Update()
    {
        if(!turnBased && enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
                enemies[i].MoveUpdate();
        }
        if(activePlayer)
            activePlayer.GetComponent<Player>().MoveUpdate();
    }

    public void SetupMap()
    {
        CleanMap();
        map = new Tile[mapWidth, mapHeight];
        Vector3 offset = new Vector3(-mapWidth / 2f + 0.5f, 0, -mapHeight / 2f + 0.5f);
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                map[i, j] = Instantiate(tilePrefab, new Vector3(i, 0, j) + offset, Quaternion.identity, transform).GetComponent<Tile>();
                map[i, j].mapPos.x = i;
                map[i, j].mapPos.y = j;
            }
    }

    public void CleanMap()
    {
        if (map != null)
            for (int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapHeight; j++)
                    if (map[i, j])
                        Destroy(map[i, j].gameObject);
    }

    public void ChangeTestCase(int newCase)
    {
        protoTaskIndex = newCase;
        SetupMap();
    }

    public bool MoveUnit(Tile oldTile, Tile newTile, Unit unit) //change unit type to Unit when possible
    {
        if (newTile.unit || newTile.impassible) return false;

        oldTile.OnExit();
        oldTile.unit = null;
        newTile.unit = unit;
        newTile.OnEnter();
        //order is important for the tileAPI calls so that the unit is
        //occupying the tile that the method is called on when it is called.
        unit.transform.SetParent(newTile.transform, true);
        unit.occupiedTile = newTile;
        unit.transform.position = newTile.transform.position;

        if (unit is Player)
        {
            EnemyMoveStep();
        }
        return true;
    }

    public void EnemyMoveStep()
    {
        if (turnBased && enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
                enemies[i].Move();
        }
    }

    public void DamageTile(Tile tile, int damage, int sourceID)
    {
        if (tile == null || tile.unit == null || tile.unit.getID() == sourceID) return;

        tile.unit.Damaged(damage, sourceID);

        if (!(tile.unit is Player)) EnemyMoveStep(); //only move if player hit enemy - must change in future
    }

    public void HealTile(Tile tile, int health)
    {
        if (tile == null || tile.unit == null) return;

        tile.unit.Healed(health);
    }

    public Unit SpawnUnit(Tile tile, GameObject unit)
    {
        GameObject spawn = Instantiate(unit, tile.transform.position, Quaternion.Euler(0, 270, 0));
        tile.unit = spawn.GetComponent<Unit>();
        tile.unit.transform.SetParent(tile.transform, true);
        tile.unit.occupiedTile = tile;
        tile.unit.tileMap = this;
        tile.unit.Rotate(1);
        return tile.unit;
    }

    public bool ValidPos(int x, int y)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight || map[x, y].impassible) return false;
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
        for (int i = 0; i < 4; i++)
        {
            Vector2 target = pos + dirMap[i];
            if (ValidPos((int)target.x, (int)target.y))
                res[i] = map[(int)target.x, (int)target.y];
            else
                res[i] = null;
        }
        return res;
    }

    public void BFSPathFinding(int xPos, int yPos)
    {
        Tile currentTile;
        Tile[] neighbors = new Tile[4];       
        List<Tile> tileList = new List<Tile>();
        Queue<Tile> tileQueue = new Queue<Tile>();
        map[xPos, yPos].direction = 0;
        tileList.Add(map[xPos, yPos]);
        tileQueue.Enqueue(map[xPos, yPos]);
        while (tileQueue.Count > 0)
        {
            currentTile = tileQueue.Dequeue();
            neighbors = GetNeighbors(currentTile);
            for (int i = 0; i < 4; i++)
                if (neighbors[i] && !tileList.Contains(neighbors[i]))
                {
                    tileList.Add(neighbors[i]);
                    neighbors[i].direction = (i + 2) % 4;
                    tileQueue.Enqueue(neighbors[i]);
                }
        }
    }

    public void LoadMapFromFile(string filename)
    {
        if (!Directory.Exists(Application.dataPath + "/Maps/")) Directory.CreateDirectory(Application.dataPath + "/Maps/");
        FileStream file = File.OpenRead(Application.dataPath + "/Maps/" + filename);
        Deserialize(file);
        file.Close();
    }

    public void SaveMapToFile(string filename)
    {
        if (!Directory.Exists(Application.dataPath + "/Maps/")) Directory.CreateDirectory(Application.dataPath + "/Maps/");
        FileStream file = File.OpenWrite(Application.dataPath + "/Maps/" + filename);
        Serialize(file);
        file.Close();
    }

    [System.Serializable]
    public class SaveFormat
    {
        public short mapWidth;
        public short mapHeight;
        public List<SerializableTile> tiles; 
    }

    [System.Serializable]
    public class SerializableTile
    {
        public byte tileID; //currently meaningless
        public byte unitID; //arbitrary - using 0 = none, 1 = demoenemy, 2 = melee enemy, 3 = ranged enemy
        public bool impassible;
    }

    public void Serialize(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        SaveFormat save = new SaveFormat();
        save.mapWidth = (short)mapWidth;
        save.mapHeight = (short)mapHeight;
        save.tiles = new List<SerializableTile>();
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                SerializableTile tile = new SerializableTile();
                tile.tileID = 0;
                tile.unitID = 0;
                if(map[i,j].unit)
                {
                    if (map[i, j].unit is DemoEnemy) tile.unitID = 1;
                    else if (map[i, j].unit is SimpleMeleeEnemy) tile.unitID = 2;
                    else if (map[i, j].unit is SimpleRangedEnemy) tile.unitID = 3;
                }
                tile.impassible = map[i, j].impassible;
                save.tiles.Add(tile); //index equals j + i*mapHeight
            }
        }
        bf.Serialize(fs, save);
    }

    public void Deserialize(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        SaveFormat save = (SaveFormat)bf.Deserialize(fs);
        if (save == null) return;
        CleanMap();
        mapWidth = save.mapWidth;
        mapHeight = save.mapHeight;
        map = new Tile[mapWidth, mapHeight];
        Vector3 offset = new Vector3(-mapWidth / 2f + 0.5f, 0, -mapHeight / 2f + 0.5f);
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                SerializableTile sTile = save.tiles[i * mapHeight + j];
                map[i, j] = Instantiate(tilePrefab, new Vector3(i, 0, j) + offset, Quaternion.identity, transform).GetComponent<Tile>();
                map[i, j].mapPos.x = i;
                map[i, j].mapPos.y = j;
                if (sTile.impassible)
                    map[i, j].MakeBlock();
                if(sTile.unitID > 0)
                {
                    SpawnUnit(map[i, j], enemyPrefab);
                }
            }
    }
}
