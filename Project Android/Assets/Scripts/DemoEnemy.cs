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

	void Start () {
        currentHealth = maxHealth;

        if (maxDelay == 0)
            maxDelay = 1.0f;
        moveDelay = Random.Range(0, maxDelay);
    }
	
	void Update () {
        if (moveDelay <= 0.0f)
        {
            int xPos = (int)occupiedTile.mapPos.x;
            int yPos = (int)occupiedTile.mapPos.y;
            int moveDir = Random.Range(0, 4);

            Tile dest = tileMap.GetNeighbors(xPos, yPos)[moveDir];
            if (dest == null) return;

            Rotate(moveDir);

            if (!dest.impassible && dest.unit == null)
            {
                tileMap.MoveUnit(occupiedTile, dest, this);

                //if the enemy moves next to a player, attack it
                //tileMap.MoveUnit updates the occupiedTile field
                xPos = (int)occupiedTile.mapPos.x; 
                yPos = (int)occupiedTile.mapPos.y;
                Tile[] neighbors = tileMap.GetNeighbors(xPos, yPos);
                for (int i=0; i < 4; i++) {
                    if (neighbors[i] != null && neighbors[i].unit is Player) {
                        Rotate(i);
                        Attack();
                     }
                }
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

    public void SetInactive() {
        target = false;
        color = Color.blue;
    }

    public override void Damaged(int damage = 0) {
        if (target) {
            base.Damaged(damage);
            SetInactive();
        }
    }
}
