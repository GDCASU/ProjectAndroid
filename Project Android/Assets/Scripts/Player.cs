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

    void Start()
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
        }
    }

    public void SetRightWeapon(Weapon weapon)
    {
        if (inventory.Contains(weapon))
        {
            rightWeapon = weapon;
        }
    }
}
