using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySection : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to keep info on galaxy sections such as camera position to move camera in order to only see this galaxy section,
		references to adjacent gaaxy sections, name of galaxy section, and whether section is locked or unlocked.
	*/

	//camera position to see only this galaxy section
	public float sectionCameraX, sectionCameraY;
	
	//camera size assuming using orthographic camera
	public float cameraZoomOrthographic;
	
	//references to sections adjacent to this section
	public GalaxySection upSection;
	public GalaxySection leftSection;
	public GalaxySection rightSection;
	public GalaxySection downSection;
	
	public string sectionName;
	
	public bool lockStatus;
	
	public GameObject overworldShipContainer;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If locked, set overworld ship container game object to inactive
		//otherwise set to active 
		if(lockStatus){overworldShipContainer.SetActive(false);}
		else if(!lockStatus){overworldShipContainer.SetActive(true);}
	}
}
