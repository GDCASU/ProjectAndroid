using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextSectionButton : MonoBehaviour {

	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to encapsulate button and enum status of locked,unlocked,unable to move. Used for arrow buttons to move galaxy sections.
	*/
	
	public enum Status
	{
		LOCKED = 0, UNLOCKED, UNABLE_TO_MOVE
	};
	
	
	public Status nextSectionStatus; //status of if locked, unlocked, or not able to move there because there is nothing there
	public GameObject allowedNextSectionButtonPrefab; //reference to allowed button prefab
	public GameObject prohibitedNextSectionPrefab; //reference to prohibited button prefab
	
	private GameObject nextSectionArrowButton; 
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		

	}
	
	//function to return reference to button instantiated
	public Button GetArrowButton(){return nextSectionArrowButton.GetComponent <Button>();}
	
	//function to set button to locked status and change the model to prohibited model
	public void SetToLocked()
	{
		if(nextSectionArrowButton != null){Destroy(nextSectionArrowButton);}
		nextSectionArrowButton = Instantiate(prohibitedNextSectionPrefab, 
									new Vector3(transform.position.x, transform.position.y, transform.position.z), 
									prohibitedNextSectionPrefab.transform.rotation);
		nextSectionArrowButton.transform.SetParent (transform.Find("Scaler").parent);
		nextSectionArrowButton.SetActive(true);
	}
	
	//function to set button to locked status and change the model to prohibited model
	public void SetToUnlocked()
	{
		if(nextSectionArrowButton != null){Destroy(nextSectionArrowButton);}
		nextSectionArrowButton = Instantiate(allowedNextSectionButtonPrefab, 
									new Vector3(transform.position.x, transform.position.y, transform.position.z), 
									allowedNextSectionButtonPrefab.transform.rotation);
		nextSectionArrowButton.transform.SetParent (transform.Find("Scaler").parent);
		nextSectionArrowButton.SetActive(true);
	}
	
	public void SetToInvisible()
	{
		if(nextSectionArrowButton != null){Destroy(nextSectionArrowButton);}
	}
}
