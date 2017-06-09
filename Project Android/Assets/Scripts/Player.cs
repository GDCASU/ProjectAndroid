﻿using System;
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

    public float maxDelay;
    private float moveDelay;

    void Start()
    {
        activeWeapon = 0; //0 = melee, 1 = gun
        weaponDamage = (new List<int> { 5 , 4 }).ToArray(); //melee = 5, gun = 4 
        currentHealth = maxHealth;
        if (maxDelay == 0)
            maxDelay = 1.0f;
        moveDelay = maxDelay;
        unitID = 1;
    }

    void Update()
    {
        moveDelay -= Time.deltaTime;
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
        if (!tile.impassible && tile.unit == null && moveDelay <= 0.0f)
        {
            tileMap.MoveUnit(occupiedTile, tile, this);

            tileMap.BFSPathFinding((int)occupiedTile.mapPos.x, (int)occupiedTile.mapPos.y);

            moveDelay = maxDelay;
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

        if (moveDelay <= 0.0f)
        {
            Rotate(direction);

            if (!dest.impassible && dest.unit == null)
                tileMap.MoveUnit(occupiedTile, dest, this);

            tileMap.BFSPathFinding((int)occupiedTile.mapPos.x, (int)occupiedTile.mapPos.y);

            moveDelay = maxDelay;
        }

    }

    public override void Attack()
    {
        activeWeapon = 0;
        base.Attack();
    }

    public void Shoot()
    {
        activeWeapon = 1;
        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;

        Tile target = tileMap.GetNeighbors(xPos, yPos)[direction];
        while (target && !target.impassible)
        {
            if (target.unit)
            {
                tileMap.DamageTile(target, weaponDamage[activeWeapon], unitID);
                return;
            }

            xPos = (int)target.mapPos.x;
            yPos = (int)target.mapPos.y;
            target = tileMap.GetNeighbors(xPos, yPos)[direction];
        }
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
        tileMap.instructionText.text = "You Lose";
    }
}
