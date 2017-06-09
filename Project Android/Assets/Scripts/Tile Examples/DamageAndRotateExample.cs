using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Michael Nahom
 * Date: 6/5/17
 * Description:
     Example of how to extend the TileAPI class to create a "landmine" tile
     which damages the player the first time they step on it, and
     additionally rotates the player to a random direction.
 */

public class DamageAndRotateExample : TileAPI {
    public int damage;
    private bool exploded = false;

    public override void OnEnter()
    {
        if (!exploded)
        {
            tile.DamageUnit(damage);
            exploded = true;
            tile.RotateUnit(Random.Range(0, 4));
        }   
    }

    public override void OnExit()
    {
        return;
    }
}