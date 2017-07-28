using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: Passes title screen input to the current overlord. 

public class TitleInputHandler : MonoBehaviour {

    Overlord overlord;

    private void Start()
    {
        overlord = GameObject.FindWithTag("Overlord").GetComponent<Overlord>();
    }

	public void ToggleLeftHanded(bool left)
    {
        overlord.SetLeftHanded(left);
    }

    public void StartGame()
    {
        overlord.StartGame();
    }

    public void EnterTestRoom()
    {
        overlord.EnterTestRoom();
    }

    public void SetupControlButtons()
    {
        overlord.SetupControlButtons();
    }
}
