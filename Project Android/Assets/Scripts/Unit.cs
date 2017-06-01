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
    public int direction;
    [Header("Health and Damage")]
    public RectTransform healthBar;
    public int maxHealth;
    public int unitDamage;
    //TODO placeholder value for the amount of damage dealt by the unit until a more 
    //sophisticated system (items?) is implemented. Remove if/when no longer necessary.

    protected int currentHealth;

	void Start () {

	}
	
	void Update () {
		
	}

    private void LateUpdate() {
        healthBar.localScale = new Vector3((float)currentHealth / (float)maxHealth, 1, 1);
    }

    public virtual void Move ()
    {

    }

    public virtual void Attack()
    {
        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;

        Tile target = tileMap.GetNeighbors(xPos, yPos)[direction];
        if (target == null) return;

        tileMap.DamageTile(target, getDamage());
    }

    public virtual void Damaged(int damage = 0)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth <= 0) {
            currentHealth = 0;//so the health bar doesn't become negative
            KillUnit();
        }
    }

    public virtual int getDamage() {
        //TODO for now just returns whatever the value of unitDamage is
        //in the future, something like activeItem.damageValue
        return unitDamage;
    }

    public virtual void KillUnit() {
        //any on-death behavior the unit should have

        //default behavior is to destroy the gameObject, but this is dangerous
        //as it may leave references to destroyed objects elsewhere
        //i.e. killing an enemy in one of the test scenarios will not remove it
        //from the TileMap.enemies array
        Destroy(this.gameObject);
    }

    public void Rotate(int dir)
    {
        transform.rotation = Quaternion.Euler(0, dir * 90, 0);
        direction = dir;
    }
}
