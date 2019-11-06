using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : Item
{
   public enum buffType {Impact, Endurance, Speed};
    public enum buffSize {Small, Medium, Big};
    [SerializeField] private buffType type;
    [SerializeField] private buffSize size;
    [SerializeField] private int buff;

    public BuffItem(string name, buffType type, buffSize size) : base(name)
    {
        this.type = type;
        switch (size)
        {
            case buffSize.Small:
                this.buff = 1;
                break;

            case buffSize.Medium:
                this.buff = 3;
                break;

            case buffSize.Big:
                this.buff = 5;
                break;

             default:
                break;
        }
    }

    public int getBuff()
    {
        return buff;
    }


}
