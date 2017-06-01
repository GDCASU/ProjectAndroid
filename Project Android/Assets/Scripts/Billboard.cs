using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    //keeps the health bar rotated properly

	void Update () {
        transform.rotation = Quaternion.identity;
	}
}
