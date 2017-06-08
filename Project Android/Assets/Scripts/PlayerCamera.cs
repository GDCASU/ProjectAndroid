using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for using SceneManager

public class PlayerCamera : MonoBehaviour {

	//camera mode to use
	public enum PlayerCameraMode
	{
		STATIC = 0, FOLLOW
	};
	
	
	
	public PlayerCameraMode currentCameraMode;
	
	//reference to player
	public Player player;
	
	private Vector3 offset; //distance between camera and player
	
	
	// Use this for initialization
	void Start () 
	{
		if(player == null){initPlayerReference();}
		
		//initialize offset, assumes player is at origin
		offset = new Vector3(0,7,-6);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(player == null){initPlayerReference();}
		
	}
	
	//Late Update called after all objects moved
	//best to use for follow camera so that it gets positions of other objects
	void LateUpdate()
	{
		
		//if camera mode is in follow camera mode
		if(currentCameraMode == PlayerCameraMode.FOLLOW)
		{
			//keep main camera at certain distance(offset) from player game object
			Camera.main.transform.position = player.gameObject.transform.position + offset;
		}
	}
	
	public PlayerCameraMode GetPlayerCameraMode(){return currentCameraMode;}
	public void SetPlayerCameraMode(PlayerCameraMode mode){currentCameraMode = mode;}
	
	//function to initialize reference to player game object since player is placed in scene from another scene
	void initPlayerReference()
	{
		// if current scene is in Prototype2 scene
			if(SceneManager.GetActiveScene().name == "Prototype2")
			{
				player = Player.FindPlayer(); //get reference to Player prefab and assign it to player
			}
	}
}
