using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overlord : MonoBehaviour {

    public GameObject[] controlButtons;
    public GameObject[] controls;
    public GameObject attackButton;
    public bool leftHanded = false;
    public string titleScreenScene;
    public string testRoomScene;
    public string inGameScene;

    public int currentTask;
    public int selectedControl;
    public int controlProgress;

    int[] controlOrder;
    static bool started = false;


	void Awake() {
        if (started) Destroy(gameObject);
        started = true;
        DontDestroyOnLoad(gameObject);
        controlOrder = new int[controlButtons.Length];
        for(int i=0; i<controlButtons.Length; i++)
        {
            int j = Random.Range(0, i + 1);
            controlOrder[i] = controlOrder[j];
            controlOrder[j] = i;
        }
        controlProgress = 0;
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

    public void StartTasks()
    {
        SceneManager.LoadScene(inGameScene);
    }

    public void EnterTestRoom()
    {
        SceneManager.LoadScene(testRoomScene);
    }

    public void TasksCompleted()
    {
        SceneManager.LoadScene(titleScreenScene);
        if(controlProgress < controls.Length)
            controlProgress++;
    }
	
    public void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Prototype2Title")
        {
            SetupControlButtons();
        } else
        {
            Transform main = GameObject.Find("Canvas").transform.Find(leftHanded ? "RightControl" : "LeftControl").Find("Scaler");
            Transform atk = GameObject.Find("Canvas").transform.Find(leftHanded ? "LeftControl" : "RightControl").Find("Scaler");
            GameObject addedControl = Instantiate(controls[selectedControl], main);
            Instantiate(attackButton, atk);

            if(scene.name == inGameScene)
            {
                GameObject.Find("TileMap").GetComponent<TileMap>().ChangeTestCase(1);
            }
            else
            {
                GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(titleScreenScene));
            }
        }
    }
}
