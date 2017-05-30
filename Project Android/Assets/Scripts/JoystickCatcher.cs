using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
