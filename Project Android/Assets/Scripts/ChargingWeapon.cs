using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingWeapon : Weapon {

	public float chargeTime; //amount of time in seconds that it takes to use weapon again after use
	
	private float timeSinceUse;// amount of time in seconds 
	
	public enum State
	{
		READY_TO_USE = 0,CHARGING
	};
	
	private State weaponState;
	
	public override void PerformAttack(Unit attacker, int direction)
    {
		//if weapon is ready to use
		if(weaponState == State.READY_TO_USE)
		{
			base.PerformAttack(attacker,direction); //perform attack according to base
			weaponState = State.CHARGING; //set weapon state to charging after attack performed
		}
        
    }
    
    public override string GetInventoryText()
    {
        string text = itemName;
        text += "\n\nDamage: ";
        text += damage.ToString();
        text += "\nRange: ";
        text += range.ToString();
        text += "\nCharge Time: ";
        text += chargeTime.ToString();
        return text;
    }
	
	// Use this for initialization
	void Start () 
	{
		timeSinceUse = 0;
		weaponState = State.READY_TO_USE;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//increment timeSinceUse if weapon is in charging state
		if(weaponState == State.CHARGING){timeSinceUse += Time.deltaTime; Debug.Log("Charging Weapon! \n");}
		//put back weapon in ready to use state if time since use is more than or equal to charge time
		if(timeSinceUse >= chargeTime)
		{
			weaponState = State.READY_TO_USE; 
			timeSinceUse = 0;
		}
	}
	
	public float GetCurrentTimeSinceUse(){return timeSinceUse;}
}
