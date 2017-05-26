using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour {

	//text item to display number of touches registered on screen
	public Text touchCountText;
	
	//text item to position of text
	public Text touchPositionText;
	
	//text item to display debug messages
	public Text debugText;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//display number of touches
		touchCountText.text = Input.touchCount.ToString();
		
		Tile tile = null;
		
		//if touched once or more
		if(Input.touchCount > 0)
		{
			//if touch started
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				//display touch position
				touchPositionText.text = Input.GetTouch(0).position.x.ToString() + " " + Input.GetTouch(0).position.y.ToString();
									
				RaycastHit hit;
				
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit);
				tile = hit.collider.GetComponent<Tile>();
				if(tile){tile.SetColor(Color.yellow);}
			}
			//else if touch eneded
			if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				if(tile){tile.SetColor(Color.gray);}
			}
		}
	}
	
	//function to generate ray from touch location
	Ray GenerateRay(Vector3 touchPos)
	{
		Vector3 positionFar = new Vector3(touchPos.x,
											touchPos.y,
											Camera.main.farClipPlane);
		
		Vector3 positionNear = new Vector3(touchPos.x,
											touchPos.y,
											Camera.main.nearClipPlane);

		Vector3 positionFarRelative = Camera.main.ScreenToWorldPoint(positionFar);
		Vector3 positionNearRelative = Camera.main.ScreenToWorldPoint(positionNear);
		
		Ray thisRay = new Ray(positionFarRelative, positionFarRelative - positionNearRelative);
		return thisRay;
	}
}
