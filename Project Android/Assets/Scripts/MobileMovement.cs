using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

/*
 * Programmer: Pablo Camacho
 * Date: 05/22/17
 * Description:
	Script to test mobile joystick, on screen touch d-pad, and on screen hold d-pad
 */

public class MobileMovement : MonoBehaviour {

	//varaible reference to joystick used
	public Joystick mobileJoystick;
	
	//variable reference to text item that displays x position of mobile joystick 
	public Text xMobileJoystickText;
	
	//variable reference to text item that displays y position of mobile joystick 
	public Text yMobileJoystickText;
	
	//variable reference to text item that displays direction of mobile joystick
	public Text mobileJoystickDirectionText;
	string joystickDirectionString;
	private float initialJoystickXPosition;
	private float initialJoystickYPosition;
	 
	//variable reference to touch d pad used
	public TouchDPadOnscreen touchDPad;
	
	//variable reference to text item that displays touch d pad direction
	public Text touchDPadDirectionText;
	
	//variable reference to hold d pad used
	public HoldDPadOnscreen holdDPad;
	
	//variable reference to text item that displays hold d pad direction
	public Text holdDPadDirectionText;
	
	//variable reference to text item that displays hold d pad hold count
	public Text holdDPadHoldCountText;
	string holdCountString = "0";
	
	// Use this for initialization
	void Start () 
	{
		initialJoystickXPosition = mobileJoystick.transform.position.x;
		initialJoystickYPosition = mobileJoystick.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		xMobileJoystickText.text = mobileJoystick.transform.position.x.ToString();
		yMobileJoystickText.text = mobileJoystick.transform.position.y.ToString();
		
		//if joystick icon moved up(moved above original y coordinate), move up
		
		if(mobileJoystick.transform.position.y > initialJoystickYPosition)
		{
			//if moved right
			if(mobileJoystick.transform.position.x > initialJoystickXPosition)
			{
				joystickDirectionString = "Up Right";
			}
			//else if moved left
			else if(mobileJoystick.transform.position.x < initialJoystickXPosition)
			{
				joystickDirectionString = "Up Left";
			}
			else{joystickDirectionString = "Up";}
		}
		//else if moved down
		else if(mobileJoystick.transform.position.y < initialJoystickYPosition)
		{	
			//if moved right
			if(mobileJoystick.transform.position.x > initialJoystickXPosition)
			{
				joystickDirectionString = "Down Right";
			}
			//else if moved left
			else if(mobileJoystick.transform.position.x < initialJoystickXPosition)
			{
				joystickDirectionString = "Down Left";
			}
			else{joystickDirectionString = "Down";}
		}
		//else if not moved up or down
		else
		{
			//if moved right
			if(mobileJoystick.transform.position.x > initialJoystickXPosition)
			{
				joystickDirectionString = "Right";
			}
			//else if moved left
			else if(mobileJoystick.transform.position.x < initialJoystickXPosition)
			{
				joystickDirectionString = "Left";
			}
			else{joystickDirectionString = "None";}
		}
		
		mobileJoystickDirectionText.text = joystickDirectionString;
		
		//Touch D Pad Info
		
		if(touchDPad.getPressedUpButtonBool()){touchDPadDirectionText.text = "Up";}
		else if(touchDPad.getPressedDownButtonBool()){touchDPadDirectionText.text = "Down";}
		else if(touchDPad.getPressedLeftButtonBool()){touchDPadDirectionText.text = "Left";}
		else if(touchDPad.getPressedRightButtonBool()){touchDPadDirectionText.text = "Right";}
		else{touchDPadDirectionText.text = "None";}
		
		//Hold D Pad Info
		if(holdDPad.getHoldingUpButtonBool())
		{
			holdDPadDirectionText.text = "Up"; 
			holdCountString = holdDPad.getUpButtonHeldCount().ToString();
		}
		else if(holdDPad.getHoldingDownButtonBool())
		{
			holdDPadDirectionText.text = "Down"; 
			holdCountString = holdDPad.getDownButtonHeldCount().ToString();
		}
		else if(holdDPad.getHoldingLeftButtonBool())
		{
			holdDPadDirectionText.text = "Left";
			holdCountString = holdDPad.getLeftButtonHeldCount().ToString();
		}
		else if(holdDPad.getHoldingRightButtonBool())
		{
			holdDPadDirectionText.text = "Right";
			holdCountString = holdDPad.getRightButtonHeldCount().ToString();
		}
		else
		{
			holdDPadDirectionText.text = "None";
			holdCountString = "0";
		}
		
		holdDPadHoldCountText.text = holdCountString;
		
	}
}
