using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour {

    public void Attack()
    {
        GameObject plyObj = GameObject.FindGameObjectWithTag("Player");
        if (plyObj == null) return;
        Player player = plyObj.GetComponent<Player>();
        if (player == null) return;
        player.Shoot();
    }
}
