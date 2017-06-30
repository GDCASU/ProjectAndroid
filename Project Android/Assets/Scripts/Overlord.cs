using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: This script currently manages the player's progression through different rooms.
//              It also contains methods responsible for adding in the controls selected by the player
//              and some extraneous methods having to do with the camera.

public class Overlord : MonoBehaviour
{

    public GameObject[] controlButtons;
    public GameObject[] controls;
    public string[] levels;
    public int currentLevel;
    public GameObject attackPanel;
    public bool leftHanded = false;
    public string titleScreenScene;
    public string testRoomScene;
    public string inGameScene;

    public int selectedControl;
    public int controlProgress;

    public TileMap activeTileMap;

    int[] controlOrder;
    static bool started = false;
    bool turnBased;


    void Awake()
    {
        if (started) Destroy(gameObject);
        started = true;
        DontDestroyOnLoad(gameObject);
        controlOrder = new int[controlButtons.Length];
        for (int i = 0; i < controlButtons.Length; i++)
        {
            int j = Random.Range(0, i + 1);
            controlOrder[i] = controlOrder[j];
            controlOrder[j] = i;
        }
        controlProgress = 0;

        AssignActiveMap();
    }

    public void SetupControlButtons()
    {
        GameObject controlSchemeContainer = GameObject.Find("ControlSchemes");
        for (int i = 0; i < controlOrder.Length; i++)
        {
            int index = controlOrder[i];
            GameObject control = Instantiate(controlButtons[index], controlSchemeContainer.transform);
            control.GetComponent<Toggle>().group = controlSchemeContainer.GetComponent<ToggleGroup>();
            control.GetComponent<Toggle>().onValueChanged.AddListener((bool blah) => ControlChange(index));
            if (i == controlProgress)
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

    public void StartGame(bool tb)
    {
        turnBased = tb;
        SceneManager.LoadScene(inGameScene);
    }

    public void EnterTestRoom()
    {
        SceneManager.LoadScene(testRoomScene);
    }

    public void TasksCompleted()
    {
        SceneManager.LoadScene(titleScreenScene);
        if (controlProgress < controls.Length)
            controlProgress++;
    }

    public void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == titleScreenScene)
        {
            SetupControlButtons();
        }
        else
        {
            AssignActiveMap();
            Transform main = GameObject.Find("Canvas").transform.Find(leftHanded ? "RightControl" : "LeftControl").Find("Scaler");
            Transform atk = GameObject.Find("Canvas").transform.Find(leftHanded ? "LeftControl" : "RightControl").Find("Scaler");
            Instantiate(controls[selectedControl], main);
            Instantiate(attackPanel, atk);

            if (scene.name == inGameScene)
            {
                activeTileMap.turnBased = turnBased;
                currentLevel = -1;
                NextLevel();
            }
            else
            {
                GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(titleScreenScene));
            }

            AdjustFixedCamera();
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        activeTileMap.LoadMapFromFile(levels[currentLevel]);
        activeTileMap.SpawnPlayer(GameObject.FindWithTag("Entrance").GetComponent<Tile>());
        activeTileMap.StartTurnQueue();
    }

    public void AdjustFixedCamera()
    {
        if (!activeTileMap) return;
        Vector3 size = new Vector3(activeTileMap.mapWidth / 2f, 0, activeTileMap.mapHeight / 2f);
        Vector3 tr = Camera.main.WorldToViewportPoint(size);
        Vector3 tl = Camera.main.WorldToViewportPoint(new Vector3(-size.x, 0, size.z));
        Vector3 br = Camera.main.WorldToViewportPoint(new Vector3(size.x, 0, -size.z));
        Vector3 bl = Camera.main.WorldToViewportPoint(new Vector3(-size.x, 0, -size.z));

        float scale = ((size * 2).magnitude / 3f / (Camera.main.aspect)) / Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //needs tweaking
        
        //Debug.Log(tr);
        //Debug.Log(tl);
        //Debug.Log(br);
        //Debug.Log(bl);

        Vector3 newPos = Camera.main.transform.position;

        if (br.x - bl.x != 0.6f)
            newPos += Camera.main.transform.forward * (scale * (0.6f - (br.x - bl.x)));
        if (bl.y != 0.05f)
            newPos -= Camera.main.transform.up * (scale * (0.05f - bl.y));

        Camera.main.transform.position = newPos;
    }



    private void AssignActiveMap()
    {
        GameObject tm = GameObject.FindWithTag("TileMap");
        if (tm)
            activeTileMap = tm.GetComponent<TileMap>();
    }
}
