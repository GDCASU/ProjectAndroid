using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 5/25/17
 * Description:
    Demo enemy class. Has a 50% chance to move in a random direction or sit still every specified time interval.
    If attacked while active the enemy will become inactive and switch from red to blue. 
 */

public class DemoEnemy : Unit {


    public bool target = false;
    public MovementPattern moveType = MovementPattern.Random;
    private Color color {
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
    public float maxDelay;
    private float moveDelay;

    private int moveVar;

	void Start () {
        if (maxDelay == 0)
            maxDelay = 1.0f;
        moveDelay = Random.Range(0, maxDelay);
    }
	
	void Update () {
        if (moveDelay <= 0.0f)
        {
            int xPos = (int)occupiedTile.mapPos.x;
            int yPos = (int)occupiedTile.mapPos.y;
            
            int moveDir = -1;

            switch (moveType) {
                case MovementPattern.Horizontal:
                    moveDir = ((++moveVar) % 2) * 2; //0 or 2
                    break;
                case MovementPattern.Vertical:
                    moveDir = ((++moveVar) % 2) * 2 + 1; //1 or 3
                    break;
                case MovementPattern.Ordinal:
                    moveDir = (++moveVar) % 4; //0 or 1 or 2 or 3, progressing
                    break;
                case MovementPattern.Random:
                    moveDir = Random.Range(0, 4);
                    break;
                default:
                    moveDir = -1;
                    break;
            }
            if (moveDir == -1) return; //invalid move

            Tile dest = tileMap.GetNeighbors(xPos, yPos)[moveDir];
            if (dest == null) return;

            Rotate(moveDir);

            if (!dest.impassible && dest.unit == null)
            {
                tileMap.MoveUnit(occupiedTile, dest, this);
            }
        
            moveDelay = maxDelay;
        }
        moveDelay -= Time.deltaTime;
	}

    public void SetActive()
    {
        target = true;
        color = Color.red;
    }

    public override void Damaged()
    {
        if (target)
        {
            target = false;
            color = Color.blue;
            //Signal another DemoEnemy to become active
        }
    }
}

public enum MovementPattern
{
    Horizontal,
    Vertical,
    Ordinal,
    Random
}
