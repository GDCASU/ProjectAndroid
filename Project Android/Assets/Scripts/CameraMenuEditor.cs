using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
  Programmer: Pablo Camacho
  Date: 06/05/17
  Description:
	I made this script to change camera settings graphically and more easily.
 */

public class CameraMenuEditor : MonoBehaviour {
	
	//bool to let you know if doing debug stuff with camera
	public bool debug;
	
	//reference to camera editing button
	public Button cameraEditingButton;

//Camera Menu editing settings

	//bool to determine if camera menu is active or not
	private bool cameraMenuActive;

	//reference to image for camera menu
	public Image cameraMenuImage; 

	
	//reference to orientation watcher class script
	public OrientationWatcher orientationController;
	
	//reference to slider that will be used to control field of view 
	public Slider fovSlider;
	public Text fovSliderValueText;
	
	//reference to slider that will be used to control x tilt camera angle
	public Slider xTiltSlider;
	public Text xTiltSliderValueText;
	
	//reference to slider that will be used to control y tilt camera angle
	public Slider yTiltSlider;
	public Text yTiltSliderValueText;
	
	//reference to slider that will be used to control z tilt camera angle
	public Slider zTiltSlider;
	public Text zTiltSliderValueText;
	
	//references to translation direction buttons for moving camera
	public Button moveCameraUpButton;
	public Button moveCameraDownButton;
	
	//references to translation axis toggles
	public Toggle xAxisToggle;
	public Toggle yAxisToggle;
	public Toggle zAxisToggle;
	
	//reference to player camera 
	public PlayerCamera playerCamera;
	
	//reference to buttons to choose static or folow camera modes
	public Button staticCameraButton;
	public Button followCameraButton;
	
	//references to position text UI for display position info
	public Text xPositionText;
	public Text yPositionText;
	public Text zPositionText; 
	
	// Use this for initialization
	void Start () 
	{
		//make camera editing button invisible if debug is false, otherwise if true
		if(cameraEditingButton != null){cameraEditingButton.gameObject.SetActive(debug);}
		
		//make camera menu image false
		if(cameraMenuImage != null){cameraMenuImage.gameObject.SetActive(false);}
		
		//set initial values for sliders 
		//Important note: must be set befor callbacks for sliders or else initialize camera values with uninitialized values
		
		fovSliderValueText.text = "0";
		fovSlider.value = 0;
		
		
		xTiltSliderValueText.text = "0";
		yTiltSliderValueText.text = "0";
		zTiltSliderValueText.text = "0";
		
		if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
		{
			xTiltSlider.value = orientationController.GetPortraitCameraXTilt();
			yTiltSlider.value = orientationController.GetPortraitCameraYTilt();
			zTiltSlider.value = orientationController.GetPortraitCameraZTilt();
		}
		else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
		{
			xTiltSlider.value = orientationController.GetLandscapeCameraXTilt();
			yTiltSlider.value = orientationController.GetLandscapeCameraYTilt();
			zTiltSlider.value = orientationController.GetLandscapeCameraZTilt();
		}
		
		//set callback functions for buttons and sliders
		
		cameraEditingButton.onClick.AddListener(CameraEditorButtonClickProcess);
		
		fovSlider.onValueChanged.AddListener(delegate {FOVChangedProcess ();});
		
		xTiltSlider.onValueChanged.AddListener(delegate {xTiltChangedProcess ();});
		yTiltSlider.onValueChanged.AddListener(delegate {yTiltChangedProcess ();});
		zTiltSlider.onValueChanged.AddListener(delegate {zTiltChangedProcess ();});
		
		staticCameraButton.onClick.AddListener(StaticCameraButtonClickProcess);
		followCameraButton.onClick.AddListener(FollowCameraButtonClickProcess);
		
		moveCameraUpButton.onClick.AddListener(MoveCameraUpButtonClickProcess);
		moveCameraDownButton.onClick.AddListener(MoveCameraDownButtonClickProcess);
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if debug bool is on 
		if(debug)
		{
			//check if in portrait or landscape
			if(orientationController != null && fovSliderValueText != null)
			{
				//if in portrait, change values to portrait values
				if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
				{
					fovSlider.value = orientationController.portraitCameraFOV;
					fovSliderValueText.text = orientationController.portraitCameraFOV.ToString("0");
					
					xTiltSlider.value = orientationController.GetPortraitCameraXTilt();
					xTiltSliderValueText.text = orientationController.GetPortraitCameraXTilt().ToString("0");
					
					yTiltSlider.value = orientationController.GetPortraitCameraYTilt();
					yTiltSliderValueText.text = orientationController.GetPortraitCameraYTilt().ToString("0.0");
					
					zTiltSlider.value = orientationController.GetPortraitCameraZTilt();
					zTiltSliderValueText.text = orientationController.GetPortraitCameraZTilt().ToString("0.0");
					 
				}
				//else if in landscape, change values to landscape values
				else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
				{
					fovSlider.value = orientationController.landscapeCameraFOV;
					fovSliderValueText.text = orientationController.landscapeCameraFOV.ToString("0");
					
					xTiltSlider.value = orientationController.GetLandscapeCameraXTilt();
					xTiltSliderValueText.text = orientationController.GetLandscapeCameraXTilt().ToString("0");
					
					yTiltSlider.value = orientationController.GetLandscapeCameraYTilt();
					yTiltSliderValueText.text = orientationController.GetLandscapeCameraYTilt().ToString("0.0");
					
					zTiltSlider.value = orientationController.GetLandscapeCameraZTilt();
					zTiltSliderValueText.text = orientationController.GetLandscapeCameraZTilt().ToString("0.0");
				}
				
				//update main camera position text
				xPositionText.text = Camera.main.transform.position.x.ToString("0.00");
				yPositionText.text = Camera.main.transform.position.y.ToString("0.00");
				zPositionText.text = Camera.main.transform.position.z.ToString("0.00");
			}
		}
	}
	
	void CameraEditorButtonClickProcess()
	{
		//set camera menu image to opposite of its current state i.e if inactive set to active, if active set to inactive
		if(cameraMenuImage != null){cameraMenuImage.gameObject.SetActive(!cameraMenuImage.gameObject.activeInHierarchy);}
	}
	
	void FOVChangedProcess()
	{
		if(orientationController != null && fovSlider != null )
		{
			//if in portrait, change portrait FOV
			if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
			{
				orientationController.portraitCameraFOV = fovSlider.value;
			}
			//else if in landscape, change landscape FOV
			else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
			{
				orientationController.landscapeCameraFOV = fovSlider.value;
			}
		}
	}
	
	void xTiltChangedProcess()
	{
		if(orientationController != null && xTiltSlider != null )
		{
			//if in portrait, change portrait FOV
			if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
			{
				orientationController.RotateCameraAroundXAxisInPortrait(xTiltSlider.value);
			}
			//else if in landscape, change landscape FOV
			else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
			{
				orientationController.RotateCameraAroundXAxisInLandscape(xTiltSlider.value);
			}
		}
	}
	
	void yTiltChangedProcess()
	{
		if(orientationController != null && yTiltSlider != null )
		{
			//if in portrait, change portrait FOV
			if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
			{
				orientationController.RotateCameraAroundYAxisInPortrait(yTiltSlider.value);
			}
			//else if in landscape, change landscape FOV
			else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
			{
				orientationController.RotateCameraAroundYAxisInLandscape(yTiltSlider.value);
			}
		}
	}
	
	void zTiltChangedProcess()
	{
		if(orientationController != null && zTiltSlider != null )
		{
			//if in portrait, change portrait FOV
			if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.PORTRAIT)
			{
				orientationController.RotateCameraAroundZAxisInPortrait(zTiltSlider.value);
			}
			//else if in landscape, change landscape FOV
			else if(orientationController.currentScreenOrientation == OrientationWatcher.Orientation.LANDSCAPE)
			{
				orientationController.RotateCameraAroundZAxisInLandscape(zTiltSlider.value);
			}
		}
	}

	void StaticCameraButtonClickProcess()
	{
		playerCamera.SetPlayerCameraMode(PlayerCamera.PlayerCameraMode.STATIC) ;//set player camera to static camera mode
	}
	
	void FollowCameraButtonClickProcess()
	{
		playerCamera.SetPlayerCameraMode(PlayerCamera.PlayerCameraMode.FOLLOW); //set player camera to follow camera mode
	}
	
	void MoveCameraUpButtonClickProcess()
	{
		if(xAxisToggle.isOn){orientationController.cameraController.MoveCameraUpXAxis();}
		if(yAxisToggle.isOn){orientationController.cameraController.MoveCameraUpYAxis();}
		if(zAxisToggle.isOn){orientationController.cameraController.MoveCameraUpZAxis();}
	}
	
	void MoveCameraDownButtonClickProcess()
	{
		if(xAxisToggle.isOn){orientationController.cameraController.MoveCameraDownXAxis();}
		if(yAxisToggle.isOn){orientationController.cameraController.MoveCameraDownYAxis();}
		if(zAxisToggle.isOn){orientationController.cameraController.MoveCameraDownZAxis();}
	}
}

