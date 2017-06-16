using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Programmer: Edward Borroughs
 * Date: 5/25/17
 * Description:
    Base class for all game units  

 * Programmer: Edward Borroughs
 * Date: 5/25/17
 * Description:
    All units now have health, a health bar, and can be damaged. 
 */

public class Unit : MonoBehaviour
{

    public Tile occupiedTile;
    public TileMap tileMap;
    public int direction;
    protected Inventory inventory;
    public float moveDelay;
    public Weapon equippedWeapon;
    [Header("Health and Damage")]
    public RectTransform healthBar;
    public int maxHealth;
    protected int currentHealth;

    public int unitId;
    
    protected float moveTimer;
    

    //UnitID is the "team" of the unit. Units on the same team can't damage eachother. 
    //Enemies have an ID of 0, the player has an ID of 1.
    protected int unitID;

    void Awake()
    {
        if (moveDelay == 0)
            moveDelay = 1.0f;
        moveTimer = Random.Range(0, moveDelay);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        healthBar.localScale = new Vector3((float)currentHealth / (float)maxHealth, 1, 1);
    }

    public virtual void MoveUpdate()
    {
        if (moveTimer <= 0.0f)
        {
            Move();
        }
        moveTimer -= Time.deltaTime;
    }

    public virtual void Move()
    {
        moveTimer = moveDelay;
    }

    public virtual void Attack()
    {
        equippedWeapon.PerformAttack(this, direction);
    }

    public virtual void Damaged(int damage, int sourceID)
    {
        if (unitID != sourceID)
        {
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0; //so the health bar doesn't become negative
                KillUnit();
            }
        }
    }
    public virtual void Healed(int health)
    {
        currentHealth = currentHealth + health;
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;
        inventory.SetUnit(this);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public virtual int getID()
    {
        return unitID;
    }

    public virtual void KillUnit()
    {
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

    protected virtual Tile GetTarget()
    {
        int xPos = (int)occupiedTile.mapPos.x;
        int yPos = (int)occupiedTile.mapPos.y;

        Tile target = tileMap.GetNeighbors(xPos, yPos)[direction];
        return target;
    }
}
