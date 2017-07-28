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
		Player.FindPlayer().SavePlayerDataToGlobalControlSaveLoad(); //save player local data to temp save data
		GlobalControl.GetGlobalControlSaveLoadObject().SaveDataToSaveFile(); //save temp save data to save file
	}
	
	public void LoadSaveFileDataToTempData()
	{
		GlobalControl.GetGlobalControlSaveLoadObject().LoadDataFromSaveFile();
	}
}
