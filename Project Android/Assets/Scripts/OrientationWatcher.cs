using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationWatcher : MonoBehaviour {

	//camera field of view for when screen is in potrait mode( phone screen long side is vertical) 
	//units in degrees
	public float portraitCameraFOV = 60;
	
	//camera field of view for when screen is in landscape mode( phone screen long side is horizontal) 
	//units in degrees
	//It should be approximately 20-30 degrees less than portrait FOV if need to make landscape and portrait screens look similar
	public float landscapeCameraFOV = 40;
	
	public enum Orientation
	{
		PORTRAIT = 0, LANDSCAPE
	};
	
	//variable to indicate to others what orientation screen is in
	public Orientation currentScreenOrientation;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if screen window width is more than screen window height, zoom in on camera 
		if(Screen.width > Screen.height)
		{
			Camera.main.fieldOfView = landscapeCameraFOV;
			currentScreenOrientation = Orientation.LANDSCAPE;
		}
		//else if screen window width is less than screen window height, 
		else if(Screen.width < Screen.height)
		{
			Camera.main.fieldOfView = portraitCameraFOV;
			currentScreenOrientation = Orientation.PORTRAIT;
		}
		
	}
}
