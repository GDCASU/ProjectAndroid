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

public class Player : Unit {

    private int playerDirection; //Set to whatever direcction the player is initially facing
    public float maxDelay;
    private float moveDelay;

    void Start()
    {
        if (maxDelay == 0)
            maxDelay = 1.0f;
        moveDelay = maxDelay;
    }

    void Update () {
        //If a tile is tapped on screen, do 
        MoveTileTap(tile);
        //Else if D-Pad is held, pressed, or the joystick is held do
        MoveDirection(direction);         
        //If attack button is pressed
        Attack();
	}

    //Move directly to a tapped tile
    void MoveTileTap (Tile tile)
    {
        if (!tile.impassible && tile.unit == null && moveDelay <= 0.0f)
        {
            occupiedTile.unit = null;
            occupiedTile = tile;
            tile.unit = this;
            transform.position = tile.transform.position; //Jump directly to the tile for now
            moveDelay = maxDelay;
        }
    }

    //Rotate to face direction and then attempt to move in that direction
    //Right = 0, up = 1, left = 2, down = 3
    void MoveDirection(int direction)
    {
        int xIndex = occupiedTile.index[0];
        int yIndex = occupiedTile.index[1];
        switch (direction)
        {
            case 0:
                transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
                playerDirection = 0;
                if (xIndex < tileMap.mapWidth - 1 && !tileMap.map[xIndex + 1, yIndex].impassible && tileMap.map[xIndex + 1, yIndex].unit == null && moveDelay <= 0.0f)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex + 1, yIndex];
                    tileMap.map[xIndex + 1, yIndex].unit = this;
                    transform.position = tileMap.map[xIndex + 1, yIndex].transform.position;
                    moveDelay = maxDelay;
                }
                break;
            case 1:
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                playerDirection = 1;
                if (yIndex < tileMap.mapHeight - 1 && !tileMap.map[xIndex, yIndex + 1].impassible && tileMap.map[xIndex, yIndex + 1].unit == null && moveDelay <= 0.0f)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex, yIndex + 1];
                    tileMap.map[xIndex, yIndex + 1].unit = this;
                    transform.position = tileMap.map[xIndex, yIndex + 1].transform.position;
                    moveDelay = maxDelay;
                }
                break;
            case 2:
                transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
                playerDirection = 2;
                if (xIndex > 0 && !tileMap.map[xIndex - 1, yIndex].impassible && tileMap.map[xIndex - 1, yIndex].unit == null && moveDelay <= 0.0f)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex - 1, yIndex];
                    tileMap.map[xIndex - 1, yIndex].unit = this;
                    transform.position = tileMap.map[xIndex - 1, yIndex].transform.position;
                    moveDelay = maxDelay;
                }
                break;
            case 3:
                transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
                playerDirection = 3;
                if (yIndex > 0 && !tileMap.map[xIndex, yIndex - 1].impassible && tileMap.map[xIndex, yIndex - 1].unit == null && moveDelay <= 0.0f)
                {
                    occupiedTile.unit = null;
                    occupiedTile = tileMap.map[xIndex, yIndex - 1];
                    tileMap.map[xIndex, yIndex - 1].unit = this;
                    transform.position = tileMap.map[xIndex, yIndex - 1].transform.position;
                    moveDelay = maxDelay;
                }
                break;
            default:
                Debug.Log("Invalid direction, no movement taken.");
                break;
        }

    }

    //Attack the tile in front of the player
    public override void Attack()
    {
        int xIndex = occupiedTile.index[0];
        int yIndex = occupiedTile.index[1];
        switch (playerDirection)
        {
            case 0:
                if (xIndex < tileMap.mapWidth - 1)
                    tileMap.map[xIndex + 1, yIndex].Attacked();
                break;
            case 1:
                if(yIndex < tileMap.mapHeight - 1)
                    tileMap.map[xIndex, yIndex + 1].Attacked();
                break;
            case 2:
                if (xIndex > 0)
                    tileMap.map[xIndex - 1, yIndex].Attacked();
                break;
            case 3:
                if (yIndex > 0)
                    tileMap.map[xIndex, yIndex - 1].Attacked();
                break;
            default:
                Debug.Log("Invalid direction, no attack made.");
                break;
        }
    }
}
