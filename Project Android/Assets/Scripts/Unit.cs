using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 5/25/17
 * Description:
    Base class for all game units  
 */

public class Unit : MonoBehaviour {

    public Tile occupiedTile;
    public TileMap tileMap;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public virtual void Move ()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void Damaged()
    {

    }

}
