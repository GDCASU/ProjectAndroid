using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldTapHandler : MonoBehaviour {

/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to touch an overworld ship with mouse cursor or tap on mobile and make level menu pop up.
		Borrowed code from TapHandler.
	*/
	
	public LevelMenu levelMenu; //reference to level menu
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		 if (Input.touchCount > 0)
        {
            //if touch started
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                    return;
                OverworldShip owShip = hit.collider.GetComponent<OverworldShip>();
                if (owShip)
                {
					levelMenu.levelMenuGameObject.SetActive(true); //set level menu to active
                    levelMenu.levelTitleShipName.text = owShip.overworldShipName; //set title to name of ship
                    levelMenu.SetOverworldShipReference(owShip); //set reference in level menu to overworld ship
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
            
            GameObject owShipGameObject = hit.collider.gameObject;
            OverworldShip owShip = owShipGameObject.transform.parent.GetComponent <OverworldShip> ();
            
            if (owShip)
            {
                levelMenu.levelMenuGameObject.SetActive(true); //set level menu to active
				levelMenu.levelTitleShipName.text = owShip.overworldShipName; //set title to name of ship
				levelMenu.SetOverworldShipReference(owShip); //set reference in level menu to overworld ship
            }
        }
    }
		
}
