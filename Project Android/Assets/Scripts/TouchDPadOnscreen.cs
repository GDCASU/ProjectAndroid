using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Pablo Camacho
 * Date: 05/22/17
 * Description:
   Derived class of hold d pad on screen for use as on screen touch d pad. 
   It lets you know which directional button was pressed.
   Inherits from holdDPadOnscreeen because it needs to know if button is held in order to only enable one button press.
 */


public class TouchDPadOnscreen : HoldDPadOnscreen {

	//bools to indicate which buttons held
	private bool pressedUpButton;
	private bool pressedDownButton;
	private bool pressedLeftButton;
	private bool pressedRightButton;
	
	// Use this for initialization
	public void Start () 
	{
		base.Start();
		
		//initialize button names
		setUpButtonName("TouchUp");
		setDownButtonName("TouchDown");
		setLeftButtonName("TouchLeft");
		setRightButtonName("TouchRight");
	}
	
	// Update is called once per frame
	public void Update () 
	{
		base.Update();
		
		//if buttons are pressed and not being held, set their respective pressed button bools as true 
		//else set those as true
		if(getUpButtonActive() && !getHoldingUpButtonBool()){ setPressedUpButtonBool(true);}
		else{setPressedUpButtonBool(false);}
		
		if(getDownButtonActive() & !getHoldingDownButtonBool()){ setPressedDownButtonBool(true);}
		else{setPressedDownButtonBool(false);}
		
		if(getLeftButtonActive() && !getHoldingLeftButtonBool()){ setPressedLeftButtonBool(true);}
		else{setPressedLeftButtonBool(false);}
		
		if(getRightButtonActive() && !getHoldingRightButtonBool()){ setPressedRightButtonBool(true);}
		else{setPressedRightButtonBool(false);}
		
	}
	
	//functions to set pressed status of buttons
	protected void setPressedUpButtonBool(bool status){pressedUpButton = status;}
	protected void setPressedDownButtonBool(bool status){pressedDownButton = status;}
	protected void setPressedLeftButtonBool(bool status){pressedLeftButton = status;}
	protected void setPressedRightButtonBool(bool status){pressedRightButton = status;}
	
	//functions to get pressed status of buttons
	public bool getPressedUpButtonBool(){return pressedUpButton;}
	public bool getPressedDownButtonBool(){return pressedDownButton;}
	public bool getPressedLeftButtonBool(){return pressedLeftButton;}
	public bool getPressedRightButtonBool(){return pressedRightButton;}
}
