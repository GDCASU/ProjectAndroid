using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //for using SceneManager
using System.Runtime.Serialization.Formatters.Binary; //for using BinaryFormatter
using System.IO; //for using FileStream



public class GlobalControl : MonoBehaviour {
	
	/*
		Programmer: Pablo Camacho
		Date: 07/18/17
		Description: I made this class to handle saving and loading data between scenes and from a save file.
		* 			The code was based on this tutorial and others after it. https://www.sitepoint.com/saving-data-between-scenes-in-unity/ 
	*/

	/*
		Making GlobalControl Instance static to make it a singleton.
		The Singleton concept ensures that if there is another copy of the object 
		with this same script attached (and there will be, you need to put this object into every scene), 
		then the other object will be destroyed and this one (original) will be saved. 
	 */
	public static GlobalControl Instance;

	public PlayerData savedLocalPlayerData; //exists to have player data saved locally and not in savefile
	
	public PlayerData localCopyOfPlayerData; //exists to load copy of player data from savefile when scene is being loaded

	public bool IsSceneBeingLoaded = false;

	private string saveFileDirectory;
	
	void Awake () 
	{
		//if there is not another instance, don't destory this object in scene that has this instance
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		//if another instance of global control object exists in scene, destroy it
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
	
	//function to save player data from player to global control   
    public void SavePlayerData()
    {
		
		PlayerData localPlayerData = new PlayerData();
		
		if(Player.FindPlayer() != null)
		{
			//get player data from player
			//save player data
			localPlayerData = Player.FindPlayer().localData;
			savedLocalPlayerData = localPlayerData;
		}
		
    }
    
    //function to load player data from global control to player 
    public void LoadPlayerData()
    {
		Weapon leftWeapon = new Weapon();
		leftWeapon.damage = savedLocalPlayerData.leftWeaponDamage;
		leftWeapon.range = savedLocalPlayerData.leftWeaponRange;
		
		Player.FindPlayer().SetLeftWeapon(leftWeapon);
		
		Weapon rightWeapon = new Weapon();
		rightWeapon.damage = savedLocalPlayerData.rightWeaponDamage;
		rightWeapon.range = savedLocalPlayerData.rightWeaponRange;
		
		Player.FindPlayer().SetRightWeapon(rightWeapon);
	}
	
	//function to save data to savefile
	public void SaveData()
    {
		Debug.Log("Save Data called! \n");
		//if directory called saves doesn't exist, create the directory
        if (!Directory.Exists(saveFileDirectory)){Directory.CreateDirectory(saveFileDirectory);}
        
        //create a file called save.binary    
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(saveFileDirectory + "/save.binary");

        SavePlayerData(); //save player data from player to global control
        
        if(savedLocalPlayerData != null)
        {
			localCopyOfPlayerData = savedLocalPlayerData; //assign saved data in global control to local copy of player data
		
			//write local copy of player data in binary form to save file
			formatter.Serialize(saveFile,localCopyOfPlayerData);
			saveFile.Close(); //close save file
		}
        
    }
    
	//function to load data from savefile
    public void LoadData()
    {
		//open save file
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(saveFileDirectory + "/save.binary", FileMode.Open);

		//assign save file data to local copy of player data
        localCopyOfPlayerData = (PlayerData)formatter.Deserialize(saveFile);
        //close save file
        saveFile.Close();
    }
	
	// Use this for initialization
	void Start () 
	{
		//initialize save file directory
		saveFileDirectory = Application.dataPath + "/GameSaveFiles";
		
		//initialize player data
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
