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

 * Programmer: Edward Borroughs
 * Date: 7/12/17
 * Description:
    Added a list to keep track of the stauses a unit is suffering from,
    an enum for the different types of stauses, and the upkeep method that 
    checks all of the unit's current statuses and applies their effects. 
 */

public class Unit : MonoBehaviour
{
    public Tile occupiedTile;
    public TileMap tileMap;
    public int direction;
    protected Inventory inventory;
    public Weapon equippedWeapon;
    public List<Status> unitStatus; //List of the unit's currennt statuses
    [Header("Health and Damage")]
    public RectTransform healthBar;
    public int maxHealth;
    protected int currentHealth;

    public float turnTimer;
    public float speed;
    public bool isActiveUnit; //is this unit currently taking its turn?


    //UnitID is the "team" of the unit. Units on the same team can't damage eachother. 
    //Enemies have an ID of 0, the player has an ID of 1.
    public int unitId;

    void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        inventory = gameObject.GetComponent<Inventory>();
        if (equippedWeapon != null)
        {
            inventory.AddToInventory(equippedWeapon);
        }
    }

    private void LateUpdate()
    {
        healthBar.localScale = new Vector3((float)currentHealth / (float)maxHealth, 1, 1);
    }

    public virtual void DoTurn()
    {
        //will have to decide how to detect when each phase is finished and continue
        //easily changed later, for now just execute them in order on one frame
        Upkeep();
        StartCoroutine(MainAction());
    }

    public virtual void Upkeep()
    {
        //tick status conditions
        foreach (Status status in unitStatus)
        {
            //Add addditional status effects here
            switch(status)
            {
                case Status.Fire:
                    return;
            }
        }
    }

    public virtual IEnumerator MainAction()
    {
        //move, attack
        yield return new WaitForSeconds(0.01f); //wait for movement to finish
        Move();
        Downkeep();
    }

    public virtual void Downkeep()
    {
        TurnHandler.NextTurn();
    }

    public virtual void Move()
    {
        
    }

    public virtual void Attack()
    {
        equippedWeapon.PerformAttack(this, direction);
    }

    public virtual void Damaged(int damage, int sourceID)
    {
        if (unitId != sourceID)
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

    public virtual int GetId()
    {
        return unitId;
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

    //Add additional statuses here
    public enum Status
    {
        Fire
    }
}
