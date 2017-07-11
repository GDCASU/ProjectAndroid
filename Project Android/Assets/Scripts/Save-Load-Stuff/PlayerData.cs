using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData : MonoBehaviour {

	/*
		Programmer: Pablo Camacho
		Date: 07/08/17
		Description: I made this class to contain player data to be used with GlobalControl class. 
		Based on this tutorial and others after it. https://www.sitepoint.com/saving-data-between-scenes-in-unity/ 
		I added serializable header to tell Unity engine that it can be written in binary form.
	*/
	
	
	//Player data to save/load
	
	//player left weapon;
	public int leftWeaponDamage;
    public int leftWeaponRange;
    public string leftWeaponName;
	//public player right weapon;
	public int rightWeaponDamage;
    public int rightWeaponRange;
    public string rightWeaponName;
}
