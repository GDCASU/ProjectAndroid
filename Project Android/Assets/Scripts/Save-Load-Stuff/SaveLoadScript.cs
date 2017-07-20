using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadScript : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 07/11/17
		Description: I made this script to make it conventient to calls functions from Global Control object.
	*/

	public void SaveTempDataToSaveFile()
	{
		GlobalControl.GetGlobalControlSaveLoadObject().SaveDataToSaveFile();
	}
	
	public void LoadSaveFileDataToTempData()
	{
		GlobalControl.GetGlobalControlSaveLoadObject().LoadDataFromSaveFile();
	}
}
