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

    private bool active;
    private Color color;
    private int randomInt;
    public float maxDelay;
    private float moveDelay;

	void Start () {
        color = GetComponent<Renderer>().material.color;
        if (maxDelay == 0)
            maxDelay = 1.0f;
        moveDelay = maxDelay;
    }
	
	void Update () {
        if (moveDelay <= 0.0f)
        {
            int xIndex = occupiedTile.index[0];
            int yIndex = occupiedTile.index[1];
            randomInt = Random.Range((int)0, (int)9);
            if (randomInt == 0) //Move right
            {
                if (xIndex < tileMap.mapWidth - 1 && !tileMap.map[xIndex + 1, yIndex].impassible && tileMap.map[xIndex + 1, yIndex].unit == null)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex + 1, yIndex];
                    tileMap.map[xIndex + 1, yIndex].unit = this;
                    transform.position = tileMap.map[xIndex + 1, yIndex].transform.position;
                }
            }
            else if (randomInt == 1) //Move up
            {
                if (yIndex < tileMap.mapHeight - 1 && !tileMap.map[xIndex, yIndex + 1].impassible && tileMap.map[xIndex, yIndex + 1].unit == null)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex, yIndex + 1];
                    tileMap.map[xIndex, yIndex + 1].unit = this;
                    transform.position = tileMap.map[xIndex, yIndex + 1].transform.position;
                }
            }
            else if (randomInt == 2) //Move left
            {
                if (xIndex > 0 && !tileMap.map[xIndex - 1, yIndex].impassible && tileMap.map[xIndex - 1, yIndex].unit == null)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex - 1, yIndex];
                    tileMap.map[xIndex - 1, yIndex].unit = this;
                    transform.position = tileMap.map[xIndex - 1, yIndex].transform.position;
                }
            }
            else if (randomInt == 3) //Move down
            {
                if (yIndex > 0 && !tileMap.map[xIndex, yIndex - 1].impassible && tileMap.map[xIndex, yIndex - 1].unit == null)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex, yIndex - 1];
                    tileMap.map[xIndex, yIndex - 1].unit = this;
                    transform.position = tileMap.map[xIndex, yIndex - 1].transform.position;
                }
            }
            moveDelay = maxDelay;
        }
	}

    public void SetActive()
    {
        active = true;
        color = Color.red;
    }

    public override void Damaged()
    {
        if (active)
        {
            active = false;
            color = Color.blue;
            //Signal another DemoEnemy to become active
        }
    }
}
