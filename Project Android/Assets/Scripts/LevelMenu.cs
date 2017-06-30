using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to control level menu.
	*/
	
	public GameObject levelMenuGameObject;
	public Text levelTitleShipName;

	public Button returnToMapButton;
	
	public Button startLevelButton;
	
	private OverworldShip overworldShipRef; //reference to overworld ship that called menu
	
	// Use this for initialization
	void Start () 
	{
		levelMenuGameObject.SetActive(false); //set level menu game object to start inactive
		
		//set callback functions for buttons when clicked
		returnToMapButton.onClick.AddListener(ReturnToMapButtonClickProcess);
		startLevelButton.onClick.AddListener(StartLevelButtonClickProcess);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void ReturnToMapButtonClickProcess()
	{
		levelMenuGameObject.SetActive(false);
		overworldShipRef = null;
	}
	
	public void SetOverworldShipReference(OverworldShip thisShip){ overworldShipRef = thisShip;}
	
	void StartLevelButtonClickProcess()
	{
		SceneManager.LoadScene(overworldShipRef.sceneLevelName);
	}
}
