using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 6/8/17
 * Description:
    Simple ranged enemy class. Always follows the shortest path to the player, even if that path is 
    blocked by other units. Moves towards the player if it's manhattan distance to the player is 
    greater than 3, stops moving if it's manhattan distance to the player is equal to 3, and flees
    from the player if it's manhattan distance to the player is less than 3. If this enemy is not 
    currently fleeing after moving, it will try to attack the player if the player is on the same
    row or column as it. Can't shoot through walls or other enemies. 

 */

public class SimpleRangedEnemy : Unit
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
    private bool fleeing;

    private int moveVar;

    void Start()
    {
        equippedWeapon = new Weapon(1, 4, "Simple Gun");
        currentHealth = maxHealth;
        unitId = 0;
    }

    public override void Move()
    {
        fleeing = false;
        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;
        int playerXPos = (int)Player.FindPlayer().occupiedTile.mapPos.x;
        int playerYPos = (int)Player.FindPlayer().occupiedTile.mapPos.y;
        int manhattanDistance = Mathf.Abs(xPos - playerXPos) + Mathf.Abs(yPos - playerYPos);

        if (manhattanDistance > 3)
        {
            int moveDir = occupiedTile.direction;

            if (moveDir == -1) return; //invalid move

            Tile dest = tileMap.GetNeighbors(xPos, yPos)[moveDir];
            if (dest == null) return;

            Rotate(moveDir);

            if (!dest.impassible && dest.unit == null)
                tileMap.MoveUnit(occupiedTile, dest, this);
        }

        else if (manhattanDistance < 3)
        {
            fleeing = true;
            int moveDir = (occupiedTile.direction + 2) % 4;

            if (moveDir == -1) return; //invalid move

            Tile dest = tileMap.GetNeighbors(xPos, yPos)[moveDir];
            if (dest == null) return;

            Rotate(moveDir);

            if (!dest.impassible && dest.unit == null)
                tileMap.MoveUnit(occupiedTile, dest, this);
        }

        if (!fleeing)
        {
            if (xPos == playerXPos)
            {
                if (yPos > playerYPos)
                    direction = 1;
                else
                    direction = 3;
                Attack();
            }

            else if (yPos == playerYPos)
            {
                if (xPos > playerXPos)
                    direction = 2;
                else
                    direction = 0;
                Attack();
            }
        }
        base.Move();
    }
}
