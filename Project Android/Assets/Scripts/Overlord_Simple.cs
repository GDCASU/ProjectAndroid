using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: This script is a version of the Overlord with most features cut. Instead of sending
//              the player to an Overworld, it simply sends them through a sequence of levels according
//              to an array specified in the inspector.

public class Overlord_Simple : MonoBehaviour
{
    [Header("Controls")]
    public GameObject[] controlButtons; //buttons on title screen
    public GameObject[] controls; //actual UI elements for movement controls
    public GameObject attackPanel; //UI element for attack buttons
    public bool leftHanded = false; //if true, swap attack and controls
    public int selectedControl;

    [Header("Scene names")]
    public string titleScreenScene;
    public string inGameScene;

    [Header("Ship config")]
    //the below do not include mandatory engine/cockpit rooms
    public int smallShipRooms;
    public int mediumShipRooms;
    public int largeShipRooms;
    public int[] smallRoomCount;
    public int[] mediumRoomCount;
    public int[] largeRoomCount;

    [Header("Current ship info")]
    public string[] levels; //levels to traverse through for the current ship, or null if not in ship
    public int currentLevel; //index of above
    public OverworldShip currentShip;

    public TileMap activeTileMap;

    static bool started = false; //to ensure only one overlord exists

    void Awake()
    {
        if (started) Destroy(gameObject);
        started = true;
        DontDestroyOnLoad(gameObject);

        AssignActiveMap();
    }

    public void SetupControlButtons()
    {
        GameObject controlSchemeContainer = GameObject.Find("ControlSchemes");
        for (int i = 0; i < controlButtons.Length; i++)
        {
            GameObject control = Instantiate(controlButtons[i], controlSchemeContainer.transform);
            control.GetComponent<Toggle>().group = controlSchemeContainer.GetComponent<ToggleGroup>();
            control.GetComponent<Toggle>().onValueChanged.AddListener((bool blah) => ControlChange(i));
            if (i == 0)
                control.GetComponent<Toggle>().isOn = true;
        }
    }

    private void ControlChange(int control)
    {
        selectedControl = control;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChanged;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChanged;
    }

    public void SetLeftHanded(bool left)
    {
        leftHanded = left;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(inGameScene);
    }

    public void EnterTestRoom()
    {
        SceneManager.LoadScene("");
    }

    public void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == titleScreenScene)
        {
            SetupControlButtons();
        }
        else if (scene.name == inGameScene)
        {
            AssignActiveMap();
            Transform main = GameObject.Find("Canvas").transform.Find(leftHanded ? "RightControl" : "LeftControl").Find("Scaler");
            Transform atk = GameObject.Find("Canvas").transform.Find(leftHanded ? "LeftControl" : "RightControl").Find("Scaler");
            Instantiate(controls[selectedControl], main);
            Instantiate(attackPanel, atk);

            LoadLevel(0);
        }
    }

    public void LoadLevel(int level)
    {
        currentLevel = level;
        activeTileMap.LoadMapFromFile(levels[level]);
        //activeTileMap.SpawnPlayer(GameObject.FindWithTag("Entrance").GetComponent<Tile>());
        activeTileMap.StartTurnQueue();
    }

    //given a ship, generate the level order & begin traversing the levels of the ship
    public void EnterShip(OverworldShip ship)
    {
        GenerateLevelOrder(ship);
        SceneManager.LoadScene(inGameScene);
        currentShip = ship;
    }

    public void GenerateLevelOrder(OverworldShip ship)
    {
        //set num of levels to randomly generate
        int numLevels = 0;
        int[] counts = new int[3];
        switch(ship.size)
        {
            case OverworldShip.ShipSize.Small:
                numLevels = smallShipRooms;
                counts = smallRoomCount;
                break;
            case OverworldShip.ShipSize.Medium:
                numLevels = mediumShipRooms;
                counts = mediumRoomCount;
                break;
            case OverworldShip.ShipSize.Large:
                numLevels = largeShipRooms;
                counts = largeRoomCount;
                break;
        }

        levels = new string[numLevels + 2]; //add 2 for mandatory engine/cockpit

        string identifier = ""; //used to select rooms in randomization
        identifier += ((int)ship.zone + 1); //convert zone to 1-based int (first zone = 0+1 = 1)
        identifier += (new string[] { "S", "M", "L" })[(int)ship.size]; //convert size to char (small = 0 = "S")
        identifier += "_";

        levels[0] = "Engine" + identifier + Random.Range(0, counts[0]);
        for(int i=0; i<numLevels; i++)
        {
            levels[i + 1] = "Room" + identifier + Random.Range(0, counts[1]);
        }
        levels[levels.Length - 1] = "Cockpit" + identifier + Random.Range(0, counts[2]);
    }

    public void NextLevel()
    {
        if(currentLevel+1 >= levels.Length)
        {
            currentShip.status = OverworldShip.ShipStatus.Complete;
            SceneManager.LoadScene("");
        }
        else
            LoadLevel(currentLevel + 1);
    }

    private void AssignActiveMap()
    {
        GameObject tm = GameObject.FindWithTag("TileMap");
        if (tm)
            activeTileMap = tm.GetComponent<TileMap>();
    }
}
