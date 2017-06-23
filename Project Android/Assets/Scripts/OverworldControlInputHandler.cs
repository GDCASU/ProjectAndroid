using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldControlInputHandler: MonoBehaviour {

/*
		Programmer: Pablo Camacho
		Date: 06/14/17
		Description:
		I made this script to include click and drag camera movement with mouse and touch
		
		Borrowed code for OverworldTapHandler.
	*/
	
	public LevelMenu levelMenu; //reference to level menu
	
	public GalaxyOverworldUpdate galaxyOverworld;//reference to Galaxy Overworld
	
	
	public float dragSpeed; //how fast camera moves in click and drag
	private Vector3 dragOrigin; //where mouse left click made in world
	
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
                
                //if gameobject of overworld ship it
				GameObject owShipGameObject = hit.collider.gameObject;
				OverworldShip owShip = owShipGameObject.transform.parent.GetComponent <OverworldShip> ();
                
                if (owShip)
                {
					levelMenu.levelMenuGameObject.SetActive(true); //set level menu to active
                    levelMenu.levelTitleShipName.text = owShip.overworldShipName; //set title to name of ship
                    levelMenu.SetOverworldShipReference(owShip); //set reference in level menu to overworld ship
                }
            }
            //else if touch involved move across screen
            else if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
				 // Get movement of the finger since last frame
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				
				//make move vector based on delta position
				Vector3 move = new Vector3(touchDeltaPosition.x * dragSpeed, touchDeltaPosition.y * dragSpeed, 0.0f);
				//move camera with click and drag by move vector
				galaxyOverworld.MoveThroughSectionWithClickAndDrag(move);
			}
        }
        //if left mouse button pressed down
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
				//if nothing hit, assign mouse position to drag Origin
				dragOrigin = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0.0f);
			}
            else
            {
				//if gameobject of overworld ship it
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
        //if left mouse button released
        else if(Input.GetMouseButtonUp(0))
        {
			//make move vector from difference in position between current mouse position and dragOrigin and multiply it by dragSpeed
			Vector3 move = new Vector3((Input.mousePosition.x - dragOrigin.x) * dragSpeed,
										(Input.mousePosition.y - dragOrigin.y) * dragSpeed, 
										0.0f);
										
			//move camera with click and drag by move vector
			galaxyOverworld.MoveThroughSectionWithClickAndDrag(move);
		}
    }
		
}
