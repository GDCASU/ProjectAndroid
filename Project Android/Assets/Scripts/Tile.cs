using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: This script must be attached to all tile objects. It is used
//              to mark them as a tile and to manage information about the tile
//              and units inhabiting the tile. It also is used by the TileAPI
//              to bounce information to the unit on the tile.

public class Tile : MonoBehaviour
{
    public int direction;
    public bool impassible;
    public Unit unit = null;
    public Item item = null;
    public Vector2 mapPos = Vector2.zero;
    public bool protoTarget; //prototype yellow target tile
    public GameObject blockPrefab;

    public int tileId;

    public delegate void TileEventHandler();
    public event TileEventHandler Enter = delegate { };
    public event TileEventHandler Exit = delegate { }; 

    public void Start()
    {
        TileAPI api = GetComponent<TileAPI>();
        if (api)
        {
            Enter += api.OnEnter;
            Exit += api.OnExit;
        }
    }

    public void OnEnter()
    {
        Enter();
    }

    public void OnExit()
    {
        Exit();
    }

    public void SetColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    public void MakeBlock()
    {
        impassible = true;
        Instantiate(blockPrefab, transform.position, transform.rotation).transform.SetParent(transform, true);
    }

    //TILE BEHAVIORS BELOW THIS LINE

    public void DamageUnit(int damage)
    {
        unit.tileMap.DamageTile(this, damage, 100);
    }

    public void HealUnit(int health)
    {
        unit.tileMap.HealTile(this, health);
    }

    public void RotateUnit(int dir)
    {
        unit.Rotate(dir);
    }

    public void PickUpItem()
    {
        if (item != null) unit.GetInventory().AddToInventory(item);
    }
}
