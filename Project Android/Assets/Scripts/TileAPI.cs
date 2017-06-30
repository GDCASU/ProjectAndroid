using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Michael Nahom
 * Date: 6/5/17
 * Description:
     Provides an framework for state based tile effects.

 * How To Use:
     A tile prefab should have two scripts, the "Tile" script with the
     tileAPI field set to the second script, which must implement the 
     "TileAPI" class. Common behaviors should be defined in the Tile class
     so that can be shared across different types of tiles without
     copy pasting large amouts of code.  Several examples are provided in
     the "prefabs/Tile Examples" directory.
*/

public abstract class TileAPI : MonoBehaviour
{
    protected Tile tile;

    public void Start()
    {
        tile = gameObject.GetComponent<Tile>();
    }

    //called when a unit enters a tile
    //applied AFTER tile.unit is set to the occupying unit
    public abstract void OnEnter();

    //called when a unit leaves a tile
    //applied BEFORE tile.unit is set to null
    public abstract void OnExit();

    //still needs to be implemented
    public abstract void OnWait();
}
