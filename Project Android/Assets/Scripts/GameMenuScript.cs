using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to call game menu from a button, quit game, and change settings.
	*/

	public Image gameMenuImage; //reference to menu image containing buttons 
	
	public Button gameMenuButton; //reference to menu button on screen
	
	public Button resumeGameButton; //reference to resume game button in menu
	
	// Use this for initialization
	void Start () 
	{
		//initially set menu image to false 
		gameMenuImage.gameObject.SetActive(false);
		
		//set callback function for button
		gameMenuButton.onClick.AddListener(GameMenuButtonClickProcess);
		resumeGameButton.onClick.AddListener(ResumeGameButtonClickProcess);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void GameMenuButtonClickProcess()
	{
		//set game menu image to opposite of its acive status i.e. if active set inactive, if inactive set active
		gameMenuImage.gameObject.SetActive(!gameMenuImage.gameObject.activeInHierarchy);
	}
	
	void ResumeGameButtonClickProcess()
	{
		//set game menu image to inactive
		gameMenuImage.gameObject.SetActive(false);
	}
	
}
