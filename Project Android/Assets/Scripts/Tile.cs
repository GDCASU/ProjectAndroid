using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool impassible;
    public Tile unit; //change to Unit type when possible
    public bool protoTarget; //prototype yellow target tile

    public void SetColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }
}
