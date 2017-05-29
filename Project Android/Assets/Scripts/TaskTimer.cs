using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTimer : MonoBehaviour {

	//current time on timer
	private float currentTime = 0.0f;

	//text item that display time
	//Note: Have text item anchored to an edge 
	public Text timeText;

	//current task chosen
	private int currentTask;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Add delta time to current time
		currentTime += Time.deltaTime;
		
		//display current time
		timeText.text = currentTime.ToString("0");
	}
	
	public void resetTimer(){currentTime = 0.0f;}
	
	public void changeTask(int thisTask)
	{
		currentTask = thisTask; //assign new case
		resetTimer(); //reset timer
	}
	
}
