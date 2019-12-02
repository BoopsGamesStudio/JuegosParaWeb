using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : Item
{
    private string[] buffAttribs = {
                                     "SbuffS", "Speed", "Small", "1",
                                     "MbuffS", "Speed", "Medium", "3",
                                     "LbuffS", "Speed", "Large", "5",
                                     "SbuffI", "Impact", "Small", "1",
                                     "MbuffI", "Impact", "Medium", "3",
                                     "LbuffI", "Impact", "Large", "5",
                                     "SbuffE", "Endurance", "Small", "1",
                                     "MbuffE", "Endurance", "Medium", "3",
                                     "LbuffE", "Endurance", "Large", "5"
                                    };
    public enum buffType {Impact, Endurance, Speed};
    public enum buffSize {Small, Medium, Large};
    private buffType type;
    private buffSize size;
    private int buff;

    public BuffItem(string name) : base(name)
    {
        var index = System.Array.IndexOf(buffAttribs, name);
        type = (buffType)System.Enum.Parse(typeof(buffType), buffAttribs[index + 1]);
        size = (buffSize)System.Enum.Parse(typeof(buffSize), buffAttribs[index + 2]);
        buff = int.Parse(buffAttribs[index + 3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
    }

    public int getBuff()
    {
        return buff;
    }

    public buffType getType()
    {
        return type;
    }

    public buffSize getSize()
    {
        return size;
    }

    public override string getAttribs()
    {
        return "[" + getName() + ", " + getType() + ", " + getSize() + ", " + getBuff() + "]";
    }
    
}
