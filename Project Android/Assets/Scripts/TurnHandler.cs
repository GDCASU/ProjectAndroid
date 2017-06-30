using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour {

    static TurnHandler handler;

    List<Unit> turnQueue; //can't be a true queue because it must support insertion
    Unit activeUnit;

    public TurnHandler()
    {
        handler = this;
        turnQueue = new List<Unit>();
    }

    //add a unit to waiting queue
    public void Queue(Unit unit)
    {
        unit.turnTimer = 1 / unit.speed;
        for (int i = 0; i < turnQueue.Count; i++)
        {
            if (turnQueue[i].turnTimer > unit.turnTimer)
            {
                turnQueue.Insert(i, unit);
                return;
            }
        }
        turnQueue.Add(unit); //add to end if it has the biggest timer
        Debug.Log("Turn queue: " + turnQueue);
    }

    //can be used to give a unit absolute initiative
    public void QueueImmediate(Unit unit)
    {
        unit.turnTimer = 0;
        turnQueue.Insert(0, unit);
    }

    public void DoNextTurn()
    {
        if (activeUnit)
        {
            activeUnit.isActiveUnit = false;
            Queue(activeUnit);
        }
        if (turnQueue.Count > 0)
        {
            activeUnit = turnQueue[0];
            turnQueue.RemoveAt(0);
            foreach (Unit u in turnQueue)
                u.turnTimer -= activeUnit.turnTimer;
        }
        activeUnit.DoTurn();
        
    }

    public void EmptyQueue()
    {
        turnQueue.Clear();
    }
	
    //assumes the current unit's downkeep has already executed
	public static void NextTurn()
    {
        handler.DoNextTurn();
    }
}
