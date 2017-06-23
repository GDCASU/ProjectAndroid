using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler {

    public GameObject inventoryUI;
    public GameObject canvas;
    public Item item;

    //copy the gameObject and give the copy a new parent so that
    //it can be moved out of the scroll window
    private GameObject mobileCopy;
    private GraphicRaycaster gr;

    public void OnPointerDown(PointerEventData data)
    {
        if (item == null) return;

        inventoryUI.GetComponent<InventoryUIHandler>().SetSelectedItem(item);
        mobileCopy = Instantiate(gameObject, inventoryUI.transform,true);
        gameObject.GetComponent<Image>().color = Color.clear;
    }

    public void OnDrag(PointerEventData data)
    {
        if (item == null) return;

        mobileCopy.transform.position = data.position;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (item == null) return;

        gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject == 
                inventoryUI.GetComponent<InventoryUIHandler>().leftEquippedWeaponDisplay
                && item is Weapon)
            {
                inventoryUI.GetComponent<InventoryUIHandler>().SetLeftWeapon((Weapon)item);
            }
            if (results[i].gameObject ==
                inventoryUI.GetComponent<InventoryUIHandler>().rightEquippedWeaponDisplay
                && item is Weapon)
            {
                inventoryUI.GetComponent<InventoryUIHandler>().SetRightWeapon((Weapon)item);
            }
        }

        Destroy(mobileCopy);
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
