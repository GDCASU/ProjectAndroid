using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyOverworldUpdate : MonoBehaviour {
	

	/*
		Programmer: Pablo Camacho
		Date: 06/16/17
		Description:
		This is a new script based on GalaxyOverworld. It is updated for the final overworld UI design. 
		I also modified it for click and drag. The arrow buttons only show that section is locked.
		I made this script to manage the galaxy sections, keep player from moving in locked galaxy sections and where there are none.
		It moves the camera from one galaxy section to another.
		
		
	*/
	

	public float originX;
	public float originY;
	
	[Header("Free Camera Movement Parameters")]
	public GalaxySectionUpdate startSection; //section of galaxy player starts in
	public GalaxySectionUpdate lastSection; //last section of galaxy player is able to move in 
											//Important Note: Make sure it is a large flat cube because 
											//camera ends up showing black space at diagonal sides of sphere
	public float globalOrthographicZoom; //free camera movement mode uses 1 orthographic camera size instead of individual sizes for each galaxy section
	
	private float z = -100;

	  
	
	// Use this for initialization
	void Start () 
	{	
		MoveToThisSectionToOrigin(startSection);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	//public function to move through sections with click and drag
	public void MoveThroughSectionWithClickAndDrag(Vector3 move)
	{
		DetermineMoveCameraAtLastSectionBorder(move); //keep camera from moving across last section border	
	}
	
	//function to move to section's origin
	private void MoveToThisSectionToOrigin(GalaxySectionUpdate thisGalaxySection)
	{
		//change camera position to the section's camera position
		Camera.main.gameObject.transform.position = new Vector3(originX,originY,z);
													
		//zoom in on camera assuming it is orthographic
		Camera.main.orthographicSize = globalOrthographicZoom;	
	}
	
	
	//function to keep camera from moving in last section 
	//Use for free camera movement mode
	private void DetermineMoveCameraAtLastSectionBorder(Vector3 move)
	{
		
		//variables used for calculating if current section border was crossed
		//based on moving camera and seeing if section border is visually seen at a certain position in world transform coordinates
		//negative value means it would be crossed
		float topYBorderCrossed, bottomYBorderCrossed, leftXBorderCrossed, rightXBorderCrossed;
		 
		float borderX = lastSection.GetSectionBorderX();
		float borderY = lastSection.GetSectionBorderY();
		
		//factor to determine how much effect camera size has on border
		//generally keep this between 0.98f and 1.1f to keep camera from passing last section border by camera width
		//the smaller the value the more camera moves across border
		float orthographicZoomScaler = 1.5f; 
		
		//find the difference between the border position and the camera position plus movement
		//if negative, then it means border would be crossed 
		topYBorderCrossed = (borderY - (globalOrthographicZoom * orthographicZoomScaler)) -  ( Camera.main.transform.position.y + move.y) ;
		bottomYBorderCrossed = - ( (-borderY + (globalOrthographicZoom * orthographicZoomScaler)) - ( ( Camera.main.transform.position.y + move.y) ));
		
		leftXBorderCrossed = - ( (-borderX + (globalOrthographicZoom * orthographicZoomScaler)) - ( ( Camera.main.transform.position.x + move.x) ));
		rightXBorderCrossed = (borderX - (globalOrthographicZoom * orthographicZoomScaler)) -  ( Camera.main.transform.position.x + move.x);
		
		
		//if last section's top border would be crossed 
		if(topYBorderCrossed < 0)
		{
			//move main camera transform position by assigning it a new position at border
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
														 (borderY - (globalOrthographicZoom * orthographicZoomScaler)),
													 Camera.main.transform.position.z);
		}
		//if last section's bottom border would be crossed 
		else if(bottomYBorderCrossed < 0)
		{
			//move main camera transform position by assigning it a new position at border
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
														 -(borderY - (globalOrthographicZoom * orthographicZoomScaler)),
													 Camera.main.transform.position.z);
		}
		//if last section's left border would be crossed 
		else if(leftXBorderCrossed < 0)
		{
			//move main camera transform position by assigning it a new position at border
			Camera.main.transform.position = new Vector3(-(borderX - (globalOrthographicZoom * orthographicZoomScaler)),
														 Camera.main.transform.position.y,
													 Camera.main.transform.position.z);
		}
		//if last section's right border would be crossed 
		else if(rightXBorderCrossed < 0)
		{
			//move main camera transform position by assigning it a new position at border
			Camera.main.transform.position = new Vector3((borderX - (globalOrthographicZoom * orthographicZoomScaler)),
														 Camera.main.transform.position.y,
													 Camera.main.transform.position.z);
		}
		//if current section's border not crossed set bool to move main camera to true
		else
		{
			//move main camera transform position by assigning it a new position by adding main camera transform position and move vector
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + move.x,
														 Camera.main.transform.position.y + move.y,
														 Camera.main.transform.position.z);
		}
		
		
	}
	
	//function to check if adjacent sections are locked or unlocked and set appropriate buttons to locked or unlocked
	private void CheckAndSetAdjacentSectionsForLocks()
	{
		
	}
}
