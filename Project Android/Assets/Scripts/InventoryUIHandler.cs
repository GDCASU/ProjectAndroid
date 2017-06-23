using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour {
    public GameObject backgroundImage;
    public GameObject textDisplay;
    public GameObject weaponScrollviewContent;
    public GameObject weaponDisplay;
    public GameObject leftEquippedWeaponDisplay;
    public GameObject rightEquippedWeaponDisplay;

    private Inventory inventory;
    private Item selectedItem;
    private bool visible;
    private int numCols;
    private int numRows;
    private GameObject[] weapons;

    void Start() {
        weapons = new GameObject[] { weaponDisplay };
	}
	
	void Update () {
        
	}

    //displays the inventory ui on screen
    public void Show()
    {
        Player player = Player.FindPlayer();
        if (inventory == null)
        {
            inventory = player.GetInventory(); 
        }
        int numCols = (int)System.Math.Ceiling(player.GetInventory().maxItems / 2.0);
        if (weapons.Length != numCols * 2)
        {
            weapons = new GameObject[numCols*2];
            weapons[0] = weaponDisplay;
            for (int i = 1; i < numCols * 2; i++)
            {
                weapons[i] = Instantiate(weaponDisplay,weaponScrollviewContent.transform);
            }
        }
        for (int i = 0; i < inventory.GetContents().Count; i++)
        {
            Item item = inventory.GetContents()[i];
            weapons[i].GetComponent<Image>().sprite = 
                item.GetComponentInChildren<Image>().sprite;
            weapons[i].GetComponent<InventoryDragHandler>().item = item;
        }
        SetSelectedItem(inventory.GetContents()[0]);
        leftEquippedWeaponDisplay.GetComponent<Image>().sprite =
            player.GetLeftWeapon().GetComponentInChildren<Image>().sprite;
        rightEquippedWeaponDisplay.GetComponent<Image>().sprite =
            player.GetRightWeapon().GetComponentInChildren<Image>().sprite;
        visible = true;
        backgroundImage.gameObject.SetActive(true);
    }

    //hides the inventory ui on screen
    public void Hide()
    {
        visible = false;
        backgroundImage.gameObject.SetActive(false);
    }

    //sets the currently selected item in the inventory
    public void SetSelectedItem(Item item)
    {
        selectedItem = item;
        textDisplay.GetComponent<Text>().text = selectedItem.GetInventoryText();
    }

    //equips a weapon in the player's left weapon slot
    public void SetLeftWeapon(Weapon weapon)
    {
        Player player = Player.FindPlayer();
        //if the player attempts to place the same weapons in both slots
        //swap the slots instead
        if (player.GetRightWeapon() == weapon)
        {
            player.SetRightWeapon(player.GetLeftWeapon());
            rightEquippedWeaponDisplay.GetComponent<Image>().sprite =
                player.GetRightWeapon().GetComponentInChildren<Image>().sprite;
        }

        player.SetLeftWeapon(weapon);
        leftEquippedWeaponDisplay.GetComponent<Image>().sprite =
                player.GetLeftWeapon().GetComponentInChildren<Image>().sprite;
    }

    public void SetRightWeapon(Weapon weapon)
    {
        Player player = Player.FindPlayer();
        if (player.GetLeftWeapon() == weapon)
        {
            player.SetLeftWeapon(player.GetRightWeapon());
            leftEquippedWeaponDisplay.GetComponent<Image>().sprite =
                player.GetLeftWeapon().GetComponentInChildren<Image>().sprite;
        }
        
        player.SetRightWeapon(weapon);
        rightEquippedWeaponDisplay.GetComponent<Image>().sprite =
                player.GetRightWeapon().GetComponentInChildren<Image>().sprite;
    }
}
