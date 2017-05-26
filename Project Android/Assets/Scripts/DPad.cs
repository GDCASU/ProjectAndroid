using System;
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

// Developer:   Kyle Aycock
// Date:        5/24/17
// Description: Condensed all d-pad related code into this script & simplified.
//              Changed to use real time instead of frame time.
//              

public class DPad : MonoBehaviour
{

    public enum Button
    {
        Right,
        Down,
        Left,
        Up
    }

    public int numButtons = 4;
    public bool protoAllowHold;

    public float buttonHoldThreshold;
    public float buttonHoldRepeatTime;

    //strings containing unique name for buttons to prevent different buttons with same name from being called
    //also to set button names in derived classes.
    private string[] buttonName;

    private float[] buttonHoldTime;
    private bool[] buttonHeld;

    private bool[] buttonActive;

    public DPad()
    {
        buttonName = new string[numButtons];
        buttonHoldTime = new float[numButtons];
        buttonHeld = new bool[numButtons];
        buttonActive = new bool[numButtons];
    }

    public void OnEnable()
    {
        //initialize button names
        setButtonName(Button.Up, transform.Find("UpButton").GetComponent<ButtonHandler>().Name);
        setButtonName(Button.Down, transform.Find("DownButton").GetComponent<ButtonHandler>().Name);
        setButtonName(Button.Left, transform.Find("LeftButton").GetComponent<ButtonHandler>().Name);
        setButtonName(Button.Right, transform.Find("RightButton").GetComponent<ButtonHandler>().Name);

        foreach (Button b in ButtonIter())
        {
            buttonHoldTime[(int)b] = 0;
            buttonHeld[(int)b] = false;
            buttonActive[(int)b] = false;
        }
    }

    public void Update()
    {
        foreach (Button b in ButtonIter())
        {
            if (CrossPlatformInputManager.GetButton(buttonName[(int)b]))
            {
                buttonActive[(int)b] = (buttonHoldTime[(int)b] == 0);
                buttonHoldTime[(int)b] += Time.deltaTime;
            }
            else
                buttonHoldTime[(int)b] = 0;

            if (protoAllowHold)
            {
                if (buttonHoldTime[(int)b] > buttonHoldRepeatTime)
                    buttonHoldTime[(int)b] = 0;
            }
        }
    }

    public bool GetButton(Button button)
    {
        return CrossPlatformInputManager.GetButton(buttonName[(int)button]) && buttonActive[(int)button];
    }

    public float GetButtonHeldTime(Button button)
    {
        return buttonHoldTime[(int)button];
    }

    private Array ButtonIter()
    {
        return Enum.GetValues(typeof(Button));
    }

    protected void setButtonName(Button button, string name)
    {
        buttonName[(int)button] = name;
    }
}
