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
    public int numTasks;
    public Text progressText;
    public Text instructionText;
    public TaskTimer timer;
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject player;
    public GameObject enemy;

    public Tile[,] map;

    private int protoTaskIndex;
    private int protoTargetMax = 0;
    private int protoTargetCounter = 0;
    private DemoEnemy[] enemies;
    private Tile[] targetTiles;

    private void Start()
    {
        SetupMap();
    }

    public void SetupMap()
    {
        CleanMap();
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
        if (map != null)
            for (int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapHeight; j++)
                    if (map[i, j])
                        Destroy(map[i, j].gameObject);
    }

    public void SetupTestCase()
    {
        int[] xPos;
        int[] yPos;
        switch (protoTaskIndex)
        {
            case 1:
                //player
                SpawnUnit(map[3, 3], player);
                //target tiles
                xPos = new int[] { 1, 4, 1, 5 };
                yPos = new int[] { 1, 0, 3, 5 };
                targetTiles = new Tile[xPos.Length];
                for (int i = 0; i < xPos.Length; i++)
                    targetTiles[i] = map[xPos[i], yPos[i]];
                targetTiles[0].protoTarget = true;
                targetTiles[0].SetColor(Color.yellow);
                //other related setup
                protoTargetCounter = xPos.Length;
                if (instructionText) instructionText.text = "Step on the yellow tiles";
                break;
            case 2:
                //player
                SpawnUnit(map[2, 3], player);
                //blocks
                xPos = new int[] { 0, 5, 0, 2, 3, 4, 1, 1, 2, 4, 4 };
                yPos = new int[] { 0, 0, 1, 1, 1, 2, 3, 4, 4, 4, 5 };
                for (int i = 0; i < xPos.Length; i++)
                    map[xPos[i], yPos[i]].MakeBlock();
                //target tiles
                xPos = new int[] { 0, 3, 4, 5 };
                yPos = new int[] { 5, 2, 0, 5 };
                targetTiles = new Tile[xPos.Length];
                for (int i = 0; i < xPos.Length; i++)
                    targetTiles[i] = map[xPos[i], yPos[i]];
                targetTiles[0].protoTarget = true;
                targetTiles[0].SetColor(Color.yellow);
                //other stuff
                protoTargetCounter = xPos.Length;
                if (instructionText) instructionText.text = "Step on the yellow tiles";
                break;
            case 3:
                //player
                SpawnUnit(map[2, 2], player);
                //enemies
                xPos = new int[] { 1, 4, 0, 5, 3 };
                yPos = new int[] { 0, 2, 4, 0, 5 };
                MovementPattern[] moveBehaviors = { MovementPattern.Horizontal, MovementPattern.Ordinal, MovementPattern.Ordinal, MovementPattern.Vertical, MovementPattern.Horizontal };
                enemies = new DemoEnemy[xPos.Length];
                for (int i = 0; i < xPos.Length; i++)
                {
                    enemies[i] = (DemoEnemy)SpawnUnit(map[xPos[i], yPos[i]], enemy);
                    enemies[i].moveType = moveBehaviors[i];
                }
                enemies[0].SetActive();
                //other stuff
                protoTargetCounter = xPos.Length;
                if (instructionText) instructionText.text = "Attack the red enemies";
                break;
            case 4:
                //player
                SpawnUnit(map[3, 2], player);
                //blocks
                xPos = new int[] { 2, 3, 0, 3, 0, 1, 5, 2, 4, 0, 0, 3, 4 };
                yPos = new int[] { 0, 0, 1, 1, 2, 2, 2, 3, 3, 4, 5, 5, 5 };
                for (int i = 0; i < xPos.Length; i++)
                    map[xPos[i], yPos[i]].MakeBlock();
                //enemies
                xPos = new int[] { 0, 5, 5, 0, 1 };
                yPos = new int[] { 3, 1, 4, 0, 5 };
                MovementPattern[] moveBehaviors2 = { MovementPattern.Horizontal, MovementPattern.Ordinal, MovementPattern.Vertical, MovementPattern.Horizontal, MovementPattern.Horizontal };
                enemies = new DemoEnemy[xPos.Length];
                for (int i = 0; i < xPos.Length; i++)
                {
                    enemies[i] = (DemoEnemy)SpawnUnit(map[xPos[i], yPos[i]], enemy);
                    enemies[i].moveType = moveBehaviors2[i];
                }
                enemies[0].SetActive();
                //other stuff
                protoTargetCounter = xPos.Length;
                if (instructionText) instructionText.text = "Attack the red enemies";
                break;
            default:
                break;
        }
        protoTargetMax = protoTargetCounter;
        if (progressText)
            progressText.text = "Progress: 0/" + protoTargetMax;
        if (timer) timer.resetTimer();
    }

    public void ChangeTestCase(int newCase)
    {
        protoTaskIndex = newCase;
        SetupMap();
    }

    public bool MoveUnit(Tile oldTile, Tile newTile, Unit unit) //change unit type to Unit when possible
    {
        if (newTile.unit || newTile.impassible) return false;

        oldTile.tileAPI.OnExit();
        oldTile.unit = null;
        newTile.unit = unit;
        newTile.tileAPI.OnEnter();
        //order is important for the tileAPI calls so that the unit is
        //occupying the tile that the method is called on when it is called.
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
                Tile target = targetTiles[protoTargetMax - protoTargetCounter];
                target.protoTarget = true;
                target.SetColor(Color.yellow);
            }
            if (progressText)
                progressText.text = "Progress: " + (protoTargetMax - protoTargetCounter) + "/" + protoTargetMax;
            if (protoTargetCounter == 0) CompleteTask();
        }
        return true;
    }

    public void DamageTile(Tile tile, int damage, int sourceID)
    {
        if (tile == null || tile.unit == null || tile.unit.getID() == sourceID) return;

        if (tile.unit is DemoEnemy && ((DemoEnemy)tile.unit).target)
        {
            protoTargetCounter--;
            if (protoTargetCounter > 0)
                enemies[protoTargetMax - protoTargetCounter].SetActive();
            if (progressText)
                progressText.text = "Progress: " + (protoTargetMax - protoTargetCounter) + "/" + protoTargetMax;
            if (protoTargetCounter == 0) CompleteTask();
        }

        tile.unit.Damaged(damage, sourceID);
    }
    public void HealTile(Tile tile, int health)
    {
        if (tile == null || tile.unit == null) return;

        tile.unit.Healed(health);
    }

    public void CompleteTask()
    {
        timer.pause();
        if (protoTaskIndex >= numTasks)
            instructionText.text = "All tasks completed. Returning to main screen...";
        else
            instructionText.text = "Task complete. Moving to next...";
        StartCoroutine(NextTask());
    }

    IEnumerator NextTask()
    {
        yield return new WaitForSeconds(3);
        timer.unpause();
        if (protoTaskIndex >= numTasks) GameObject.FindWithTag("Overlord").GetComponent<Overlord>().TasksCompleted();
        else
        {
            ChangeTestCase(protoTaskIndex + 1);
            SetupMap();
        }
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
}
