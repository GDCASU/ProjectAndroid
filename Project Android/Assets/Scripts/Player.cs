using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 5/25/17
 * Description:
    Player class. Has methods for moving to a specific tile given the goal, moving based on D-Pad and Joystick movement, and Attacking 
    the tile directly in front of the player.
 */

/*
 * Programmer: Pablo Camacho
 * Date: 07/11/17
 * Description:
    I added functions to implement methods of loading local data from global save load object to player 
    and saving local data from player to local data in global save load object. 
     
 */
 
public class Player : Unit
{
    [Header("Default Weapons")]
    public Weapon defaultSwordPrefab;
    public Weapon defaultGunPrefab;
    public Weapon thirdWeaponPrefab;

    private bool canMove;

    //the player has access to two weapons at once
    private Weapon leftWeapon;
    private Weapon rightWeapon;
    
    public PlayerData localData; // data to be saved locally and to save file if player so chooses

    void Start()
    {
//		Debug.Log("Player Initialization started! ");
	
		//load initial save data from global control save load object to player
		LoadPlayerDataFromGlobalControlSaveLoad();	
        
        //unitId = 1;
        canMove = true;

        //adding in a third weapon just to test that the inventory UI works properly
        Weapon third = Instantiate(thirdWeaponPrefab, gameObject.transform);
        inventory.AddToInventory(third);
    }

    public void EquipWeapon(string weapon)
    {
        Weapon wep = (Weapon)inventory.GetContents().Find(w => w.itemName == weapon);
        if (wep != null)
            EquipWeapon(wep);
    }

    public void EquipWeapon(Weapon weapon) 
    {
        equippedWeapon = weapon;
    }

    //Move directly to a tapped tile
    public void MoveTileTap(Tile tile)
    {
        float dx = tile.mapPos.x - occupiedTile.mapPos.x;
        float dy = tile.mapPos.y - occupiedTile.mapPos.y;
        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            if (dx > 0) Rotate(0);
            else Rotate(2);
        }
        else
        {
            if (dy > 0) Rotate(3);
            else Rotate(1);
        }
        if (Mathf.Abs(dx) > 1 || Mathf.Abs(dy) > 1 || (Mathf.Abs(dx) == 1 && Mathf.Abs(dy) == 1)) return;
        if (!tile.impassible && tile.unit == null && canMove)
        {
            tileMap.MoveUnit(occupiedTile, tile, this);

            tileMap.BFSPathFinding((int)occupiedTile.mapPos.x, (int)occupiedTile.mapPos.y);

            base.Move();
            canMove = false;
            TurnHandler.NextTurn();
        }
    }

    //Rotate to face direction and then attempt to move in that direction
    //Right = 0, up = 1, left = 2, down = 3
    public void MoveDirection(int direction)
    {
        if (direction < 0 || direction > 3) return;

        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;

        Tile dest = tileMap.GetNeighbors(xPos, yPos)[direction];
        if (dest == null) return;

        if (canMove)
        {
            Rotate(direction);

            if (!dest.impassible && dest.unit == null)
                tileMap.MoveUnit(occupiedTile, dest, this);

            tileMap.BFSPathFinding((int)occupiedTile.mapPos.x, (int)occupiedTile.mapPos.y);

            base.Move();
            canMove = false;
            TurnHandler.NextTurn();
        }

    }

    public override void Move()
    {
        canMove = true;
    }

    public override void Downkeep()
    {
        
    }

    public static Player FindPlayer()
    {
        GameObject plyObj = GameObject.FindGameObjectWithTag("Player");
        if (plyObj == null) return null;
        return plyObj.GetComponent<Player>();
    }

    public override void KillUnit()
    {
        base.KillUnit();
    }

    public Weapon GetLeftWeapon()
    {
        return leftWeapon;
    }

    public Weapon GetRightWeapon()
    {
        return rightWeapon;
    }

    public void SetLeftWeapon(Weapon weapon)
    {
        if (inventory.Contains(weapon))
        {
            leftWeapon = weapon;
            localData.leftWeaponDamage = leftWeapon.damage;
            localData.leftWeaponDamage = leftWeapon.range;
            localData.leftWeaponName = leftWeapon.itemName;
        }
    }

    public void SetRightWeapon(Weapon weapon)
    {
        if (inventory.Contains(weapon))
        {
            rightWeapon = weapon;
            localData.rightWeaponDamage = rightWeapon.damage;
            localData.rightWeaponDamage = rightWeapon.range;
            localData.rightWeaponName = rightWeapon.itemName;
        }
    }
    
    //function to load player data from global control save load object
    //use this for when inside InGame scene
    public void LoadPlayerDataFromGlobalControlSaveLoad()
    {
		//assign saved local player data in global control to local data in player
		GlobalControl globalControlSaveLoad = GlobalControl.GetGlobalControlSaveLoadObject();
		
		if(globalControlSaveLoad.savedLocalPlayerDataTemporary != null){localData = globalControlSaveLoad.savedLocalPlayerDataTemporary;}
		else{localData = globalControlSaveLoad.initialDefaultPlayerData;}
		
		//set left weapon from local save data in global control save load
		if(defaultSwordPrefab != null)
		{
			Weapon savedLeftWeapon = Instantiate(defaultSwordPrefab,gameObject.transform);		
			savedLeftWeapon.damage = localData.leftWeaponDamage;
			savedLeftWeapon.range = localData.leftWeaponRange;
			savedLeftWeapon.itemName = localData.leftWeaponName;
			inventory = GetComponent<Inventory>();
			inventory.AddToInventory(savedLeftWeapon);
			SetLeftWeapon(savedLeftWeapon);
		}
		//set right weapon from local save data in global control save load
		if(defaultGunPrefab != null)
		{
			Weapon savedRightWeapon = Instantiate(defaultGunPrefab,gameObject.transform);		
			savedRightWeapon.damage = localData.rightWeaponDamage;
			savedRightWeapon.range = localData.rightWeaponRange;
			savedRightWeapon.itemName = localData.rightWeaponName;
			inventory = GetComponent<Inventory>();
			inventory.AddToInventory(savedRightWeapon);
			SetRightWeapon(savedRightWeapon);
		}
		//set health from local save data
		currentHealth = localData.health;
	}
	
	//function to load player data from global control save load object
	//use this when getting out of InGame scene to overworld
    public void SavePlayerDataToGlobalControlSaveLoad()
    {
		GlobalControl globalControlSaveLoad = GlobalControl.GetGlobalControlSaveLoadObject();
		
		//set left weapon to local save data in global control save load					
		globalControlSaveLoad.savedLocalPlayerDataTemporary = localData;
		
		
		
		Debug.Log("Left Weapon loaded from initial save data is " + leftWeapon + " damage:" + leftWeapon.damage
					+ " range:" + leftWeapon.range + " name:" + leftWeapon.itemName +"\n");
					
		Debug.Log("Right Weapon loaded from initial save data is " + rightWeapon + " damage:" + rightWeapon.damage
					+ " range:" + rightWeapon.range + " name:" + rightWeapon.itemName +"\n");
		
        
	}
	
	//function to load from default values before save load system
	private void LoadFromDefault()
	{
		inventory = GetComponent<Inventory>();
        if (defaultSwordPrefab != null)
        {
            Weapon sword = Instantiate(defaultSwordPrefab,gameObject.transform);
            inventory.AddToInventory(sword);
            SetLeftWeapon(sword);
            
        }
        if (defaultGunPrefab != null)
        {
            Weapon gun = Instantiate(defaultGunPrefab,gameObject.transform);
            inventory.AddToInventory(gun);
            SetRightWeapon(gun);
        }
        
        currentHealth = maxHealth;
        
        
        Debug.Log("Left Weapon loaded from default is " + leftWeapon + " damage:" + leftWeapon.damage
					+ " range:" + leftWeapon.range +  " name:" + leftWeapon.itemName + "\n");
		
		Debug.Log("Right Weapon loaded from default is " + rightWeapon + " damage:" + rightWeapon.damage
					+ " range:" + rightWeapon.range + " name:" + rightWeapon.itemName +"\n");
        
	}
	
}
