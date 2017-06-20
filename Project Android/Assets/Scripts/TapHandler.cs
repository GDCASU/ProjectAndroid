using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: This class handles taps when the control method is set to use it.
//              It simply finds the tile that was tapped and passes that information to the player.

public class TapHandler : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            //if touch started
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                    return;
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile)
                {
                    Player player = Player.FindPlayer();
                    if(player)
                        player.MoveTileTap(tile);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile)
            {
                Player player = Player.FindPlayer();
                if (player)
                    player.MoveTileTap(tile);
            }
        }
    }
}
