using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    /*
    * Programmer: Michael Nahom
    * Date: 6/4/17
    * Description:
        Inventory class. Should always be attached to a unit.
        Provides methods for adding to, removing from the inventory.
        Also tracks the currently active weapon of each type (gun and sword)
    */

    public Item activeSword;
    public Item activeGun;
    public int maxItems = 0; //zero for no limit

    private List<Item> contents = new List<Item>();
    //Never directly call Add() or Remove() on contents since the items need
    //to keep track of which inventory they are in. Use AddToInventory() and
    //RemoveFromInventory() instead.
    private Unit unit;

    void Start () {
        if (gameObject != null && unit == null)
        {
            //in case the inventory is added from the inspector window
            //make sure to track the unit it belongs to
            SetUnit(gameObject.GetComponent<Unit>());
            unit.SetInventory(this);
        }
        if (activeSword != null)
        {
            AddToInventory(activeSword);
        }
        if (activeGun != null)
        {
            AddToInventory(activeGun);
        }
    }
	
	void Update () {
		
	}

    public bool AddToInventory(Item item, bool removeFromCurrent = false)
    {
        //The item will only be transferred from one inventory to another if
        //removeFromCurrent is set to true
        if (!(contents.Count < maxItems || maxItems == 0))
            //TODO somehow let the player know their inventory is full if
            //inventory size is a game feature
            return false;

        if (item.GetInventory() == null)
        {
            contents.Add(item);
            item.SetInventory(this);
            return true;
        }
        else if (removeFromCurrent)
        {
            item.GetInventory().RemoveFromInventory(item);
            AddToInventory(item);
        }
        return false;
    }

    public bool RemoveFromInventory(Item item)
    {
        //returns false on failure (probably b/c item is not in inventory)
        if (contents.Remove(item))
        {
            item.SetInventory(null);
            return true;
        }
        return false;
    }

    public void SetActiveSword(Item item)
    {
        if (Contains(item))
        {
            activeSword = item;
        }
    }

    public void SetActiveGun(Item item)
    {
        if (Contains(item))
        {
            activeGun = item;
        }
    }

    public void SetUnit(Unit u)
    {
        //The inventory will only be transferred from one unit to another if
        //removeFromCurrent is set to true
        unit = u;
    }

    public Unit GetUnit()
    {
        //returns the unit that the Inventory is attached TO
        return unit;
    }

    public bool Contains(Item item)
    {
        //returns true if item is in the inventory
        return contents.Contains(item);
    }
}
