using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyOverworld : MonoBehaviour {
	

	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to manage the galaxy sections, keep player from moving in locked galaxy sections and where there are none.
		It moves the camera from one galaxy section to another.
		
	*/
	
	//references to text of direction arrows used for moving to other sections
	public Text upSectionText;
	public Text downSectionText;
	public Text rightSectionText;
	public Text leftSectionText;
	
	//references to buttons for moving section
	public NextSectionButton upSectionButton;
	public NextSectionButton downSectionButton;
	public NextSectionButton leftSectionButton;
	public NextSectionButton rightSectionButton;
	
	private float z = -100;
	public GalaxySection currentSection; //current section of galaxy player is in
	
	
	// Use this for initialization
	void Start () 
	{
		//set camera to camera position of current galaxy section
		Camera.main.gameObject.transform.position = new Vector3(currentSection.sectionCameraX,currentSection.sectionCameraY,z);
		
		//intialize text of direction arrows on canvas
		upSectionText.text = " ";
		downSectionText.text = " ";
		leftSectionText.text = " ";
		rightSectionText.text = " ";

        MoveToThisSection(currentSection);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//update text of direction arrows
		if(currentSection.upSection != null){upSectionText.text = currentSection.upSection.sectionName;}
		else{upSectionText.text = " ";}
		if(currentSection.downSection != null){downSectionText.text = currentSection.downSection.sectionName;}
		else{downSectionText.text = " ";}
		if(currentSection.leftSection != null){leftSectionText.text = currentSection.leftSection.sectionName;}
		else{leftSectionText.text = " ";}
		if(currentSection.rightSection != null){rightSectionText.text = currentSection.rightSection.sectionName;}
		else{rightSectionText.text = " ";}
	}
	
	public void MoveToThisSection(GalaxySection thisGalaxySection)
	{
		//change camera position to the section's camera position
		Camera.main.gameObject.transform.position = new Vector3(currentSection.sectionCameraX,
																	currentSection.sectionCameraY,
																	z);
		//zoom in on camera assuming it is orthographic
		Camera.main.orthographicSize = currentSection.cameraZoomOrthographic;
		//check for locks in adjacent section and set locks in buttons if needed
		CheckAndSetAdjacentSectionsForLocks();
		//set callback functions for newly created buttons
		SetCallbackFunctionForDirectionButtons();
	}
	
	public void MoveToLeftSection()
	{
		
		if(currentSection.leftSection != null)
		{
			//make left section the current section
			currentSection = currentSection.leftSection;
			//move to this section
			MoveToThisSection(currentSection);
		}
	}
	
	public void MoveToUpSection()
	{
		if(currentSection.upSection != null)
		{
			//make up section the current section
			currentSection = currentSection.upSection;
			
			//move to this section
			MoveToThisSection(currentSection);
		}
	}
	
	public void MoveToDownSection()
	{
		if(currentSection.downSection != null)
		{
			//make down section the current section
			currentSection = currentSection.downSection;
			
			//move to this section
			MoveToThisSection(currentSection);
		}
	}
	
	public void MoveToRightSection()
	{
		if(currentSection.rightSection != null)
		{
			
			//make right section the current section
			currentSection = currentSection.rightSection;
			
			//move to this section
			MoveToThisSection(currentSection);
		}
	}
	
	//function to check if adjacent sections are locked or unlocked and set appropriate buttons to locked or unlocked
	private void CheckAndSetAdjacentSectionsForLocks()
	{
		//if a section adjacent to current section is locked, set button to locked, 
		//if a section adjacent to curent section is unlocked, set button to unlocked
		//if there is no section adjacent to current section in a certain direction, set button to invisible
		if(currentSection.upSection != null)
		{
			if(currentSection.upSection.lockStatus){upSectionButton.SetToLocked();}
			else if(!currentSection.upSection.lockStatus){upSectionButton.SetToUnlocked();}
		}
		else{upSectionButton.SetToInvisible();}
		
		if(currentSection.downSection != null)
		{
			if(currentSection.downSection.lockStatus){downSectionButton.SetToLocked();}
			else if(!currentSection.downSection.lockStatus){downSectionButton.SetToUnlocked();}
		}
		else{downSectionButton.SetToInvisible();}
		
		if(currentSection.leftSection != null)
		{
			if(currentSection.leftSection.lockStatus){leftSectionButton.SetToLocked();}
			else if(!currentSection.leftSection.lockStatus){leftSectionButton.SetToUnlocked();}
		}
		else{leftSectionButton.SetToInvisible();}
		
		if(currentSection.rightSection != null)
		{
			if(currentSection.rightSection.lockStatus){rightSectionButton.SetToLocked();}
			else if(!currentSection.rightSection.lockStatus){rightSectionButton.SetToUnlocked();}
		}
		else{rightSectionButton.SetToInvisible();}
	}
	
	private void SetCallbackFunctionForDirectionButtons()
	{
		if(currentSection.upSection != null)
		{
			//if unlocked, add move to up section as the function called when up is clicked on
			if(!currentSection.upSection.lockStatus){upSectionButton.GetArrowButton().onClick.AddListener(MoveToUpSection);}		
		}
		
		if(currentSection.downSection != null)
		{
			//if unlocked, add move to down section as the function called when down is clicked on
			if(!currentSection.downSection.lockStatus){downSectionButton.GetArrowButton().onClick.AddListener(MoveToDownSection);}
		}
		else{downSectionButton.SetToInvisible();}
		
		if(currentSection.leftSection != null)
		{
			//if unlocked, add move to left section as the function called when left is clicked on
			if(!currentSection.leftSection.lockStatus){leftSectionButton.GetArrowButton().onClick.AddListener(MoveToLeftSection);}
		}
		else{leftSectionButton.SetToInvisible();}
		
		if(currentSection.rightSection != null)
		{
			//if unlocked, add move to right section as the function called when right is clicked on
			if(!currentSection.rightSection.lockStatus){rightSectionButton.GetArrowButton().onClick.AddListener(MoveToRightSection);}
		}
	}
}
