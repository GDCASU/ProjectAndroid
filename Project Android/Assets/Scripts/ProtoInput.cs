using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Programmer: Pablo Camacho
 * Date: 05/22/17
 * Description:
	Script to test mobile joystick, on screen touch d-pad, and on screen hold d-pad
 */

// Developer:   Kyle Aycock
// Date:        5/24/17
// Description: Condensed input parsing code & improved joystick code

public class ProtoInput : MonoBehaviour
{

    //Joystick related fields
    public Joystick mobileJoystick;
    private Vector3 initJoystickPos;
    public float diagZoneWidth;
    public float moveRepeatDelay;
    private int holdDirection = -1;
    private float holdTime;

    //Touch DPad fields
    public DPad touchDPad;

    //Hold DPad fields
    public DPad holdDPad;

    //Tap fields
    public bool tapEnabled;

    public void SetTapEnabled(bool enabled)
    {
        tapEnabled = enabled;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject plyObj = GameObject.FindGameObjectWithTag("Player");
        if (plyObj == null) return;
        Player player = plyObj.GetComponent<Player>();
        if (player == null) return;

        //Joystick
        if (mobileJoystick.moved)
        {
            //Find angle of joystick direction from +x axis
            Vector2 offset = mobileJoystick.delta;
            float angle = -Mathf.Atan2(offset.y, offset.x); //range [-PI, PI]

            //Find width of orthogonal and diagonal zones
            float diagWidth = diagZoneWidth * Mathf.Deg2Rad;
            float orthWidth = (Mathf.PI / 2) - diagWidth;

            if (angle < -orthWidth / 2) angle += Mathf.PI * 2; //fit range to [-orthwidth/2,2Pi-orthwidth/2]
            float upperBound = -orthWidth / 2; //upper bound of each zone for checking against angle

            //string[] directions = { "Right", "Up-Right", "Up", "Up-Left", "Left", "Down-Left", "Down", "Down-Right" };
            for (int i = 0; i < 8; i++)
            {
                //alternate between adding an orthogonal and diagonal zone
                if (i % 2 == 0) upperBound += orthWidth;
                else upperBound += diagWidth;
                if (angle <= upperBound)
                {
                    if (i % 2 == 0) //throw out diagonal inputs
                    {
                        if (holdDirection == i)
                        {
                            holdTime -= Time.deltaTime;
                            if (holdTime < 0)
                            {
                                holdTime = moveRepeatDelay;
                                player.MoveDirection(i / 2);
                            }
                        }
                        else
                            holdDirection = i;
                    }
                    break;
                }
            }
        }

        //D-Pads
        foreach(DPad.Button b in Enum.GetValues(typeof(DPad.Button)))
        {
            if (touchDPad.isActiveAndEnabled && touchDPad.GetButton(b))
                player.MoveDirection((int)b);
            if (holdDPad.isActiveAndEnabled && holdDPad.GetButton(b))
                player.MoveDirection((int)b);

        }

        //Tap
        //if touched once or more
        if (tapEnabled && Input.touchCount > 0)
        {
            //if touch started
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if(!Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                    return;
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile)
                    player.MoveTileTap(tile);
            }
        }

        //Click - for debug
        if (tapEnabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile)
                player.MoveTileTap(tile);
        }
    }

    public void Attack()
    {
        GameObject plyObj = GameObject.FindGameObjectWithTag("Player");
        if (plyObj == null) return;
        Player player = plyObj.GetComponent<Player>();
        if (player == null) return;
        player.Attack();
    }
}
