using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    [SerializeField] protected string name;


    public Item (string name)
    {
        this.name = name;
    }

    public string getName()
    {
        return name;
    }

    public virtual string getAttribs()
    {
        return "";
    }
}
