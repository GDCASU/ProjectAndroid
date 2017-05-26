using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool impassible;
    public Unit unit = null;
    public Vector2 mapPos = Vector2.zero;
    public bool protoTarget; //prototype yellow target tile
    public GameObject blockPrefab;

    public void SetColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    public void MakeBlock()
    {
        impassible = true;
        Instantiate(blockPrefab, transform.position, transform.rotation).transform.SetParent(transform, true);
    }
}
