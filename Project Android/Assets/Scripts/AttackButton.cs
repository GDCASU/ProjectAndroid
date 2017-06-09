using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

    public void SwordAttack()
    {
        Attack(0);
    }
    public void GunAttack()
    {
        Attack(1);
    }

	private void Attack(byte type)
    {
        GameObject plyObj = GameObject.FindGameObjectWithTag("Player");
        if (plyObj == null) return;
        Player player = plyObj.GetComponent<Player>();
        if (player == null) return;
        if (type == 0)
        {
            player.SwordAttack();
        }
        else if (type == 1)
        {
            player.GunAttack();
        }
    }
}
