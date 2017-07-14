using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to control level menu.
*/

public class LevelMenu : MonoBehaviour
{
    public GameObject levelMenuGameObject;
    public Text levelTitleShipName;
    public Overlord overlord;

    public Button returnToMapButton;

    public Button startLevelButton;

    private OverworldShip overworldShipRef; //reference to overworld ship that called menu

    // Use this for initialization
    void Start()
    {
        //set callback functions for buttons when clicked
        returnToMapButton.onClick.AddListener(ReturnToMap);
        startLevelButton.onClick.AddListener(StartLevel);
    }

    void ReturnToMap()
    {
        levelMenuGameObject.SetActive(false);
        overworldShipRef = null;
    }

    public void SetInfo(string name, OverworldShip ship, Overlord overlord)
    {
        levelTitleShipName.text = name;
        overworldShipRef = ship;
        this.overlord = overlord;
    }

    void StartLevel()
    {
        overlord.EnterShip(overworldShipRef);
    }
}
