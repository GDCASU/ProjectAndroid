using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySectionUpdate : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 06/16/17
		Description:
		This is a new script based on GalaxySection that was designed for final Overworld UI design. 
		Removed directional sections and replaced with one adjacent section.
		I made this script to keep info on galaxy sections such as camera position to move camera in order to only see this galaxy section,
		references to an adjacent galaxy sections, name of galaxy section, and whether section is locked or unlocked.
	*/

	 
	//the world position of section border assuming section is a circle and centered at origin
	private float sectionBorderX,sectionBorderY;
	
	
	public string sectionName; //name of Section
	
	public bool initialLockStatus; //whether level is locked or not in the beginning of the game
	
	private bool lockStatus; //whether section is locked or not
	
	public GameObject overworldShipContainer; //reference to container that holds overworld ships in section
	
	public GameObject sectionBorder; //reference to game object section border in section
	
	public Material lockedSectionMaterial; //reference to material used for when section is locked
	public Material unlockedSectionMaterial; //reference to material used for when section is unlocked
	
	//reference to adjacent section
	public GalaxySectionUpdate nextSection;

	
	// Use this for initialization
	void Start () 
	{
		calculateSectionBorder();
		if(initialLockStatus){SetToLocked();}
		else if(!initialLockStatus){SetToUnlocked();}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	private void calculateSectionBorder()
	{
		//calculate section border x and section border y as half of circle section length
		sectionBorderX = sectionBorder.transform.lossyScale.x * 0.5f;	
		sectionBorderY = sectionBorder.transform.lossyScale.y * 0.5f;
	}
	
	//function to return section border
	public float GetSectionBorderX(){return sectionBorderX;}
	public float GetSectionBorderY(){return sectionBorderY;}
	
	//public function to set section to locked
	public void SetToLocked()
	{
		lockStatus = true;
		overworldShipContainer.SetActive(false);
		
		Renderer rend = sectionBorder.GetComponent<Renderer>();
		rend.material = lockedSectionMaterial;
	}
	
	//public function to set section to unlocked
	public void SetToUnlocked()
	{
		lockStatus = false;
		overworldShipContainer.SetActive(true);
		
		Renderer rend = sectionBorder.GetComponent<Renderer>();
		rend.material =  unlockedSectionMaterial;
	}

	//public function to return locked status of section
	public bool GetLockedStatus(){return lockStatus;}
	
}
