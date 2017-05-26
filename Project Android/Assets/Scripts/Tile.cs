using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool impassible;
    public Unit unit = null;
    public int[] index = new int[2];
    public bool protoTarget; //prototype yellow target tile

    public void SetColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    //Flashes red when attacked and signals the occupying unit it was attacked if that unit exists
    public void Attacked()
    {
        //Flash red
        if (unit != null)
            unit.Damaged();
    }
}
