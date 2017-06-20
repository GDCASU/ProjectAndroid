using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: Script attached to the Canvas in the Map Editor that handles all input from
//              buttons, fields, etc. in the canvas. Entirely responsible for all map editor
//              functionality.

public class EditorInputHandler : MonoBehaviour
{
    public SerializeDirectory directory;
    public InputField widthField;
    public InputField heightField;
    public InputField saveNameField;
    public InputField loadNameField;
    public Dropdown tileDropdown;
    public Dropdown unitDropdown;
    public TileMap tileMap;

    private int selectedTile;
    private int selectedUnit;

    public void Start()
    {
        //populate dropdowns
        List<Dropdown.OptionData> opts = new List<Dropdown.OptionData>();
        for (int i = 0; i < directory.tileList.Length; i++)
            opts.Add(new Dropdown.OptionData(directory.tileList[i].name));
        tileDropdown.ClearOptions();
        tileDropdown.AddOptions(opts);
        opts = new List<Dropdown.OptionData>();
        for (int i = 0; i < directory.unitList.Length; i++)
            opts.Add(new Dropdown.OptionData(directory.unitList[i].name));
        unitDropdown.ClearOptions();
        unitDropdown.AddOptions(opts);
        selectedTile = 0;
        selectedUnit = 0;
        widthField.text = tileMap.mapWidth.ToString();
        heightField.text = tileMap.mapHeight.ToString();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            foreach (RectTransform rt in GetComponentsInChildren<RectTransform>())
            {
                if (!rt.gameObject.name.Equals("Canvas") && RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition))
                {
                    return;
                }
            }

            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 worldPos = tile.transform.position;
                    Vector2 pos = tile.mapPos;
                    Destroy(tile.gameObject);
                    Tile newTile = Instantiate(directory.tileList[selectedTile], worldPos, Quaternion.identity, tileMap.transform).GetComponent<Tile>();
                    tileMap.map[(int)pos.x, (int)pos.y] = newTile;
                    newTile.mapPos = pos;
                    if (newTile.tag == "Entrance")
                        newTile.SetColor(Color.green);
                    else if (newTile.tag == "Exit")
                        newTile.SetColor(Color.red);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (tile.unit) Destroy(tile.unit.gameObject);
                    tileMap.SpawnUnit(tile, directory.unitList[selectedUnit]);
                }
            }
        }
    }

    public void ChangeSelectedTile(int index)
    {
        selectedTile = index;
    }

    public void ChangeSelectedUnit(int index)
    {
        selectedUnit = index;
    }

    public void ChangeMapSize()
    {
        tileMap.mapWidth = int.Parse(widthField.text);
        tileMap.mapHeight = int.Parse(heightField.text);
        Vector3 offset = new Vector3(-tileMap.mapWidth / 2f + 0.5f, 0, -tileMap.mapHeight / 2f + 0.5f);
        Tile[,] oldMap = tileMap.map;
        tileMap.map = new Tile[tileMap.mapWidth, tileMap.mapHeight];

        int dx = (tileMap.mapWidth - oldMap.GetLength(0)) / 2;
        int dy = (tileMap.mapHeight - oldMap.GetLength(1)) / 2;

        for (int i = 0; i < oldMap.GetLength(0); i++)
            for (int j = 0; j < oldMap.GetLength(1); j++)
            {
                if (CheckValidIndex(tileMap.map, i + dx, j + dy))
                {
                    tileMap.map[i + dx, j + dy] = oldMap[i, j];
                    tileMap.map[i + dx, j + dy].transform.position = new Vector3(i+dx, 0, j+dy) + offset;
                    tileMap.map[i + dx, j + dy].mapPos = new Vector2(i + dx, j + dy);
                }
                else
                    Destroy(oldMap[i, j].gameObject);
            }

        for (int i = 0; i < tileMap.mapWidth; i++)
            for (int j = 0; j < tileMap.mapHeight; j++)
            {
                if (!tileMap.map[i, j])
                {
                    tileMap.map[i, j] = Instantiate(directory.tileList[0], new Vector3(i, 0, j) + offset, Quaternion.identity, tileMap.transform).GetComponent<Tile>();
                    tileMap.map[i, j].mapPos = new Vector2(i, j);
                }
            }
    }

    private bool CheckValidIndex(Tile[,] map, int x, int y)
    {
        return (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1));
    }

    public void ResetMap()
    {
        tileMap.SetupMap();
    }

    public void Save()
    {
        tileMap.SaveMapToFile(saveNameField.text);
    }

    public void Load()
    {
        tileMap.LoadMapFromFile(loadNameField.text);
        widthField.text = tileMap.mapWidth.ToString();
        heightField.text = tileMap.mapHeight.ToString();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Entrance"))
            go.GetComponent<Tile>().SetColor(Color.green);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Exit"))
            go.GetComponent<Tile>().SetColor(Color.red);
    }
}
