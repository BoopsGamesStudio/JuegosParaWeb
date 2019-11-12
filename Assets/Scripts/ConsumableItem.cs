using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public ConsumableItem(string name) : base(name)
    {

    }

    public override string getAttribs()
    {
        return "[" + getName() + "]";
    }
}
