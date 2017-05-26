using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public string Name;
    public bool allowHold;

    void OnEnable()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetDownState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetUpState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (allowHold && eventData.pointerPress && eventData.pointerPress.GetComponent<ButtonHandler>())
        {
            SetDownState();
            eventData.pointerPress.GetComponent<Selectable>().OnPointerUp(eventData);
            GetComponent<Selectable>().OnPointerDown(eventData);
            eventData.pointerPress = gameObject;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetUpState();
    }

    public void SetDownState()
    {
        CrossPlatformInputManager.SetButtonDown(Name);
    }


    public void SetUpState()
    {
        CrossPlatformInputManager.SetButtonUp(Name);
    }


    public void SetAxisPositiveState()
    {
        CrossPlatformInputManager.SetAxisPositive(Name);
    }


    public void SetAxisNeutralState()
    {
        CrossPlatformInputManager.SetAxisZero(Name);
    }


    public void SetAxisNegativeState()
    {
        CrossPlatformInputManager.SetAxisNegative(Name);
    }

    public void Update()
    {

    }
}
