using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  Programmer: Pablo Camacho
  Date: 06/05/17
  Description:
	I had made this earlier to keep track of whether phone is in landscape or portrait orientation and to change field of view
	for each orientation.
	I added several variables to change camera rotation in landscape or portrait.
 */

public class OrientationWatcher : MonoBehaviour {

	public enum Orientation
	{
		PORTRAIT = 0, LANDSCAPE
	};
	
	//variable to indicate to others what orientation screen is in
	public Orientation currentScreenOrientation;

	//camera field of view for when screen is in potrait mode( phone screen long side is vertical) 
	//units in degrees
	public float portraitCameraFOV;
	
	//camera field of view for when screen is in landscape mode( phone screen long side is horizontal) 
	//units in degrees
	//It should be approximately 20-30 degrees less than portrait FOV if need to make landscape and portrait screens look similar
	public float landscapeCameraFOV;
	
	//rotation of camera around x axis
	private float portraitCameraXTilt = 50;
	private float landscapeCameraXTilt = 50;
	private bool rotateCameraAroundXAxisBool;
	
	//rotation of camera around y axis
	private float portraitCameraYTilt = 0;
	private float landscapeCameraYTilt = 0;
	private bool rotateCameraAroundYAxisBool;
	
	//rotation of camera around z axis
	private float portraitCameraZTilt = 0;
	private float landscapeCameraZTilt = 0;
	private bool rotateCameraAroundZAxisBool;
	
	//reference to CameraController class to rotate and move camera
	public CameraController cameraController;
	
	// Use this for initialization
	void Start () 
	{
		if(Screen.width > Screen.height)
		{
			currentScreenOrientation = Orientation.LANDSCAPE;
			//initialize camera tilt x,y,z angles
			landscapeCameraXTilt = Camera.main.transform.eulerAngles.x;
			landscapeCameraYTilt = Camera.main.transform.eulerAngles.y;
			landscapeCameraZTilt = Camera.main.transform.eulerAngles.z;
		}
		else if(Screen.width < Screen.height)
		{
			currentScreenOrientation = Orientation.PORTRAIT;
			//initialize camera tilt x,y,z angles
			portraitCameraXTilt = Camera.main.transform.eulerAngles.x;
			portraitCameraYTilt = Camera.main.transform.eulerAngles.y;
			portraitCameraZTilt = Camera.main.transform.eulerAngles.z;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if screen window width is more than screen window height, zoom in on camera 
		if(Screen.width > Screen.height)
		{	
			currentScreenOrientation = Orientation.LANDSCAPE;
			
			//assign landscape camera values to main camera
			Camera.main.fieldOfView = landscapeCameraFOV;
			// if rotate camera around x axis bool is true, rotate camera around x axis and reset bool to false
			if(rotateCameraAroundXAxisBool)
			{
				cameraController.RotateCameraAroundXAxis(landscapeCameraXTilt);
				rotateCameraAroundXAxisBool = false;
			}
			// if rotate camera around y axis bool is true, rotate camera around y axis and reset bool to false
			if(rotateCameraAroundYAxisBool)
			{
				cameraController.RotateCameraAroundYAxis(landscapeCameraYTilt);
				rotateCameraAroundYAxisBool = false;
			}
			// if rotate camera around z axis bool is true, rotate camera around z axis and reset bool to false
			if(rotateCameraAroundZAxisBool)
			{
				cameraController.RotateCameraAroundZAxis(landscapeCameraZTilt);
				rotateCameraAroundZAxisBool = false;
			}
			
		}
		//else if screen window width is less than screen window height, 
		else if(Screen.width < Screen.height)
		{
			currentScreenOrientation = Orientation.PORTRAIT;
			//assign landscape camera values to main camera
			Camera.main.fieldOfView = portraitCameraFOV;
			
			// if rotate camera around x axis bool is true, rotate camera around x axis and reset bool to false
			if(rotateCameraAroundXAxisBool)
			{
				cameraController.RotateCameraAroundXAxis(portraitCameraXTilt);
				rotateCameraAroundXAxisBool = false;
			}
			
			// if rotate camera around y axis bool is true, rotate camera around y axis and reset bool to false
			if(rotateCameraAroundYAxisBool)
			{
				cameraController.RotateCameraAroundYAxis(portraitCameraYTilt);
				rotateCameraAroundYAxisBool = false;
			}
			
			// if rotate camera around z axis bool is true, rotate camera around z axis and reset bool to false
			if(rotateCameraAroundZAxisBool)
			{
				cameraController.RotateCameraAroundZAxis(portraitCameraZTilt);
				rotateCameraAroundZAxisBool = false;
			}
		}
		
	}
	
	public float GetPortraitCameraXTilt(){return portraitCameraXTilt;}
	public float GetLandscapeCameraXTilt(){return landscapeCameraXTilt;}
	
	//function to rotate camera around x axis in portrait
	public void RotateCameraAroundXAxisInPortrait(float tiltAngle)
	{
		//set portrait camera x tilt
		portraitCameraXTilt = tiltAngle;
		rotateCameraAroundXAxisBool = true;
	}
	
	//function to rotate camera around x axis in landscape
	public void RotateCameraAroundXAxisInLandscape(float tiltAngle)
	{
		//set portrait camera x tilt
		landscapeCameraXTilt = tiltAngle;
		rotateCameraAroundXAxisBool = true;
	}
	
	public float GetPortraitCameraYTilt(){return portraitCameraYTilt;}
	public float GetLandscapeCameraYTilt(){return landscapeCameraYTilt;}
	
	//function to rotate camera around x axis in portrait
	public void RotateCameraAroundYAxisInPortrait(float tiltAngle)
	{
		//set portrait camera x tilt
		portraitCameraYTilt = tiltAngle;
		rotateCameraAroundYAxisBool = true;
	}
	
	//function to rotate camera around x axis in landscape
	public void RotateCameraAroundYAxisInLandscape(float tiltAngle)
	{
		//set portrait camera x tilt
		landscapeCameraYTilt = tiltAngle;
		rotateCameraAroundYAxisBool = true;
	}
	
	public float GetPortraitCameraZTilt(){return portraitCameraZTilt;}
	public float GetLandscapeCameraZTilt(){return landscapeCameraZTilt;}
	
	//function to rotate camera around x axis in portrait
	public void RotateCameraAroundZAxisInPortrait(float tiltAngle)
	{
		//set portrait camera x tilt
		portraitCameraZTilt = tiltAngle;
		rotateCameraAroundZAxisBool = true;
	}
	
	//function to rotate camera around x axis in landscape
	public void RotateCameraAroundZAxisInLandscape(float tiltAngle)
	{
		//set portrait camera x tilt
		landscapeCameraZTilt = tiltAngle;
		rotateCameraAroundZAxisBool = true;
	}
}
