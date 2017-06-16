using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: Since the joystick is disabled when not being actively moved,
//              this script, attached to a transparent image, catches pointer events
//              and passes them to the joystick.

public class JoystickCatcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public Joystick joystick;

    public void OnDrag(PointerEventData eventData)
    {
        joystick.Drag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.PointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.PointerUp(eventData);
    }
}
