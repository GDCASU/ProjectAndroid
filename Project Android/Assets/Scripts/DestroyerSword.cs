using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerSword : ChargingWeapon {

	// Use this for initialization
	void Start () 
	{
		base.Start(); //for initialization of variables for charge time
		
		damage = 12;
		range = 1;
		chargeTime = 4;
		itemName = "Destroyer's Sword";
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();//for updating time
	}
}
