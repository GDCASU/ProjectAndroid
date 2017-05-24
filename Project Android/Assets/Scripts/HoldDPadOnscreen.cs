using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Pablo Camacho
 * Date: 05/22/17
 * Description:
   Derived class of base d pad on screen for use as on screen hold d pad. 
   It lets you know which directional button was pressed and held and for how long.
 */

public class HoldDPadOnscreen : BaseDPadOnScreen {

	//bools to indicate which buttons held
	private bool holdingUpButton;
	private bool holdingDownButton;
	private bool holdingLeftButton;
	private bool holdingRightButton;
	
	//variable to count how long buttons held
	private int upButtonHeldCount;
	private int downButtonHeldCount;
	private int leftButtonHeldCount;
	private int rightButtonHeldCount;
	
	// Use this for initialization
	public void Start () 
	{
		base.Start();
		//initialize button names
		setUpButtonName("HoldUp");
		setDownButtonName("HoldDown");
		setLeftButtonName("HoldLeft");
		setRightButtonName("HoldRight");
	}
	
	// Update is called once per frame
	public void Update () 
	{
		base.Update();
		
		//if buttons status are active, increment respective counts 
		//else reset respective button counts
		if(getUpButtonActive()){incrementUpButtonHeldCount();}
		else{ resetUpButtonHeldCount();}
		
		if(getDownButtonActive()){incrementDownButtonHeldCount();}
		else{ resetDownButtonHeldCount();}
		
		if(getLeftButtonActive()){incrementLeftButtonHeldCount();}
		else{ resetLeftButtonHeldCount();}
		
		if(getRightButtonActive()){incrementRightButtonHeldCount();}
		else{ resetRightButtonHeldCount();}
		
		//if button held counts are more than 5, number determined through simple testing
		//then, declare as button being held
		
		if(getUpButtonHeldCount() >= 5){ setHoldingUpButtonBool(true);}
		else{ setHoldingUpButtonBool(false);}
		
		if(getDownButtonHeldCount() >= 5){ setHoldingDownButtonBool(true);}
		else{ setHoldingDownButtonBool(false);}
		
		if(getLeftButtonHeldCount() >= 5){ setHoldingLeftButtonBool(true);}
		else{ setHoldingLeftButtonBool(false);}
		
		if(getRightButtonHeldCount() >= 5){ setHoldingRightButtonBool(true);}
		else{ setHoldingRightButtonBool(false);}
		
		//if button held counts are more than 50, reset counts 
		if(getUpButtonHeldCount() >= 50){ resetUpButtonHeldCount();}
		
		if(getDownButtonHeldCount() >= 50){ resetDownButtonHeldCount();}
		
		if(getLeftButtonHeldCount() >= 50){ resetLeftButtonHeldCount();}
		
		if(getRightButtonHeldCount() >= 50){ resetRightButtonHeldCount();}
	}
	
	//functions to increment count variables of buttons
	private void incrementUpButtonHeldCount(){ upButtonHeldCount++;}
	private void incrementDownButtonHeldCount(){ downButtonHeldCount++;}
	private void incrementLeftButtonHeldCount(){ leftButtonHeldCount++;}
	private void incrementRightButtonHeldCount(){ rightButtonHeldCount++;}
	
	//function to reset count variables of buttons
	protected void resetUpButtonHeldCount(){ upButtonHeldCount = 0;}
	protected void resetDownButtonHeldCount(){ downButtonHeldCount = 0;}
	protected void resetLeftButtonHeldCount(){ leftButtonHeldCount = 0;}
	protected void resetRightButtonHeldCount(){ rightButtonHeldCount = 0;}
	
	//functions to return count variables of buttons
	public int getUpButtonHeldCount(){ return upButtonHeldCount;}
	public int getDownButtonHeldCount(){ return downButtonHeldCount;}
	public int getLeftButtonHeldCount(){ return leftButtonHeldCount;}
	public int getRightButtonHeldCount(){ return rightButtonHeldCount;}
	
	//functions to set status of buttons being held
	protected void setHoldingUpButtonBool(bool status){holdingUpButton = status;}
	protected void setHoldingDownButtonBool(bool status){holdingDownButton = status;}
	protected void setHoldingLeftButtonBool(bool status){holdingLeftButton = status;}
	protected void setHoldingRightButtonBool(bool status){holdingRightButton = status;}
	
	//functions to get status of buttons being held
	public bool getHoldingUpButtonBool(){ return holdingUpButton;}
	public bool getHoldingDownButtonBool(){ return holdingDownButton;}
	public bool getHoldingLeftButtonBool(){ return holdingLeftButton;}
	public bool getHoldingRightButtonBool(){ return holdingRightButton;}
}
