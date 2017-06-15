using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to call inventory menu from a button and check inventory.
	*/

	public Image inventoryMenuImage; //reference to menu image containing buttons 
	
	public Button inventoryMenuButton; //reference to menu button on screen
	
	public Button resumeGameButton; //reference to resume game button in menu
	
	// Use this for initialization
	void Start () 
	{
		//initially set menu image to false 
		inventoryMenuImage.gameObject.SetActive(false);
		
		//set callback function for button
		inventoryMenuButton.onClick.AddListener(InventoryMenuButtonClickProcess);
		resumeGameButton.onClick.AddListener(ResumeGameButtonClickProcess);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void InventoryMenuButtonClickProcess()
	{
		//set game menu image to opposite of its acive status i.e. if active set inactive, if inactive set active
		inventoryMenuImage.gameObject.SetActive(!inventoryMenuImage.gameObject.activeInHierarchy);
	}
	
	void ResumeGameButtonClickProcess()
	{
		//set game menu image to inactive
		inventoryMenuImage.gameObject.SetActive(false);
	}
}
