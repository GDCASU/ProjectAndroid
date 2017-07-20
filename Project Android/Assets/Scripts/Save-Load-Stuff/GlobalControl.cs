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
		Date: 07/08/17
		Description: I made this class to handle saving and loading data between scenes and from a save file.
		* 			The code was based on this tutorial and others after it. https://www.sitepoint.com/saving-data-between-scenes-in-unity/ 
	*/

	/*
		Making GlobalControl Instance static to make it a singleton.
		The Singleton concept ensures that if there is another copy of the object 
		with this same script attached (and there will be, you need to put this object into every scene), 
		then the other object will be destroyed and this one (original) will be saved. 
	 */
	 
	/*
		The workflow for the Save Load System works like this for player data and other object's data as well.
		
		1. Initial default data object is initialized with default values and used if starting a new game.
		   Saved local temporary data object is initialized as null.
		
		2. If choose to start new game, saved local temporary data is kept null.
		   If choose to load from savefile, local copy of data for savefile object is initialized with save file data
		   and saved local temporary data object is initialized with save file data.
		
		3.Within Player class, there is a local data object that is initialized with data from 
		 	(1) initial default data object if saved local temporary data is null or 
		 	(2) initialized with data from saved local temporary data object if saved local temporary data object is not null.
			
			-Local data object of Player class is updated within player class if player sets left and/or right weapon. 
		
		4.(Not Yet Implemented)
			If moving to overworld from InGame, Saved local temporary data object is assigned data from player local data object
			which is more up to date.
			
			If moving to InGame from overworld, Player local data object is assigned data from Saved local temporary data object.
			
		5. Saving Data to savefile
			-If choose to save data to savefile, local copy of data for savefile is assigned data from saved local temporary data object. 
			The local copy of data for savefile object is then serialized and put into save file.
			
			*Note: Saved local temporary data object is not directly saved to savefile because want to have option for player to keep
			playing after saving and choose not to save later progress. 
			It also makes it easier to have a variable for temporary data and a separate variable for permanent data to put in savefile.
			It also gives option to open savefile once when player quits game as opposed to opening savefile
			every time player chooses to save data.
	 */
	public static GlobalControl Instance;

	public PlayerData savedLocalPlayerDataTemporary; //exists to have player data saved temporarily and not in savefile
	
	public PlayerData localCopyOfPlayerDataForSavefile; //exists to load copy of player data from savefile

	public PlayerData initialDefaultPlayerData; //data for player to start with

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
	
	
	//function to save data to savefile
	public void SaveDataToSaveFile()
    {
		Debug.Log("Save Data To Savefile called! \n");
		//if directory called saves doesn't exist, create the directory
        if (!Directory.Exists(saveFileDirectory)){Directory.CreateDirectory(saveFileDirectory);}
        
        //create a file called save.binary    
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(saveFileDirectory + "/save.binary");

        //save player data from player to global control			
        if(savedLocalPlayerDataTemporary != null)
        {
			//assign saved local data in global control to local copy of player data for save file
			localCopyOfPlayerDataForSavefile = savedLocalPlayerDataTemporary; 
		
			//write local copy of player data in binary form to save file
			formatter.Serialize(saveFile,localCopyOfPlayerDataForSavefile);
			saveFile.Close(); //close save file
		}
        
    }
    
	//function to load data from savefile
    public void LoadDataFromSaveFile()
    {
		//open save file
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(saveFileDirectory + "/save.binary", FileMode.Open);

		//assign save file data to local copy of player data
        localCopyOfPlayerDataForSavefile = (PlayerData)formatter.Deserialize(saveFile);
        //close save file
        saveFile.Close();
        
        //assign local copy of player data to temporary save data
        if(localCopyOfPlayerDataForSavefile != null){savedLocalPlayerDataTemporary = localCopyOfPlayerDataForSavefile;}
    }
    
    public static GlobalControl GetGlobalControlSaveLoadObject()
    {	
		GameObject gcObj = GameObject.FindGameObjectWithTag("Global Save Load Object");
        if (gcObj == null){return null;}
        else{return gcObj.GetComponent<GlobalControl>();}
	}
	
	// Use this for initialization
	void Start () 
	{
		//initialize save file directory
		saveFileDirectory = Application.dataPath + "/GameSaveFiles";
		
		//initialize local temporary player data as null
		savedLocalPlayerDataTemporary = null;
		
		//initialize values for local player data that aren't in savefile
		//left weapon 
		int leftDamage = 3;
		initialDefaultPlayerData.leftWeaponDamage = leftDamage;
		int leftRange = 7;
		initialDefaultPlayerData.leftWeaponRange = leftRange;
		string leftName = "My Ass Initial";
		initialDefaultPlayerData.leftWeaponName = leftName;
		
		//right weapon 
		int rightDamage = 3;
		initialDefaultPlayerData.rightWeaponDamage = rightDamage;
		int rightRange = 7;
		initialDefaultPlayerData.rightWeaponRange = rightRange;
		string rightName = "My Mass Initial";
		initialDefaultPlayerData.rightWeaponName = rightName;
		
		//player health
		int health = 20;
		initialDefaultPlayerData.health = health;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
