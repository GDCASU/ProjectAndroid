﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartTasks()
    {
        overlord.StartTasks();
    }

    public void EnterTestRoom()
    {
        overlord.EnterTestRoom();
    }
}