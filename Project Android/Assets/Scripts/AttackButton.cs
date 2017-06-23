using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

    //left and right refer to the UI orientation i.e.
    //RightAttack is called by tapping the right button
    public void RightAttack()
    {
        Attack(0);
    }
    public void LeftAttack()
    {
        Attack(1);
    }

	private void Attack(byte type)
    {
        Player player = Player.FindPlayer();
        if (player == null) return;
        if (type == 0)
        {
            player.EquipWeapon(player.GetRightWeapon());
            player.Attack();
        }
        else if (type == 1)
        {
            player.EquipWeapon(player.GetLeftWeapon());
            player.Attack();
        }
    }
}
