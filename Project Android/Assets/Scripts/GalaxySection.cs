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
	
	//references to sections adjacent to this section
	public GalaxySection upSection;
	public GalaxySection leftSection;
	public GalaxySection rightSection;
	public GalaxySection downSection;
	
	public string sectionName;
	
	public bool lockStatus;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
