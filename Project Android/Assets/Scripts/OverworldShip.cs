using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldShip : MonoBehaviour {
	
	public enum ModelShipType
	{
		COMPLETED = 0, INCOMPLETE, SPECIAL_INCOMPLETE, INVISBLE
	};
	
	public ModelShipType shipType;
	
	//references to prefabs to ship models for specific types of ship such as completed, incomplete, and special
	public GameObject inCompleteShipModel;
	public GameObject completedShipModel;
	public GameObject specialIncompeleteShipModel;
	
	public GameObject visualCapsuleReference; //reference to capsule use to help visualize rotation and position of ship model
	
	private GameObject shipModel; //ship model that will be assigned a reference to any prefab of ship model
	
	public string sceneLevelName; //name of scene that contains level to enter 
	
	public string overworldShipName;
	
	// Use this for initialization
	void Start () 
	{
		
		
		//depending on the ship type set, activate the game object of the ship type selected and deactive others
		if(shipType == ModelShipType.COMPLETED)
		{
			//deactivate visual capsule component in overworld ship prefab
			visualCapsuleReference.SetActive(false);
			
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(completedShipModel, 
									new Vector3(transform.position.x, transform.position.y, transform.position.z), 
									completedShipModel.transform.rotation);
			//set parent for ship model to that of visual capsule reference which is the overworld ship
			shipModel.transform.SetParent (visualCapsuleReference.transform.parent);
			//set to shipModel gameobject to active
			shipModel.SetActive(true); 
			 
		}
		else if(shipType == ModelShipType.INCOMPLETE)
		{
			//deactivate visual capsule component in overworld ship prefab
			visualCapsuleReference.SetActive(false);
			
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(inCompleteShipModel, 
									new Vector3(transform.position.x, transform.position.y, transform.position.z), 
									inCompleteShipModel.transform.rotation);
			//set parent for ship model to that of visual capsule reference which is the overworld ship
			shipModel.transform.SetParent (visualCapsuleReference.transform.parent);
			//set to shipModel gameobject to active
			shipModel.SetActive(true); 
		}
		else if(shipType == ModelShipType.SPECIAL_INCOMPLETE)
		{
			//deactivate visual capsule component in overworld ship prefab
			visualCapsuleReference.SetActive(false);
			
			
			//assign a copy of prefab for completed ship model to shipModel 
			shipModel = Instantiate(specialIncompeleteShipModel, 
									new Vector3(transform.position.x, transform.position.y, transform.position.z), 
									specialIncompeleteShipModel.transform.rotation);
			//set parent for ship model to that of visual capsule reference which is the overworld ship
			shipModel.transform.SetParent (visualCapsuleReference.transform.parent);
			//set to shipModel gameobject to active
			shipModel.SetActive(true); 
		}
		else if(shipType == ModelShipType.INVISBLE)
		{
			//deactivate visual capsule component in overworld ship prefab
			visualCapsuleReference.SetActive(false);
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
