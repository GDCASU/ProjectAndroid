using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : Item {
    public override void PerformAttack(Tile target)
    {
        GetTileMap().DamageTile(target, 10, GetUnit().getID());
        //TODO change once attack patterns are implemented
    }
}
