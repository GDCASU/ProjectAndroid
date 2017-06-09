using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 6/8/17
 * Description:
    Simple melee enemy class. Always follows the shortest path to the player, even if that path is 
    blocked by other units. Automatically attacks the player if it is next to them after attempting
    to move. 

 */

public class SimpleMeleeEnemy : Unit
{

    private Color color
    {
        get
        {
            return transform.Find("Model").GetComponent<Renderer>().material.color;
        }
        set
        {
            transform.Find("Model").GetComponent<Renderer>().material.color = value;
        }
    }

    private int randomInt;
    private int moveVar;

    void Start()
    {
        equippedWeapon = new Weapon(2, 1, "Simple Sword");
        currentHealth = maxHealth;
        unitID = 0;
    }

    public override void Move()
    {
        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;

        int moveDir = occupiedTile.direction;

        if (moveDir == -1) return; //invalid move

        Tile dest = tileMap.GetNeighbors(xPos, yPos)[moveDir];
        if (dest == null) return;

        Rotate(moveDir);

        if (!dest.impassible && dest.unit == null)
            tileMap.MoveUnit(occupiedTile, dest, this);

        xPos = (int)occupiedTile.mapPos.x;
        yPos = (int)occupiedTile.mapPos.y;
        Tile[] neighbors = tileMap.GetNeighbors(xPos, yPos);
        for (int i = 0; i < 4; i++)
        {
            if (neighbors[i] != null && neighbors[i].unit is Player)
            {
                Rotate(i);
                Attack();
            }
        }

        base.Move();
    }

}