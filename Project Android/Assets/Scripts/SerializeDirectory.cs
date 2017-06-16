using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: This class is attached to a prefab and serves as a common directory
//              so that tiles and units can be saved, loaded, and used in the map editor
//              and have unique IDs.

public class SerializeDirectory : MonoBehaviour {

    public GameObject[] tileList;
    public GameObject[] unitList;

}
