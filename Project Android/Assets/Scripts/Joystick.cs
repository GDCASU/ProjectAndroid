using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour
{
    public enum AxisOption
    {
        // Options for which axes to use
        Both, // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical // Only vertical
    }

    public int MovementRange = 100;
    public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
    public float DeadZone; //percentage of range in which movement not registered
    public float diagZoneWidth;
    public float moveRepeatDelay;

    public bool moved;
    public Vector2 delta;

    public GameObject shadow;

    Vector2 m_StartPos;
    bool m_UseX; // Toggle for using the x axis
    bool m_UseY; // Toggle for using the Y axis
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    int holdDirection;
    float holdTime;

    void Start()
    {
        Transform catcher = transform.parent.parent.parent.Find("JoystickCatcher");
        catcher.gameObject.SetActive(true);
        catcher.GetComponent<JoystickCatcher>().joystick = this;
        OnRectTransformDimensionsChange();
    }

    void OnEnable()
    {
        CreateVirtualAxes();
    }

    void UpdateVirtualAxes(Vector2 value)
    {
        var delta = m_StartPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;
        if (m_UseX)
        {
            m_HorizontalVirtualAxis.Update(-delta.x);
        }

        if (m_UseY)
        {
            m_VerticalVirtualAxis.Update(delta.y);
        }
    }

    void CreateVirtualAxes()
    {
        // set axes to use
        m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        // create new axes based on axes to use
        if (m_UseX)
        {
            m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        }
        if (m_UseY)
        {
            m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
        }
    }

    void Update()
    {
        if (moved)
        {
            //Find angle of joystick direction from +x axis
            Vector2 offset = delta;
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
                                Player player = Player.FindPlayer();
                                if (player)
                                    player.MoveDirection(i / 2);
                            }
                        }
                        else
                        {
                            holdDirection = i;
                            holdTime = 0;
                        }
                    }
                    break;
                }
            }
        }
    }


    public void Drag(PointerEventData data)
    {
        Vector2 newPos = data.position - m_StartPos;
        newPos = newPos.normalized * Mathf.Clamp(newPos.magnitude, 0, MovementRange); //clamp magnitude to movementrange
        delta = newPos;
        moved = (newPos.magnitude / MovementRange > DeadZone); //only register as a move if it's a significant movement
        transform.position = newPos + m_StartPos;
        UpdateVirtualAxes(transform.position);
        
    }


    public void PointerUp(PointerEventData data)
    {
        ((RectTransform)transform).anchoredPosition = Vector2.zero;
        UpdateVirtualAxes(m_StartPos);
        GetComponent<Image>().enabled = false;
        moved = false;

        shadow.GetComponent<Image>().enabled = false;
    }


    public void PointerDown(PointerEventData data)
    {
        m_StartPos = data.position;
        transform.position = m_StartPos;
        GetComponent<Image>().enabled = true;
        holdTime = 0;

        shadow.GetComponent<Image>().enabled = true;
        shadow.transform.position = m_StartPos;
    }

    void OnDisable()
    {
        // remove the joysticks from the cross platform input
        if (m_UseX)
        {
            m_HorizontalVirtualAxis.Remove();
        }
        if (m_UseY)
        {
            m_VerticalVirtualAxis.Remove();
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        MovementRange = (int)(((RectTransform)transform.parent.parent).rect.size.magnitude / 4);
    }
}