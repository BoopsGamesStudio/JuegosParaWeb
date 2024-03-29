﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics
{
    public string model;
    public int playerId;
    public float impact;
    public float movementSpeed;
    public float endurance;
    public List<Item> inventory;

    public string getStats()
    {
        return "[" + playerId + ", " + impact + ", " + endurance + ", " + movementSpeed + ", " + "]";
    }

    public Weapon getWeapon()
    {
        Weapon weapon = (Weapon)inventory.Find((x) => x is Weapon);
        return weapon;
    }

    public Weapon.weaponType getWeaponType()
    {
        Weapon weapon = (Weapon) inventory.Find((x) => x is Weapon);
        return weapon.getType();
    }
}

