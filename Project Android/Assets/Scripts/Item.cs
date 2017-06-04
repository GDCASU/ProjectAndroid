using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    /*
    * Programmer: Michael Nahom
    * Date: 6/4/17
    * Description:
        Item class. May or may not be part of an Inventory
        Provides helper methods for adding to, removing from an inventory.
        Also tracks the currently active weapon of each type (gun and sword)
    */
    
    protected Inventory inventory;
    protected bool onMap;
    public Tile occupiedTile;
    protected TileMap tileMap;

    void Start () {
        if (gameObject != null && inventory == null)
        {
            //in case the item is added from the inspector window
            //make sure to track the inventory it's in
            SetInventory(gameObject.GetComponent<Inventory>());
        }
    }
	
	void Update () {
		
	}

    public bool PlaceOnTile(Tile target)
    {
        //Attempts to place an Item on a tile.  Fails if the tile is occupied
        //or impassible. Also fails if the item is in an inventory but cannot 
        //be removed.

        TileMap tempT = GetTileMap();
        //need a temporary copy since GetTileMap() will return null once the
        //inventory is removed

        if (target.unit || target.impassible)
            return false;
        if (inventory != null && !(inventory.RemoveFromInventory(this)))
            return false;

        target.item = this;
        onMap = true;
        occupiedTile = target;
        tileMap = tempT;
        return true;
    }

    public bool PickUpFromTile(Inventory inv)
    {
        if ((!onMap) || inv == null)
            return false;

        inv.AddToInventory(this);
        onMap = false;
        occupiedTile = null;
        tileMap = null; //now tracked by inventory.unit.tileMap
        return true;
    }

    public void SetInventory(Inventory inv)
    {
        if (inv == null)
        {
            inventory = inv;
        }
        else if (inv.Contains(this))
        {
            //SetInventory() only changes the inventory an Item THINKS it's in
            //so don't change it unless the inventory actually contains the
            //item.
            inventory = inv;
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public Unit GetUnit()
    {
        //returns the unit that the parent inventory is attached TO
        return GetInventory().GetUnit();
    }

    public TileMap GetTileMap()
    {
        //returns the tile map that the parent unit is in
        if (GetInventory() != null)
        {
            return GetUnit().tileMap;
        }
        else if (onMap)
        {
            return tileMap;
        }
        else return null;
    }

    public virtual void PerformAttack(Tile target)
    {

    }

    public virtual void ApplyStatusEffect(Tile target)
    {

    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.RemoveFromInventory(this);
            //remove any references before destroying the item
        }
    }
}
