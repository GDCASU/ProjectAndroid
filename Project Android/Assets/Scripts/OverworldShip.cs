using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldShip : MonoBehaviour {
	
    //current status of ship - needs to be saved
	public enum ShipStatus
	{
		Complete = 0, Incomplete, IncompleteSpecial, Invisible
	}

    //ship size - assigned by New Game, needs to be saved
    public enum ShipSize
    {
        Small = 0, Medium, Large
    }

    //ship zone - assigned in inspector, no need to save
    public enum ShipZone
    {
        First = 0, Second, Third
    }
	
	public ShipStatus status;
    public ShipSize size;
    public ShipZone zone;
	
	//references to prefabs to ship models for specific types of ship such as completed, incomplete, and special
	public GameObject incompleteShipModel;
	public GameObject completedShipModel;
	public GameObject specialIncompeleteShipModel;
	
	public GameObject visualCapsuleReference; //reference to capsule use to help visualize rotation and position of ship model
	
	private GameObject shipModel; //ship model that will be assigned a reference to any prefab of ship model
	
	public string overworldShipName;
	
	// Use this for initialization
	void Start () 
	{
        //deactivate visual capsule component in overworld ship prefab
        visualCapsuleReference.SetActive(false);

        //depending on the ship type set, activate the game object of the ship type selected and deactive others
        if (status == ShipStatus.Complete)
		{
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(completedShipModel, 
									transform.position, 
									completedShipModel.transform.rotation);
		}
		else if(status == ShipStatus.Incomplete)
		{
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(incompleteShipModel, 
									transform.position, 
									incompleteShipModel.transform.rotation);
		}
		else if(status == ShipStatus.IncompleteSpecial)
		{
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(specialIncompeleteShipModel, 
									transform.position, 
									specialIncompeleteShipModel.transform.rotation);
		}

		if(status != ShipStatus.Invisible)
		{
            //set parent for ship model to that of visual capsule reference which is the overworld ship
            shipModel.transform.SetParent(visualCapsuleReference.transform.parent);
            //set to shipModel gameobject to active
            shipModel.SetActive(true);
        }
		
	}
}
