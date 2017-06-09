using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Michael Nahom
 * Date: 6/5/17
 * Description:
     Example of how to extend the TileAPI class to create a "landmine" tile
     which damages the player the first time they step on it.
 */

public class LandmineExample : TileAPI {
    public int damage;
    private bool exploded = false;

    public override void OnEnter(Object[] args = null)
    {
        if (!exploded)
        {
            tile.DamageUnit(damage);
            exploded = true;
        }
    }

    public override void OnExit(Object[] args = null)
    {
        return;
    }
}
