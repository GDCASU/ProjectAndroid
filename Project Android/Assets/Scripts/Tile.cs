using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int direction;
    public bool impassible;
    public Unit unit = null;
    public Item item = null;
    public Vector2 mapPos = Vector2.zero;
    public bool protoTarget; //prototype yellow target tile
    public GameObject blockPrefab;
    [Header("Advanced API")]
    public TileAPI tileAPI;

    private delegate void tileAPIDel(Object[] args = null);

    public void Start()
    {
        tileAPIDel onEnter = tileAPI.OnEnter;
        tileAPIDel onExit = tileAPI.OnExit;
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
