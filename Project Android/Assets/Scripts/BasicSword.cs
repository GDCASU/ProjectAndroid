using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : Item {
    public override void PerformAttack(Tile target)
    {
        GetTileMap().DamageTile(target, 2, GetUnit().getID());
        //TODO change once attack patterns are implemented
    }
}
