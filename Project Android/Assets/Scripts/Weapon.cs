using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: Represents a weapon that can perform attacks. A simple
//              weapon can be created using the constructor, but more complex
//              weapons should be specified in another class that derives from this one.

public class Weapon : Item {

    public int damage;
    public int range;

    public Weapon(int damage, int range, string name = "???")
    {
        this.damage = damage;
        this.range = range;
        this.itemName = name;
    }

    public virtual void PerformAttack(Unit attacker, int direction)
    {
        Tile target = attacker.tileMap.GetNeighbors(attacker.occupiedTile)[direction];
        for (int i = 1; i < range; i++)
        {
            if (target == null || target.unit || target.impassible) break;
            target = attacker.tileMap.GetNeighbors(target)[direction];
        }
        if (target)
            attacker.tileMap.DamageTile(target, damage, attacker.getID());
    }
}
