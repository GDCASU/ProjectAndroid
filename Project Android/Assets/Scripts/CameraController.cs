using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

/*
  Programmer: Pablo Camacho
  Date: 06/05/17
  Description:
	I had made this script to abstract rotating camera and translating camera for other scripts such as OrientationWatcher.
 */
	private float cameraSpeed = 10; //how fast camera moves
	
	//variables for angle to turn main camera around x axis
	private float xRotationCameraAngle = 50;
	private bool rotateCameraXAxisBool = false;// bool to tell main camera to turn or not
	
	//variables for angle to turn main camera around x axis
	private float yRotationCameraAngle = 0;
	private bool rotateCameraYAxisBool = false;// bool to tell main camera to turn or not

	//variables for angle to turn main camera around x axis
	private float zRotationCameraAngle = 0;
	private bool rotateCameraZAxisBool = false;// bool to tell main camera to turn or not

	//variables to translate camera
	private bool moveCameraUpXAxisBool;
	private bool moveCameraUpYAxisBool;
	private bool moveCameraUpZAxisBool;
	private bool moveCameraDownXAxisBool;
	private bool moveCameraDownYAxisBool;
	private bool moveCameraDownZAxisBool;	
	// Use this for initialization
	void Start () 
	{
		//Initialize bools to false
		
		
		xRotationCameraAngle = 50;
		rotateCameraXAxisBool = false;
		yRotationCameraAngle = 0;
		rotateCameraYAxisBool = false;		
		zRotationCameraAngle = 0;
		rotateCameraZAxisBool = false;
		
		moveCameraUpXAxisBool = false;
		moveCameraUpYAxisBool = false;
		moveCameraUpZAxisBool = false;
		moveCameraDownXAxisBool = false;
		moveCameraDownYAxisBool = false;
		moveCameraDownZAxisBool = false;	
	}
	
	
	
	// Update is called once per frame
	void Update () 
	{
		//if rotate camera around x axis bool is true, rotate camera and reset bool to false
		if(rotateCameraXAxisBool)
		{
			TiltCameraAroundXAxis(xRotationCameraAngle);
			rotateCameraXAxisBool = false;
		}
		
		//if rotate camera around y axis bool is true, rotate camera and reset bool to false
		if(rotateCameraYAxisBool)
		{
			TiltCameraAroundYAxis(yRotationCameraAngle);
			rotateCameraYAxisBool = false;
		}
		
		//if rotate camera up or down around z axis bool is true, rotate camera and reset bool to false
		if(rotateCameraZAxisBool)
		{
			TiltCameraAroundZAxis(zRotationCameraAngle);
			rotateCameraZAxisBool = false;
		}
		
		//if move camera up or down in x axis bool is true, move camera and reset bool
		if(moveCameraUpXAxisBool)
		{
			MoveCameraInXAxis(cameraSpeed);
			moveCameraUpXAxisBool = false;
		}
		else if(moveCameraDownXAxisBool)
		{
			MoveCameraInXAxis(-cameraSpeed);
			moveCameraDownXAxisBool = false;
		}
		
		//if move camera up or down in y axis bool is true, move camera and reset bool
		if(moveCameraUpYAxisBool)
		{
			MoveCameraInYAxis(cameraSpeed);
			moveCameraUpYAxisBool = false;
		}
		else if(moveCameraDownYAxisBool)
		{
			MoveCameraInYAxis(-cameraSpeed);
			moveCameraDownYAxisBool = false;
		}
		
		//if move camera up or down in z axis bool is true, move camera and reset bool
		if(moveCameraUpZAxisBool)
		{
			MoveCameraInZAxis(cameraSpeed);
			moveCameraUpZAxisBool = false;
		}
		else if(moveCameraDownZAxisBool)
		{
			MoveCameraInZAxis(-cameraSpeed);
			moveCameraDownZAxisBool = false;
		}
	}
	
	//public function to allow others to turn camera around x axis by certain angle
	public void RotateCameraAroundXAxis(float tiltAngle)
	{
		xRotationCameraAngle = tiltAngle;
		rotateCameraXAxisBool = true;
	}
	
	//public function to allow others to turn camera around y axis by certain angle
	public void RotateCameraAroundYAxis(float tiltAngle)
	{
		yRotationCameraAngle = tiltAngle;
		rotateCameraYAxisBool = true;
	}
	
	//public function to allow others to turn camera around z axis by certain angle
	public void RotateCameraAroundZAxis(float tiltAngle)
	{
		zRotationCameraAngle = tiltAngle;
		rotateCameraZAxisBool = true;
	}
	
	//function to only tilt main camera around x axis
	private void TiltCameraAroundXAxis(float tiltAngle)
	{
		Camera.main.transform.localEulerAngles = new Vector3(tiltAngle,0.0f,0.0f);
	}
	
	
	//function to only tilt main camera around y axis
	private void TiltCameraAroundYAxis(float tiltAngle)
	{
		Camera.main.transform.localEulerAngles = new Vector3(0.0f,tiltAngle,0.0f);
	}
	
	//function to only tilt main camera around z axis
	private void TiltCameraAroundZAxis(float tiltAngle)
	{
		Camera.main.transform.localEulerAngles = new Vector3(0.0f,0.0f,tiltAngle);
	}
	
	//public function to allow others to move camera in x axis
	public void MoveCameraUpXAxis(){moveCameraUpXAxisBool = true;}
	public void MoveCameraDownXAxis(){moveCameraDownXAxisBool = true;} 
	//public function to allow others to move camera in y axis
	public void MoveCameraUpYAxis(){moveCameraUpYAxisBool = true;}
	public void MoveCameraDownYAxis(){moveCameraDownYAxisBool = true;}
	//public function to allow others to move camera in z axis
	public void MoveCameraUpZAxis(){moveCameraUpZAxisBool = true;}
	public void MoveCameraDownZAxis(){moveCameraDownZAxisBool = true;}
	//private function to move camera in x axis
	private void MoveCameraInXAxis(float speed)
	{
		Camera.main.transform.Translate(new Vector3(speed,0,0) * Time.deltaTime);
	}
	//private function to move camera in y axis
	private void MoveCameraInYAxis(float speed)
	{
		Camera.main.transform.Translate(new Vector3(0,speed,0) * Time.deltaTime);
	}
	//private function to move camera in z axis
	private void MoveCameraInZAxis(float speed)
	{
		Camera.main.transform.Translate(new Vector3(0,0,speed) * Time.deltaTime);
	}
}
