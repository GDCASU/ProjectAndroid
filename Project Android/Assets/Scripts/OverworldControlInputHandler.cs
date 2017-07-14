using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Programmer: Pablo Camacho
    Date: 06/14/17
    Description:
    I made this script to include click and drag camera movement with mouse and touch

    Borrowed code for OverworldTapHandler.
*/

public class OverworldControlInputHandler : MonoBehaviour
{
    public LevelMenu levelMenu; //reference to level menu

    public GalaxyOverworldUpdate galaxyOverworld;//reference to Galaxy Overworld

    public float dragSpeed; //how fast camera moves in click and drag
    private Vector3 dragOrigin; //where mouse left click made in world

    public Overlord overlord;

    void Start()
    {
        GameObject overlordObject = GameObject.FindWithTag("Overlord");
        if (overlordObject)
            overlord = overlordObject.GetComponent<Overlord>();
        else
            Debug.LogError("Overlord not found! Please add one to the scene if you wish to skip the title screen.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //if gameobject of overworld ship it
                GameObject owShipGameObject = hit.collider.gameObject;
                OverworldShip owShip = owShipGameObject.transform.parent.GetComponent<OverworldShip>();

                if (owShip)
                {
                    levelMenu.levelMenuGameObject.SetActive(true); //set level menu to active
                    levelMenu.SetInfo(owShip.overworldShipName, owShip, overlord); //pass necessary info to level menu
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //make move vector from difference in position between current mouse position and dragOrigin and multiply it by dragSpeed
            Vector3 move = new Vector3((dragOrigin.x - Input.mousePosition.x) * dragSpeed,
                                        (dragOrigin.y - Input.mousePosition.y) * dragSpeed,
                                        0.0f);

            //move camera with click and drag by move vector
            galaxyOverworld.MoveThroughSectionWithClickAndDrag(move);
            dragOrigin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        }
    }

}
