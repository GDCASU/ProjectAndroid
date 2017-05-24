using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

/*
 * Programmer: Pablo Camacho
 * Date: 05/22/17
 * Description:
   Base class for on screen d pad. It lets you know which directional button was pressed.
 */

public class BaseDPadOnScreen : MonoBehaviour {

	
	//bools to indicate status of buttons 
	private bool upButtonActive;
	private bool downButtonActive;
	private bool leftButtonActive;
	private bool rightButtonActive;
	
	//strings containing unique name for buttons to prevent different buttons with same name from being called
	//also to set button names in derived classes.
	private string upButtonName;
	private string downButtonName;
	private string leftButtonName;
	private string rightButtonName;
	
	// Use this for initialization
	public void Start () 
	{
		//initialize button status to not active
		upButtonActive = false;
		downButtonActive = false;
		leftButtonActive = false;
		rightButtonActive = false;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		upButtonActive = CrossPlatformInputManager.GetButton(upButtonName);
		//if(upButtonActive){Debug.Log("Pressing Up! \n");}
		//else{Debug.Log("Not Pressing Up! \n");}
		
		downButtonActive = CrossPlatformInputManager.GetButton(downButtonName);
		leftButtonActive = CrossPlatformInputManager.GetButton(leftButtonName);
		rightButtonActive = CrossPlatformInputManager.GetButton(rightButtonName);
	}
	
	//Functions to return status of buttons
	public bool getUpButtonActive(){return upButtonActive; }
	public bool getDownButtonActive(){return downButtonActive;}
	public bool getLeftButtonActive(){return leftButtonActive;}
	public bool getRightButtonActive(){return rightButtonActive;}
	
	//Functions to set button names so button can be referenced and checked if pressed.
	protected void setUpButtonName(string name){upButtonName = name;}
	protected void setDownButtonName(string name){downButtonName = name;}
	protected void setLeftButtonName(string name){leftButtonName = name;}
	protected void setRightButtonName(string name){rightButtonName = name;}
}
