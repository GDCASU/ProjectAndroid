using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Michael Nahom
 * Date: 6/5/17
 * Description:
    Example of how to extend the TileAPI class to create a three behavior
    tile - rotates the player to a random direction and, picks up any item
    on the tile, upon entering, and heals the unit when it leaves the tile
*/

public class ThreeBehaviorsExample : TileAPI{
    public int health;

    public override void OnEnter(Object[] args = null)
    {
        tile.RotateUnit(Random.Range(0, 4));
        tile.PickUpItem();
    }

    public override void OnExit(Object[] args = null)
    {
        tile.HealUnit(health);
    }
}