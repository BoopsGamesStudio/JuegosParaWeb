using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics
{
    public int playerId;
    public float impact;
    public float movementSpeed;
    public float endurance;
    public List<Item> inventory;

    public string getStats()
    {
        return "[" + playerId + ", " + impact + ", " + endurance + ", " + movementSpeed + ", " + "]";
    }
}

