using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : Component
{
    [SerializeField] protected string iName;


    public Item (string name)
    {
        this.iName = name;
    }

    public string getName()
    {
        return iName;
    }

    public virtual string getAttribs()
    {
        return "";
    }
}
